using AutoMapper;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.UnitOfWork;
using DesafioTecnicoAvanade.EstoqueApi.DTOs;
using DesafioTecnicoAvanade.EstoqueApi.Filters.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnicoAvanade.EstoqueApi.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IProductReadOnlyRepository _readRepository;
        private readonly IProductWriteOnlyRepository _writeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductReadOnlyRepository readRepository,
            IProductWriteOnlyRepository writeRepository, IMapper mapper,
            IUnitOfWork unitOfWork, ILogger<ProductService> logger)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _readRepository.GetById(id);
            return _mapper.Map<ProductDTO>(product);
        }
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var proudcts = await _readRepository.GetAll();
            return _mapper.Map<IEnumerable<ProductDTO>>(proudcts);
        }
        public async Task AddProduct(ProductDTO productDTO)
        {
            var product = _mapper.Map<EstoqueApi.Models.Product>(productDTO);
            await _writeRepository.Create(product);
            productDTO.Id = product.Id;
        }
        public async Task Updateproduct(ProductDTO productDTO)
        {
            var product = _mapper.Map<EstoqueApi.Models.Product>(productDTO);
            await _writeRepository.Update(product);
        }
        public async Task RemoveProduct(int id)
        {
            var product = await _readRepository.GetById(id);
            await _writeRepository.Delete(product.Id);
        }

        public async Task DecrementStock(int productId, long quantity)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var product = await _readRepository.GetProductForUpdateAsync(productId);

                if (product == null)
                {
                    throw new ProductNotFoundException("Produto não encontrado.");
                }

                if (product.Stock < quantity)
                {
                    throw new InsufficientStockException("Estoque insuficiente.");
                }

                product.Stock -= quantity;

                await _writeRepository.UpdateProductAsync(product);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


    }
}
