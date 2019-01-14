using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EddsWaffle.Models;
using System.Data;
namespace EddsWaffle.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult LoginAuth()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(EddsWaffle.Models.User UserModel)
        {
            using (DB_EddsWaffleEntities auth = new DB_EddsWaffleEntities())
            {
                var CashierSegment = auth.User.Where(x => x.username == UserModel.username && x.role == "Cashier").FirstOrDefault();
                var SupervisorSegment = auth.User.Where(y => y.username == UserModel.username && y.role == "Supervisor").FirstOrDefault();
                var AdminSegment = auth.User.Where(z => z.username == UserModel.username && z.role == "Admin").FirstOrDefault();

                if (CashierSegment!=null)
                {
                    return RedirectToAction("Transaction", "Cashier");
                }
                else if(SupervisorSegment!=null)
                {
                    return RedirectToAction("SalesMonitoring", "Supervisor");
                }
                else if (AdminSegment!=null)
                {
                    return RedirectToAction("Accounts", "Admin");
                }
                
                 else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Incorrect Username / Password - Please Try Again'); window.location.href = '/Login/LoginAuth';</script>");
                }
            }
                
        }
    }
}