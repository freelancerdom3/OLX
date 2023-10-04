using Olx_New_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Olx_New_Project.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult CheckAvailability(int userId, int advertiseId)
        {
            // Call the IsProductAvailable method from Repositorypayment to check product availability
            DataAccess repository = new DataAccess();
            bool isAvailable = repository.IsProductAvailable(advertiseId);

            // Return a JSON response indicating whether the product is available
            return Json(new { IsAvailable = isAvailable }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PurchaseProduct(int userId, int advertiseId)
        {
            DataAccess repository = new DataAccess();

            bool purchaseResult = repository.PurchaseProduct(userId, advertiseId);

            if (purchaseResult)
            {
                // Purchase was successful
                return Json(new { Success = true, Message = "Purchase was successful!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Purchase failed due to insufficient balance or other reasons
                return Json(new { Success = false, Message = "Purchase failed. Insufficient balance or product unavailable." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddMoney(int userId, int amountToAdd)
        {
            // You can replace "Repositorypayment" with your actual repository class
            DataAccess repository = new DataAccess();

            // Assuming AddMoneyToBuyerWallet returns a boolean indicating success or failure
            bool addMoneyResult = repository.AddingMoneyToBuyerWallet(userId, amountToAdd);

            if (addMoneyResult)
            {
                return Json(new { Success = true, Message = "Money added successfully!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false, Message = "Failed to add money. Please try again." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}