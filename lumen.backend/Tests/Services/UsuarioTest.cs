namespace Tests.Services;

[TestClass]
public class UsuarioTest
{
    private AppDbContext _context;
    private IUsuarioService _service;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _service = new Application.Services.UsuarioService(new Infrastructure.Repositories.UsuarioRepository(_context));
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [DataTestMethod]
    [DataRow("Felipe", "felipe@email.com", "123")]
    [DataRow("Maria", "maria@email.com", "senhaForte")]
    public async Task CriarAsync_DadosValidos_DeveSalvarComHash(string nome, string email, string senha)
    {
        var request = new CriarUsuarioRequest 
        { 
            Nome = nome, 
            Email = email, 
            Senha = senha 
        };

        var result = await _service.CriarAsync(request);

        Assert.IsNotNull(result);
        
        var noBanco = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == result.Id);
        
        Assert.IsNotNull(noBanco);
        Assert.AreNotEqual(senha, noBanco.SenhaHash);
    }

    [DataTestMethod]
    [DataRow("Novo Nome", "test@test.com", "Nova Bio", "5511999999999", "novo-nome")]
    [DataRow("Minha Loja Gamer", "loja@gamer.com", "Bio da Loja", "5511888888888", "minha-loja-gamer")]
    public async Task AtualizarAsync_DadosPerfil_DeveAtualizarEFormatarSlug(
        string novoNome, string email, string bio, string whatsapp, string expectedSlug)
    {
        var criado = await _service.CriarAsync(new CriarUsuarioRequest { Nome = "User", Email = "inicial@test.com", Senha = "123" });
        
        var updateRequest = new AtualizarUsuarioRequest 
        { 
            Nome = novoNome, 
            Email = email,
            Bio = bio,
            Whatsapp = whatsapp
        };

        var sucesso = await _service.AtualizarAsync(criado.Id, updateRequest);

        Assert.IsTrue(sucesso);
        
        var noBanco = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == criado.Id);
        
        Assert.AreEqual(novoNome, noBanco.Nome);
        Assert.AreEqual(expectedSlug, noBanco.Slug);
        Assert.AreEqual(whatsapp, noBanco.WhatsApp);
    }

    [DataTestMethod]
    [DataRow("a@a.com", "b@b.com")]
    [DataRow("primeiro@email.com", "segundo@email.com")]
    public async Task AtualizarAsync_EmailEmUsoPorOutro_DeveLancarExcecao(string emailUsuarioA, string emailUsuarioB)
    {
        await _service.CriarAsync(new CriarUsuarioRequest { Nome = "User A", Email = emailUsuarioA, Senha = "123" });
        var userB = await _service.CriarAsync(new CriarUsuarioRequest { Nome = "User B", Email = emailUsuarioB, Senha = "123" });

        var requestConflito = new AtualizarUsuarioRequest { Nome = "User B", Email = emailUsuarioA };

        var ex = await Assert.ThrowsExceptionAsync<Exception>(() => 
            _service.AtualizarAsync(userB.Id, requestConflito));

        Assert.AreEqual("Este e-mail já está em uso por outra conta.", ex.Message);
    }

    [DataTestMethod]
    [DataRow("Felipe", "f@f.com", "123")]
    [DataRow("Admin", "admin@sys.com", "admin123")]
    public async Task ObterPerfilPorIdAsync_DeveRetornarResponseSemDadosSensiveis(string nome, string email, string senha)
    {
        var criado = await _service.CriarAsync(new CriarUsuarioRequest { Nome = nome, Email = email, Senha = senha });

        var response = await _service.ObterPerfilPorIdAsync(criado.Id);

        Assert.IsNotNull(response);
        Assert.AreEqual(nome, response.Nome);
    }
}