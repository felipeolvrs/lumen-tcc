
using Infrastructure.Repositories;

namespace Application.Services;

public class ColecaoService : IColecaoService
{
    private readonly IColecaoRepository _colecaoRepository;
    private readonly IProdutoRepository _produtoRepository;

    public ColecaoService(IColecaoRepository colecaoRepository, IProdutoRepository produtoRepository)
    {
        _colecaoRepository = colecaoRepository;
        _produtoRepository = produtoRepository;
    }

    public ColecaoService(ColecaoRepository colecaoRepository)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ColecaoResponse>> ObterPorUsuarioAsync(Guid userId)
    {
        if (userId == Guid.Empty) 
            throw new ArgumentException("Usuário inválido.");
        
        IEnumerable<Colecao> colecoes = await _colecaoRepository.ObterPorUsuarioAsync(userId);
        return colecoes.Select(MapToResponse).ToList();
    }
    
    public async Task<ColecaoResponse?> ObterPorIdAsync(Guid id)
    {
        if (id == Guid.Empty) 
            throw new ArgumentException("ID inválido.");
        
        var colecao = await _colecaoRepository.GetByIdAsync(id);
        return colecao is null ? null : MapToResponse(colecao);
    }

    public async Task<ColecaoResponse> CriarAsync(Guid userId, CriarColecaoRequest dto)
    {
        var colecao = new Colecao
        {
            Id = Guid.NewGuid(),
            UsuarioId = userId,
            TenantId = userId,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            UrlCapa = dto.UrlCapa,
            Publica = dto.Publica,
            Produtos = new List<Produto>()
        };

        //TODO: Nem preciso dizer que isso precisa de refatoração, né?
        if (dto.ProdutosIds?.Any() is true)
        {
            foreach (var prodId in dto.ProdutosIds.Distinct())
            {
                var produto = await _produtoRepository.GetByIdAsync(prodId);
                
                if (produto is not null) 
                    colecao.Produtos.Add(produto);
            }
        }

        await _colecaoRepository.AddAsync(colecao);
        return MapToResponse(colecao);
    }

    public async Task AtualizarAsync(Guid id, Guid userId, AtualizarColecaoRequest dto)
    {
        var colecao = await _colecaoRepository.GetByIdAsync(id);
    
        if (colecao == null) 
            throw new Exception("Coleção não encontrada.");
        
        if (colecao.UsuarioId != userId) 
            throw new Exception("Acesso negado.");

        colecao.Nome = dto.Nome;
        colecao.Descricao = dto.Descricao;
        colecao.UrlCapa = dto.UrlCapa; 
        colecao.Publica = dto.Publica;
        colecao.UpdatedAt = DateTime.UtcNow;

        List<Guid> novosIds = dto.ProdutosIds?.Distinct().ToList() ?? new List<Guid>();
        
        List<Produto> paraRemover = colecao.Produtos.Where(p => !novosIds.Contains(p.Id)).ToList();
        
        foreach (var p in paraRemover) 
            colecao.Produtos.Remove(p);

        List<Guid> atuaisIds = colecao.Produtos.Select(p => p.Id).ToList();
        IEnumerable<Guid> paraAdicionarIds = novosIds.Except(atuaisIds);

        foreach (var prodId in paraAdicionarIds)
        {
            var produto = await _produtoRepository.GetByIdAsync(prodId);
            
            if (produto is not null) 
                colecao.Produtos.Add(produto);
        }

        await _colecaoRepository.UpdateAsync(colecao);
    }

    public async Task ExcluirAsync(Guid id)
    {
        var colecao = await _colecaoRepository.GetByIdAsync(id);
        
        if (colecao is null) 
            throw new Exception("Coleção não encontrada.");
        
        await _colecaoRepository.DeleteAsync(id);
    }
    
    private ColecaoResponse MapToResponse(Colecao colecao)
    {
        return new ColecaoResponse
        {
            Id = colecao.Id,
            UsuarioId = colecao.UsuarioId,
            Nome = colecao.Nome,
            Descricao = colecao.Descricao,
            UrlCapa = colecao.UrlCapa,
            Publica = colecao.Publica,
            CreatedAt = colecao.CreatedAt,
            UpdatedAt = colecao.UpdatedAt,
            Produtos = colecao.Produtos?.Select(p => new ProdutoSimplificadoResponse 
            { 
                Id = p.Id, 
                Nome = p.Nome, 
                Preco = p.Preco,
                UrlImagem = p.UrlImagem 
            }).ToList() ?? new()
        };
    }
}