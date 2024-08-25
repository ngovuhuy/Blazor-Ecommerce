using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models
{
    public class UpdateProfileResponseDTO
    {
        public bool IsProfileUpdatedSuccessfully { get; set; }
        public List<string> Errors { get; set; }
    }
}
