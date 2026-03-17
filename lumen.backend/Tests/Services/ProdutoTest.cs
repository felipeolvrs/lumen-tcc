namespace Tests.Services
{
    [TestClass]
    public class ProdutoTest
    {
        private AppDbContext _context;
        private ServiceFactory _factory;
        private IProdutoService _service;
        private Guid _defaultUserId;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _factory = new ServiceFactory();
            _service = _factory.ProdutoService(_context);
            
            SeedDatabase();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            _defaultUserId = Guid.NewGuid();
            var usuario = new Usuario 
            { 
                Id = _defaultUserId, 
                Nome = "Usuário Padrão", 
                Email = "teste@teste.com", 
                Ativo = true 
            };
            
            _context.Set<Usuario>().Add(usuario);
            _context.SaveChanges();
        }

        [DataTestMethod]
        [DataRow("Criar_Produto_Completo", "Monitor UltraWide", 2500.00, "34 polegadas", "Monitores", "http://teste.com/img.jpg")]
        [DataRow("Criar_Produto_Apenas_Obrigatorios", "Mouse Básico", 50.00, null, null, null)]
        public async Task CriarAsync_DadosValidos_DevePersistirComIntegridade(
            string cenario, string nome, double precoDouble, string descricao, string categoria, string urlImagem)
        {
            decimal preco = (decimal)precoDouble;
            var request = new ProdutoRequest 
            { 
                Nome = nome, 
                Preco = preco,
                Descricao = descricao,
                Categoria = categoria,
                UrlImagem = urlImagem,
                UsuarioId = _defaultUserId
            };

            var result = await _service.CriarAsync(request);

            _context.ChangeTracker.Clear();

            Assert.IsNotNull(result, $"Cenário: {cenario}");
            Assert.AreNotEqual(Guid.Empty, result.Id, $"Cenário: {cenario}");

            var noBanco = await _context.Set<Produto>().FirstOrDefaultAsync(p => p.Id == result.Id);
            Assert.IsNotNull(noBanco, $"Cenário: {cenario}");
            Assert.AreEqual(nome, noBanco.Nome, $"Cenário: {cenario}");
            Assert.AreEqual(preco, noBanco.Preco, $"Cenário: {cenario}");
            Assert.AreEqual(_defaultUserId, noBanco.UsuarioId, $"Cenário: {cenario}");
        }

        [DataTestMethod]
        [DataRow("Validacao_Nome_Vazio", "", 10.00, "nome")]
        [DataRow("Validacao_Preco_Negativo", "Teclado", -50.00, "preço")]
        [DataRow("Validacao_Nome_Gigante", "GIGANTE", 10.00, "limite")] 
        public async Task CriarAsync_DadosInvalidos_DeveLancarErroEspecifico(
            string cenario, string nome, double precoDouble, string expectedErrorSnippet)
        {
            if (nome == "GIGANTE") nome = new string('A', 201);

            var request = new ProdutoRequest 
            { 
                Nome = nome, 
                Preco = (decimal)precoDouble,
                UsuarioId = _defaultUserId
            };

            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => 
                _service.CriarAsync(request), $"Cenário: {cenario}");

            Assert.IsTrue(
                ex.Message.Contains(expectedErrorSnippet, StringComparison.OrdinalIgnoreCase), 
                $"Cenário: {cenario}");
        }

        [DataTestMethod]
        [DataRow("Obter_Produto_Existente", true)]
        [DataRow("Obter_Produto_Inexistente_Retorna_Null", false)]
        public async Task ObterPorIdAsync_RetornosEsperados(string cenario, bool exists)
        {
            Guid targetId = Guid.NewGuid();

            if (exists)
            {
                var criado = await _service.CriarAsync(new ProdutoRequest { Nome = "Item", Preco = 10, UsuarioId = _defaultUserId });
                targetId = criado.Id;
            }

            var resultado = await _service.ObterPorIdAsync(targetId);

            if (exists)
            {
                Assert.IsNotNull(resultado, $"Cenário: {cenario}");
                Assert.AreEqual(targetId, resultado.Id, $"Cenário: {cenario}");
            }
            else
            {
                Assert.IsNull(resultado, $"Cenário: {cenario}");
            }
        }

        [DataTestMethod]
        [DataRow("Atualizar_Existente_Sucesso", true, false)]
        [DataRow("Atualizar_Id_Inexistente_Retorna_False", false, false)]
        [DataRow("Atualizar_Item_Ja_Deletado_Retorna_False", true, true)]
        public async Task AtualizarAsync_RetornosEsperados(string cenario, bool exists, bool isDeleted)
        {
            Guid targetId = Guid.NewGuid();
            var updateRequest = new ProdutoRequest { Nome = "Editado", Preco = 200, UsuarioId = _defaultUserId };

            if (exists)
            {
                var criado = await _service.CriarAsync(new ProdutoRequest { Nome = "Antigo", Preco = 100, UsuarioId = _defaultUserId });
                targetId = criado.Id;

                var produtoDb = await _context.Set<Produto>().FindAsync(criado.Id);
                produtoDb!.CreatedAt = DateTime.UtcNow.AddDays(-1);
                produtoDb.UpdatedAt = DateTime.UtcNow.AddDays(-1);
                await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear();

                if (isDeleted)
                {
                    await _service.ExcluirAsync(targetId);
                }
            }

            var sucesso = await _service.AtualizarAsync(targetId, updateRequest);

            if (exists && !isDeleted)
            {
                Assert.IsTrue(sucesso, $"Cenário: {cenario}");
                
                var noBanco = await _context.Set<Produto>().AsNoTracking().FirstOrDefaultAsync(p => p.Id == targetId);
                Assert.AreEqual("Editado", noBanco!.Nome, $"Cenário: {cenario}");
                Assert.IsTrue(noBanco.UpdatedAt > noBanco.CreatedAt, $"Cenário: {cenario}");
            }
            else
            {
                Assert.IsFalse(sucesso, $"Cenário: {cenario}");
            }
        }

        [DataTestMethod]
        [DataRow("Excluir_Existente_Retorna_True", true)]
        [DataRow("Excluir_Inexistente_Retorna_False", false)]
        public async Task ExcluirAsync_RetornosEsperados(string cenario, bool exists)
        {
            Guid targetId = Guid.NewGuid();

            if (exists)
            {
                var criado = await _service.CriarAsync(new ProdutoRequest { Nome = "Lixo", Preco = 10, UsuarioId = _defaultUserId });
                targetId = criado.Id;
            }

            var sucesso = await _service.ExcluirAsync(targetId);

            Assert.AreEqual(exists, sucesso, $"Cenário: {cenario}");

            if (exists)
            {
                var noBanco = await _context.Set<Produto>().AnyAsync(p => p.Id == targetId);
                Assert.IsFalse(noBanco, $"Cenário: {cenario}");
            }
        }
    }
}