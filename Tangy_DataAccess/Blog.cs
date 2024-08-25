using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_DataAccess
{
    public class Blog
    {
        [Key]
        public int Id { get; set; } 

        public string Name { get; set; }

        public string Image { get; set; }
        public string title { get; set; }
        public string Description { get; set; }
    }
}
