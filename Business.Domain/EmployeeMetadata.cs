using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

//using System.Web.Mvc;
//using System.Web.Mvc.Html;
//using System.Web.UI.WebControls;

namespace Business.Domain
{
    public class EmployeeMetadata
    {
        [Required(ErrorMessage = "Please Enter Employment ID")]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        [StringLength(50, ErrorMessage = "Last Name should be less than equal to 50 characters long.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [StringLength(150, ErrorMessage = "Address should be less than equal to 150 characters long.")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter Age")]
        public byte Age { get; set; }

        [Required(ErrorMessage = "Please Enter Employment")]
        public byte Employment { get; set; }

        [Required(ErrorMessage = "Please Enter Active")]
        public byte Active { get; set; }


      
    }
}