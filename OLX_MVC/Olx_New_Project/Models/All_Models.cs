using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Olx_New_Project.Models
{
    public class ProductCategoryModel
    {
        public int productCategoryId { get; set; }
        public string productCategoryName { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
    }

    public class ProductSubCategoryModel 
    {
        public int productSubCategoryId { get; set; }
        public int productCategoryId { get; set; }
        public string productSubCategoryName { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
    }
    public class UserList
    { 
        public int UserId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userEmail { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime createdon { get; set; }
        public DateTime updatedon { get; set; }

    }
    public class RegistrationModel
    {

        public int userId { get; set; }

        [Required(ErrorMessage = "Please Enter FirstName")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Please Enter LastName")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "Email Id is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string userEmail { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} character", MinimumLength = 8)]
        [RegularExpression(@"^([a-zA-Z0-9@*#]{8,15})$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 " +
                             "UpperCase, LowerCase, Number, Special Character")]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "MobileNo")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Entered phone format is not valid.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Please Select the gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "The Address field is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please select a city")]
        public string City { get; set; }


    }


}