using Domain.Entities;
using Domain.Requests;
using Domain.Responses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<IEnumerable<ProdutoResponse>> ObterTodosAsync()
    {
        IEnumerable<Produto> produtos = await _produtoRepository.GetAllAsync();
        return produtos.Select(MapToResponse);
    }

    public async Task<ProdutoResponse?> ObterPorIdAsync(Guid id)
    {
        if (id == Guid.Empty) 
            throw new ArgumentException("ID inválido.");

        var produto = await _produtoRepository.GetByIdAsync(id);
        return produto is null ? null : MapToResponse(produto);
    }

    public async Task<ProdutoResponse> CriarAsync(Guid userId, ProdutoRequest dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("O nome do produto é obrigatório.");

        if (dto.Preco <= 0)
            throw new ArgumentException("O preço deve ser maior que zero.");

        var produto = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            Categoria = dto.Categoria,
            UrlImagem = dto.UrlImagem,
            UsuarioId = userId,
            TenantId = userId,
            LinkAfiliado = dto.LinkAfiliado,
            ReviewPessoal = dto.ReviewPessoal,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _produtoRepository.AddAsync(produto);
        return MapToResponse(produto);
    }

    public async Task<bool> AtualizarAsync(Guid id, Guid userId, ProdutoRequest dto)
    {
        if (id == Guid.Empty) 
            throw new ArgumentException("ID inválido.");
        
        if (dto.Preco <= 0) 
            throw new ArgumentException("O preço deve ser maior que zero.");

        var produto = await _produtoRepository.GetByIdAsync(id);
        
        if (produto is null || produto.UsuarioId != userId)
            return false;

        UpdateEntity(produto, dto);
        
        await _produtoRepository.UpdateAsync(produto);
        return true;
    }

    public async Task<bool> ExcluirAsync(Guid id, Guid userId)
    {
        if (id == Guid.Empty) 
            return false;

        var produto = await _produtoRepository.GetByIdAsync(id);
        
        if (produto is null || produto.UsuarioId != userId)
            return false;

        await _produtoRepository.DeleteAsync(produto.Id);
        return true;
    }

    public async Task<List<string>> ListarCategoriasDoUsuarioAsync(Guid userId)
    {
        if (userId == Guid.Empty) 
            throw new ArgumentException("Usuário inválido.");
        
        return await _produtoRepository.ListarCategoriasDoUsuarioAsync(userId);
    }
    
    private ProdutoResponse MapToResponse(Produto produto)
    {
        return new ProdutoResponse
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Categoria = produto.Categoria,
            UrlImagem = produto.UrlImagem,
            UsuarioId = produto.UsuarioId,
            LinkAfiliado = produto.LinkAfiliado,
            ReviewPessoal = produto.ReviewPessoal,
            CreatedAt = produto.CreatedAt,
            UpdatedAt = produto.UpdatedAt
        };
    }

    private void UpdateEntity(Produto produto, ProdutoRequest dto)
    {
        produto.Nome = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.Preco = dto.Preco;
        produto.Categoria = dto.Categoria;
        produto.UrlImagem = dto.UrlImagem;
        produto.LinkAfiliado = dto.LinkAfiliado;
        produto.ReviewPessoal = dto.ReviewPessoal;
        produto.UpdatedAt = DateTime.UtcNow;
    }
}