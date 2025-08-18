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

    public async Task<Product> Delete(int id)
    {
        var product = await GetById(id);
        _dbContext.Products.Remove(product);
        await _unitOfWork.Commit();
        return product;
    }

}
