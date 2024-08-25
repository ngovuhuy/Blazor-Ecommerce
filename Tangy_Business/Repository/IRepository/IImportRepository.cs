using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_DataAccess;

namespace Tangy_Business.Repository.IRepository
{
    public interface IImportRepository
    {
        void AddImport(Import import);
        Import GetImportById(int importId);
        public Task<IEnumerable<Import>> GetAllImports();
        void UpdateProductQuantity(int productId, int importQuantity);
        Task<bool> Delete(int id);
        Task<decimal> CalculateTotalAmountInRange(DateTime startDate, DateTime endDate);
        Task<decimal> CalculateTotalAmount();
    }
}
