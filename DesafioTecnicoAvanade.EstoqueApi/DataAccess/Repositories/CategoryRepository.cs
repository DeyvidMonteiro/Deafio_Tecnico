using DesafioTecnicoAvanade.EstoqueApi.DataAccess.Context;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.UnitOfWork;
using DesafioTecnicoAvanade.EstoqueApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnicoAvanade.EstoqueApi.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRideOnlyRepository, ICategoryWriteOlyRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryRepository(AppDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesProducts()
        {
            return await _dbContext.Categories.Include(c => c.Products).ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await _dbContext.Categories.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Category> Create(Category category)
        {
            _dbContext.Categories.Add(category);
            await _unitOfWork.Commit();
            return category;
        }

        public async Task<Category> Update(Category category)
        {
            _dbContext.Entry(category).State = EntityState.Modified;
            await _unitOfWork.Commit();
            return category;
        }

        public async Task<Category> Delete(int id)
        {
            var category = await GetById(id);
            _dbContext.Categories.Remove(category);
            await _unitOfWork.Commit();
            return category;
        }

    }
}
