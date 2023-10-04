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


    public class AdvertiseListModel
    {
        public int advertiseId { get; set; }
        public int productSubCategoryId { get; set; }
        public string advertiseTitle { get; set; }
        public string advertiseDescription { get; set; }
        public decimal advertisePrice { get; set; }
        public int areaId { get; set; }
        public bool advertiseStatus { get; set; }
        public int userId { get; set; }
        public bool advertiseapproved { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }

    }


    // payment models pnkj

    public class Users
    {
        public int userId { get; set; }
    }
    public class MyAdvertise
    {
        public int advertiseId { get; set; }
        public int advertiseapproved { get; set; }

        public int userId { get; set; }
    }

    public class PaymentdetailsSeller
    {
        public int PaymentIds { get; set; }

        public int ReceivedAmount { get; set; }

        public int SellerWallet { get; set; }
        public int TransactionIds { get; set; }

        public int advertiseId { get; set; }
        public DateTime TransactionTimeS { get; set; }


    }

    public class PaymentdetailsBuyer
    {
        public int paymentIdB { get; set; }
        public decimal TotalamountPaid { get; set; }
        public int transactionId { get; set; }
        public decimal Buyerwallet { get; set; }
        public int advertiseId { get; set; }
        public int userId { get; set; }
        public DateTime TransactionTimeP { get; set; }
    }

}