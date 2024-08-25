using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_DataAccess;

namespace Tangy_Business.Repository.IRepository
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetUsersAsync();
    }
}
