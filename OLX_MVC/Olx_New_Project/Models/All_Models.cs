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

}