using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRN221_Project.ModelViews
{
    public class MuaHangVM
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please enter Full Name")]
        public string FullName { get; set; }
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please enter the phone number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Delivery address")]
        public string Address { get; set; }

    }
}
