namespace Tests.Services
{
    [TestClass]
    public class ColecaoTest
    {
        private AppDbContext _context;
        private ServiceFactory _factory;
        private IColecaoService _service;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _factory = new ServiceFactory();
            _service = _factory.ColecaoService(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [DataTestMethod]
        [DataRow("Verão 2026", "Produtos de praia")]
        [DataRow("Inverno 2026", "Roupas de frio")]
        [DataRow("Outono 2026", "")]
        public async Task CriarAsync_DadosValidos_DevePersistirNoBanco(string nome, string descricao)
        {
            var userId = Guid.NewGuid();
            var request = new CriarColecaoRequest 
            { 
                Nome = nome, 
                Descricao = descricao 
            };

            var result = await _service.CriarAsync(userId, request);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(nome, result.Nome);
            Assert.AreEqual(userId, result.UsuarioId);

            var noBanco = await _context.Set<Colecao>().FirstOrDefaultAsync(c => c.Id == result.Id);
            Assert.IsNotNull(noBanco);
            Assert.AreEqual(descricao, noBanco.Descricao);
            Assert.IsTrue((DateTime.UtcNow - noBanco.CreatedAt).TotalSeconds < 2);
        }

        [DataTestMethod]
        [DataRow(true, "")]
        [DataRow(true, "   ")]
        [DataRow(true, null)]
        [DataRow(false, "Nome Válido")]
        public async Task CriarAsync_DadosInvalidos_DeveLancarErro(bool isUsuarioValido, string nome)
        {
            var userId = isUsuarioValido ? Guid.NewGuid() : Guid.Empty;
            var request = new CriarColecaoRequest { Nome = nome, Descricao = "D" };

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => 
                _service.CriarAsync(userId, request));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(3)]
        [DataRow(5)]
        public async Task ObterPorUsuarioAsync_DeveRetornarApenasColecoesDoUsuario(int quantidadeColecoes)
        {
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            for (int i = 0; i < quantidadeColecoes; i++)
            {
                await _service.CriarAsync(userA, new CriarColecaoRequest { Nome = $"Col A{i}" });
            }
            
            await _service.CriarAsync(userB, new CriarColecaoRequest { Nome = "Col B1" });

            var lista = await _service.ObterPorUsuarioAsync(userA);

            Assert.IsNotNull(lista);
            Assert.AreEqual(quantidadeColecoes, lista.Count);
            Assert.IsTrue(lista.All(x => x.UsuarioId == userA));
        }

        [TestMethod]
        public async Task ObterPorUsuarioAsync_UsuarioSemColecao_DeveRetornarListaVazia()
        {
            var lista = await _service.ObterPorUsuarioAsync(Guid.NewGuid());

            Assert.IsNotNull(lista);
            Assert.AreEqual(0, lista.Count);
        }

        [DataTestMethod]
        [DataRow("Novo Nome 1", "Nova Descricao 1")]
        [DataRow("Novo Nome 2", "")]
        public async Task AtualizarAsync_DadosValidos_DeveAtualizarCamposEData(string novoNome, string novaDescricao)
        {
            var criado = await _service.CriarAsync(Guid.NewGuid(), new CriarColecaoRequest { Nome = "Antigo" });
            var dataCriacaoOriginal = criado.CreatedAt;

            await Task.Delay(50);

            var requestUpdate = new AtualizarColecaoRequest 
            { 
                Nome = novoNome, 
                Descricao = novaDescricao 
            };

            await _service.AtualizarAsync(criado.Id, requestUpdate);

            var atualizado = await _context.Set<Colecao>().AsNoTracking().FirstOrDefaultAsync(c => c.Id == criado.Id);
            
            Assert.AreEqual(novoNome, atualizado.Nome);
            Assert.AreEqual(novaDescricao, atualizado.Descricao);
            Assert.AreEqual(dataCriacaoOriginal, atualizado.CreatedAt);
            Assert.IsTrue(atualizado.UpdatedAt > dataCriacaoOriginal);
        }

        [TestMethod]
        public async Task AtualizarAsync_IdInexistente_DeveLancarErro()
        {
            await Assert.ThrowsExceptionAsync<Exception>(() => 
                _service.AtualizarAsync(Guid.NewGuid(), new AtualizarColecaoRequest { Nome = "X" }));
        }

        [TestMethod]
        public async Task ExcluirAsync_Existente_DeveRemoverDoBanco()
        {
            var criado = await _service.CriarAsync(Guid.NewGuid(), new CriarColecaoRequest { Nome = "Deletar" });
            
            await _service.ExcluirAsync(criado.Id);

            var count = await _context.Set<Colecao>().CountAsync();
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public async Task ExcluirAsync_NaoExistente_DeveLancarErro()
        {
            await Assert.ThrowsExceptionAsync<Exception>(() => 
                _service.ExcluirAsync(Guid.NewGuid()));
        }

        [DataTestMethod]
        [DataRow("Batom", 20.0)]
        [DataRow("Esmalte", 15.5)]
        public async Task AdicionarProdutoAsync_DeveVincularProdutoNaColecao(string nomeProduto, double precoDbl)
        {
            var preco = (decimal)precoDbl;
            var colecao = await _service.CriarAsync(Guid.NewGuid(), new CriarColecaoRequest { Nome = "Vitrine" });
            
            var produto = new Produto 
            { 
                Id = Guid.NewGuid(), 
                Nome = nomeProduto, 
                Preco = preco, 
                UsuarioId = colecao.UsuarioId 
            };
            await _context.Set<Produto>().AddAsync(produto);
            await _context.SaveChangesAsync();

            await _service.AdicionarProdutoAsync(colecao.Id, produto.Id);

            var colecaoNoBanco = await _context.Set<Colecao>()
                .Include(c => c.Produtos) 
                .FirstOrDefaultAsync(c => c.Id == colecao.Id);

            Assert.IsNotNull(colecaoNoBanco);
            Assert.IsNotNull(colecaoNoBanco.Produtos);
            Assert.AreEqual(1, colecaoNoBanco.Produtos.Count);
            Assert.AreEqual(produto.Id, colecaoNoBanco.Produtos.First().Id);
        }

        [TestMethod]
        public async Task RemoverProdutoAsync_DeveDesvincularProduto()
        {
            var colecao = await _service.CriarAsync(Guid.NewGuid(), new CriarColecaoRequest { Nome = "Vitrine" });
            var produto = new Produto { Id = Guid.NewGuid(), Nome = "Batom", Preco = 20 };
            
            await _context.Set<Produto>().AddAsync(produto);
            await _context.SaveChangesAsync();
            await _service.AdicionarProdutoAsync(colecao.Id, produto.Id);

            var checkAntes = await _context.Set<Colecao>().Include(c => c.Produtos).FirstAsync(c => c.Id == colecao.Id);
            Assert.AreEqual(1, checkAntes.Produtos.Count);

            await _service.RemoverProdutoAsync(colecao.Id, produto.Id);

            var colecaoApos = await _context.Set<Colecao>()
                .Include(c => c.Produtos)
                .FirstAsync(c => c.Id == colecao.Id);

            Assert.AreEqual(0, colecaoApos.Produtos.Count);
        }
    }
}