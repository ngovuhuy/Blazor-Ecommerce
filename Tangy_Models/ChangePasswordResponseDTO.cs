using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models
{
    public class ChangePasswordResponseDTO
    {
        public bool IsPasswordChangedSuccessfully { get; set; }
        public List<string> Errors { get; set; }
    }
}
