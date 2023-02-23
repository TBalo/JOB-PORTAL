using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Models
{
    public class ForgotPasswordMV
    {
        [Required(ErrorMessage = "Required*")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required*")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required*")]
        public string ConfirmPassword { get; set; }
    }
}