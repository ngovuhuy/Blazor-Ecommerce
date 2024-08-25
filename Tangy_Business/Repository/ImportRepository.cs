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

namespace Tangy_Business.Repository
{
    public class ImportRepository : IImportRepository
    {
        private readonly ApplicationDbContext _db;
 
        public ImportRepository(ApplicationDbContext db )
        {
            _db = db;
        
        }
        public void AddImport(Import import)
        {
            _db.Imports.Add(import);
            _db.SaveChanges();
            UpdateProductQuantity(import.ProductId, import.ImportQuantity);
        }

        public async Task<decimal> CalculateTotalAmount()
        {
            var imports = await _db.Imports.ToListAsync();
            decimal totalAmount = (decimal)imports.Sum(import => import.UnitPrice * import.ImportQuantity);
            return (decimal)totalAmount;
        }

        public async Task<decimal> CalculateTotalAmountInRange(DateTime startDate, DateTime endDate)
        {
            var totalAmount = await _db.Imports
                .Where(i => i.DateAdded >= startDate && i.DateAdded <= endDate)
                .SumAsync(i => (decimal)(i.UnitPrice * i.ImportQuantity));

            return totalAmount;
        }

        public async Task<bool> Delete(int id)
        {
            var importToDelete = await _db.Imports.FindAsync(id);

            if (importToDelete == null)
                return false; // Không tìm thấy đối tượng để xóa

            _db.Imports.Remove(importToDelete);
            await _db.SaveChangesAsync();
            return true; // Xóa thành công
        }

        public async Task<IEnumerable<Import>> GetAllImports()
        {
            return await  _db.Imports.ToListAsync();
        }

        public Import GetImportById(int importId)
        {
            return _db.Imports.Find(importId);
        }

        public void UpdateProductQuantity(int productId, int importQuantity)
        {
            var product = _db.Products.Find(productId);

            if (product != null)
            {
                product.Quantity += importQuantity;
                _db.SaveChanges();
            }
        }
    }
}
