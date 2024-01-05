using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models
{
    public class OrderHeaderDTO
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Order Total")]
        public double OderTotal { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name ="Shipping Date")]
        public DateTime ShippingDate { get; set; }

        [Required]
        public string Status { get; set; }  
        // stripe payment
        public string? SessionId { get; set; }

        public string? PaymentIntentId { get; set; }

        [Display(Name= "Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }


        [Display(Name = "Street Address")]
        [Required(ErrorMessage = "Vui Lòng nhập địa chỉ")]
        public string StreetAddress { get; set; }

        [Required]
        public string State { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Vui Lòng nhập Thành phố")]
        public string City { get; set; }



        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        public string? Tracking { get; set; }

        public string? Carrier { get; set; }
    }
}
