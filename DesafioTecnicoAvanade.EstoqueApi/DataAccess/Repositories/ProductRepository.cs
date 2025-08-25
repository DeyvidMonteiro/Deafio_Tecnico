using DesafioTecnicoAvanade.EstoqueApi.DataAccess.Context;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.UnitOfWork;
using DesafioTecnicoAvanade.EstoqueApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnicoAvanade.EstoqueApi.DataAccess.Repositories;

public class ProductRepository : IProductWriteOnlyRepository, IProductReadOnlyRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public ProductRepository(AppDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        return await _dbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Product> Create(Product product)
    {
        _dbContext.Products.Add(product);
        await _unitOfWork.Commit();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        _dbContext.Entry(product).State = EntityState.Modified;
        await _unitOfWork.Commit();
        return product;
    }


    public async Task Decrement(int productId, long quantity)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var product = await _dbContext.Products
                .FromSqlRaw("SELECT * FROM Products WITH (UPDLOCK) WHERE Id = {0}", productId).FirstOrDefaultAsync();

            if (product == null || product.Stock < quantity)
            {
                await transaction.RollbackAsync();
                throw new Exception("Estoque insuficiente ou produto não encontrado.");
            }

            product.Stock -= quantity;
            _dbContext.Products.Update(product);

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public async Task<Product> Delete(int id)
    {
        var product = await GetById(id);
        _dbContext.Products.Remove(product);
        await _unitOfWork.Commit();
        return product;
    }

}
