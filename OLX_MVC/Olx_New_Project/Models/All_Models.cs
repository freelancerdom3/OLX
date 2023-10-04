using System;
using System.Collections.Generic;
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

}