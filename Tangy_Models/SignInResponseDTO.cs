using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models
{
    public class SignInResponseDTO
    {
        public bool IsAuthSuccessful { get; set; }  


        public string ErrorMessage { get; set; }


        public string ToKen { get; set; }

        public UserDTO UserDTO { get; set; }
    }
}
