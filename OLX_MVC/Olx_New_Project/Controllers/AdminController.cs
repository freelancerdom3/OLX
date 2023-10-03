using Olx_New_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Olx_New_Project.Controllers
{
    public class AdminController : Controller
    {

        DataAccess dataAccess = null;
        public AdminController()
        {
            dataAccess = new DataAccess();
        }


        public ActionResult Dashboard()
        {
            return View("Dashboard", "Admin_Layout");
        }

        public ActionResult SubCategoryList()
        {
            IEnumerable<ProductSubCategoryModel> productDetails = dataAccess.GetProductDetailsLists();
            
            return View("SubCategoryList", "Admin_Layout", productDetails);
        }

        public ActionResult SubCategoryListDetails(int productSubCategoryId)
        {
            ProductSubCategoryModel productDetails = dataAccess.GetProductDetails(productSubCategoryId);
            return View("SubCategoryListDetails", "Admin_Layout", productDetails);
        }

        public ActionResult SubCategoryListCreate()
        {
            return View("SubCategoryListCreate", "Admin_Layout");
        }

        [HttpPost]
        public ActionResult SubCategoryListCreate(ProductSubCategoryModel productDetails)
        {
            try
            {
                dataAccess.AddProductDetails(productDetails);

                return RedirectToAction(nameof(SubCategoryList));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult SubCategoryListEdit(int productSubCategoryId)
        {
            ProductSubCategoryModel productDetails = dataAccess.GetProductDetails(productSubCategoryId);
            return View("SubCategoryListEdit", "Admin_Layout", productDetails);
        }

        [HttpPost]
        public ActionResult SubCategoryListEdit(ProductSubCategoryModel productDetails)
        {
            try
            {
                dataAccess.UpdateProductDetails(productDetails);
                return RedirectToAction(nameof(SubCategoryList));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SubCategoryListDelete(int productSubCategoryId)
        {
            ProductSubCategoryModel productDetails = dataAccess.GetProductDetails(productSubCategoryId);
            return View("SubCategoryListDelete", "Admin_Layout", productDetails);
        }

        [HttpPost]
        public ActionResult SubCategoryListDelete(ProductSubCategoryModel productDetails)
        {
            try
            {
                dataAccess.DeleteProductDetails(productDetails.@productSubCategoryId);
                return RedirectToAction(nameof(SubCategoryList));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult UserIndex()
        {
            IEnumerable<UserList> ul = dataAccess.GetAllUser();
            return View("UserIndex", "admin_layout", ul);
        }

        public ActionResult UserDetails(int? id)
        {
            UserList product = dataAccess.GetUserData(id);
            return View("UserDetails", "admin_layout", product);
        }

        public ActionResult UserEdit(int id)
        {
            UserList user = dataAccess.GetUserData(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult UserEdit(UserList ul)
        {
            try
            {
                dataAccess.Updateuser(ul);

                return RedirectToAction(nameof(UserIndex));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult UserDelete(int? id)
        {
            UserList userList = dataAccess.GetUserData(id);
            return View(userList);
        }

        // POST: olx/Delete/5
        [HttpPost]
        public ActionResult UserDelete(int id)
        {
            try
            {
                dataAccess.DeleteUser(id);
                return RedirectToAction(nameof(UserIndex));
            }
            catch
            {
                return View();
            }
        }

    }
}
