using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.API.Entities
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; } = string.Empty;
        public string ProductDiscription { get; set; } = string.Empty;
        public int Amount { get; set; }

       
        
    }
}
