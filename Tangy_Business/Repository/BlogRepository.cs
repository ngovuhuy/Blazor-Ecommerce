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
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _db;

        public BlogRepository(ApplicationDbContext db)
        {
            _db = db;

        }
        public void Create(Blog objDTO)
        {
            _db.Blogs.Add(objDTO);
            _db.SaveChangesAsync();
       
        }

        public async Task<bool> Delete(int id)
        {
            var importToDelete = await _db.Blogs.FindAsync(id);

            if (importToDelete == null)
                return false; // Không tìm thấy đối tượng để xóa

            _db.Blogs.Remove(importToDelete);
            await _db.SaveChangesAsync();
            return true; // Xóa thành công
        }

        public Task<Blog> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Blog>> GetAll()
        {
            return await _db.Blogs.ToListAsync();
        }

        public async Task<Blog> Update(Blog objDTO)
        {
            var existingBlog = await _db.Blogs.FindAsync(objDTO.Id);

            if (existingBlog == null)
            {
                // Blog không tồn tại
                return null;
            }

            // Cập nhật thông tin của Blog từ objDTO
            existingBlog.Name = objDTO.Name;
            existingBlog.Image = objDTO.Image;
            existingBlog.title = objDTO.title;
            existingBlog.Description = objDTO.Description;
            // Lưu các thay đổi vào cơ sở dữ liệu
            await _db.SaveChangesAsync();
            return existingBlog;
        }
    }
}
