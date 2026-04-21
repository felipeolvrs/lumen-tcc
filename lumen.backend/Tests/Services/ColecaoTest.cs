namespace Tests.Services
{
    [TestClass]
    public class ColecaoTest
    {
        private AppDbContext _context;
        private IColecaoService _service;
        private Guid _defaultUserId;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            
            // Instanciando os repositórios necessários para o Service
            var colecaoRepo = new ColecaoRepository(_context);
            var produtoRepo = new ProdutoRepository(_context);
            
            _service = new ColecaoService(colecaoRepo, produtoRepo);
            _defaultUserId = Guid.NewGuid();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [DataTestMethod]
        [DataRow("Verão 2026", "Produtos de praia")]
        [DataRow("Inverno 2026", "")]
        public async Task CriarAsync_DadosValidos_DevePersistir(string nome, string descricao)
        {
            var request = new CriarColecaoRequest { Nome = nome, Descricao = descricao };

            var result = await _service.CriarAsync(_defaultUserId, request);

            Assert.IsNotNull(result);
            Assert.AreEqual(nome, result.Nome);
            Assert.AreEqual(_defaultUserId, result.UsuarioId);

            var noBanco = await _context.Set<Colecao>().FindAsync(result.Id);
            Assert.IsNotNull(noBanco);
            Assert.AreEqual(descricao, noBanco.Descricao);
        }

        [TestMethod]
        public async Task ObterPorUsuarioAsync_DeveRetornarColecoesCorretas()
        {
            var outroUsuario = Guid.NewGuid();
            await _service.CriarAsync(_defaultUserId, new CriarColecaoRequest { Nome = "Col 1" });
            await _service.CriarAsync(_defaultUserId, new CriarColecaoRequest { Nome = "Col 2" });
            await _service.CriarAsync(outroUsuario, new CriarColecaoRequest { Nome = "Col Outro" });

            var lista = await _service.ObterPorUsuarioAsync(_defaultUserId);

            Assert.AreEqual(2, lista.Count);
            Assert.IsTrue(lista.All(x => x.UsuarioId == _defaultUserId));
        }

        [TestMethod]
        public async Task AtualizarAsync_DadosValidos_DeveAlterarCampos()
        {
            // Arrange
            var criado = await _service.CriarAsync(_defaultUserId, new CriarColecaoRequest { Nome = "Original" });
            var requestUpdate = new AtualizarColecaoRequest 
            { 
                Nome = "Editado", 
                Descricao = "Nova Desc" 
            };

            await _service.AtualizarAsync(criado.Id, _defaultUserId, requestUpdate);

            // Assert
            var atualizado = await _context.Set<Colecao>().FindAsync(criado.Id);
            Assert.AreEqual("Editado", atualizado.Nome);
            Assert.AreEqual("Nova Desc", atualizado.Descricao);
        }

        [TestMethod]
        public async Task AtualizarAsync_UsuarioErrado_DeveLancarErro()
        {
            var criado = await _service.CriarAsync(_defaultUserId, new CriarColecaoRequest { Nome = "Original" });
            var hackerId = Guid.NewGuid();

            await Assert.ThrowsExceptionAsync<Exception>(() => 
                _service.AtualizarAsync(criado.Id, hackerId, new AtualizarColecaoRequest { Nome = "Hack" }));
        }

        [TestMethod]
        public async Task ExcluirAsync_Existente_DeveDeletar()
        {
            var criado = await _service.CriarAsync(_defaultUserId, new CriarColecaoRequest { Nome = "Lixo" });
            
            await _service.ExcluirAsync(criado.Id);

            var existe = await _context.Set<Colecao>().AnyAsync(c => c.Id == criado.Id);
            Assert.IsFalse(existe);
        }

        [TestMethod]
        public async Task GerenciarProdutos_ViaAtualizar_DeveSincronizarLista()
        {
            var colecao = await _service.CriarAsync(_defaultUserId, new CriarColecaoRequest { Nome = "Vitrine" });
            var produto = new Produto { Id = Guid.NewGuid(), Nome = "Produto Teste", Preco = 10, TenantId = _defaultUserId };
            
            _context.Set<Produto>().Add(produto);
            await _context.SaveChangesAsync();

            // 2. Adicionar produto via AtualizarAsync (usando a lista de IDs)
            var requestComProduto = new AtualizarColecaoRequest 
            { 
                Nome = "Vitrine", 
                ProdutosIds = new List<Guid> { produto.Id } 
            };
            
            await _service.AtualizarAsync(colecao.Id, _defaultUserId, requestComProduto);

            var noBanco = await _context.Set<Colecao>().Include(c => c.Produtos).FirstAsync(c => c.Id == colecao.Id);
            Assert.AreEqual(1, noBanco.Produtos.Count);

            requestComProduto.ProdutosIds = new List<Guid>();
            await _service.AtualizarAsync(colecao.Id, _defaultUserId, requestComProduto);

            var aposRemover = await _context.Set<Colecao>().Include(c => c.Produtos).FirstAsync(c => c.Id == colecao.Id);
            Assert.AreEqual(0, aposRemover.Produtos.Count);
        }
    }
}