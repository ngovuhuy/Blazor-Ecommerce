using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_DataAccess;
using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{
    public interface IBlogRepository
    {
        void Create(Blog objDTO);
  
        public Task<Blog> Update(Blog objDTO);
 
        Task<bool> Delete(int id);
        public Task<Blog> Get(int id);
        public Task<IEnumerable<Blog>> GetAll();
    }
}
