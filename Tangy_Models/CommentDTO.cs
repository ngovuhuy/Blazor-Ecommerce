using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }

        public UserDTO User { get; set; }

        public int ProductId { get; set; }

        public ProductDTO Product { get; set; }
    }
}
