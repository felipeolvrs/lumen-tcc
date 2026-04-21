namespace Tests.Services
{
    [TestClass]
    public class ProdutoTest
    {
        private AppDbContext _context;
        private IProdutoService _service;
        private Guid _defaultUserId;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            var repository = new ProdutoRepository(_context); 
            _service = new ProdutoService(repository);
            
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
                Nome = "Usuário Teste", 
                Email = "teste@lumen.com", 
                Ativo = true,
                TenantId = _defaultUserId 
            };
            
            _context.Set<Usuario>().Add(usuario);
            _context.SaveChanges();
        }

        [DataTestMethod]
        [DataRow("Criar_Produto_Completo", "Monitor UltraWide", 2500.00)]
        [DataRow("Criar_Produto_Simples", "Mouse", 50.00)]
        public async Task CriarAsync_DadosValidos_DevePersistir(string cenario, string nome, double precoDouble)
        {
            // Arrange
            var request = new ProdutoRequest { Nome = nome, Preco = (decimal)precoDouble };

            // Act
            var result = await _service.CriarAsync(_defaultUserId, request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nome, result.Nome);
            
            var noBanco = await _context.Set<Produto>().AnyAsync(p => p.Id == result.Id && p.TenantId == _defaultUserId);
            Assert.IsTrue(noBanco, "O produto deve existir no banco vinculado ao Tenant correto.");
        }

        [DataTestMethod]
        [DataRow("Nome_Vazio", "", 10.00, "obrigatório")]
        [DataRow("Preco_Zero", "Teclado", 0.00, "maior que zero")]
        public async Task CriarAsync_DadosInvalidos_DeveLancarErro(string cenario, string nome, double precoDouble, string erroEsperado)
        {
            // Arrange
            var request = new ProdutoRequest { Nome = nome, Preco = (decimal)precoDouble };

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => 
                _service.CriarAsync(_defaultUserId, request));

            Assert.IsTrue(ex.Message.Contains(erroEsperado, StringComparison.OrdinalIgnoreCase));
        }
        
        [DataTestMethod]
        [TestCategory("Validações de Negócio")]
        [DataRow("Nome_Vazio", "", 15.90, "obrigatório")]
        [DataRow("Preço_Zero", "Produto Teste", 0.00, "maior que zero")]
        [DataRow("Preço_Negativo", "Produto Teste", -5.00, "maior que zero")]
        public async Task CriarAsync_DadosInvalidos_DeveLancarArgumentException(string cenario, string nome, double preco, string erroEsperado)
        {
            // 1. Arrange (Preparação)
            var request = new ProdutoRequest { Nome = nome, Preco = (decimal)preco };

            // 2. Act (Ação)
            var excecao = await Assert.ThrowsExceptionAsync<ArgumentException>(() => 
                _service.CriarAsync(_defaultUserId, request));

            // 3. Assert (Verificação)
            Assert.IsTrue(excecao.Message.Contains(erroEsperado, StringComparison.OrdinalIgnoreCase), 
                $"Falha no cenário: {cenario}");
        }

        [DataTestMethod]
        [DataRow("Atualizar_Sucesso", true)]
        [DataRow("Atualizar_Usuario_Errado", false)]
        public async Task AtualizarAsync_VerificarSeguranca(string cenario, bool mesmoUsuario)
        {
            // Arrange
            var criado = await _service.CriarAsync(_defaultUserId, new ProdutoRequest { Nome = "Original", Preco = 10 });
            var updateId = mesmoUsuario ? _defaultUserId : Guid.NewGuid();
            var request = new ProdutoRequest { Nome = "Alterado", Preco = 20 };

            // Act
            var sucesso = await _service.AtualizarAsync(criado.Id, updateId, request);

            // Assert
            Assert.AreEqual(mesmoUsuario, sucesso);
        }

        [DataTestMethod]
        [DataRow("Excluir_Sucesso", true)]
        public async Task ExcluirAsync_DeveRemoverRegistro(string cenario, bool existe)
        {
            // Arrange
            var criado = await _service.CriarAsync(_defaultUserId, new ProdutoRequest { Nome = "Deletar", Preco = 10 });

            // Act
            var sucesso = await _service.ExcluirAsync(criado.Id, _defaultUserId);

            // Assert
            Assert.IsTrue(sucesso);
            var aindaExiste = await _context.Set<Produto>().AnyAsync(p => p.Id == criado.Id);
            Assert.IsFalse(aindaExiste);
        }
    }
}