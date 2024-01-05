using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ProductDTO> Create(ProductDTO objDTO)
        {
            var obj = _mapper.Map<ProductDTO, Product>(objDTO);
            var addedObj = _db.Products.Add(obj);
            await _db.SaveChangesAsync();
            return _mapper.Map<Product, ProductDTO>(addedObj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                _db.Products.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductDTO> Get(int id)
        {
            var obj = await _db.Products.Include(u =>u.Category).Include(u=>u.ProductPrices).FirstOrDefaultAsync(u => u.Id == id);
            if (obj != null)
            {
                return _mapper.Map<Product, ProductDTO>(obj);
            }
            return new ProductDTO();
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(_db.Products.Include(u => u.Category).Include(u => u.ProductPrices));
        }
        public async Task<IEnumerable<ProductDTO>> SearchProducts(string searchQuery)
        {
            var query = _db.Products
                .Include(u => u.Category)
                .Include(u => u.ProductPrices)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p =>
                    EF.Functions.Like(p.Name, $"%{searchQuery}%") ||
                    EF.Functions.Like(p.Description, $"%{searchQuery}%")
                );
            }

            var products = await query.ToListAsync();

            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
        }
        public async Task<ProductDTO> Update(ProductDTO objDTO)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(u => u.Id == objDTO.Id);
            if (obj != null)
            {
                obj.Name = objDTO.Name;
                obj.Description = objDTO.Description;
                obj.ImageUrl = objDTO.ImageUrl;
                obj.CategoryId = objDTO.CategoryId;
                obj.Color = objDTO.Color;
                obj.ShopFavorites = objDTO.ShopFavorites;
                obj.CustomerFavorites = objDTO.CustomerFavorites;
                _db.Products.Update(obj);
                await _db.SaveChangesAsync();
                return _mapper.Map<Product, ProductDTO>(obj);

            }
            return objDTO;
        }
    }
}
