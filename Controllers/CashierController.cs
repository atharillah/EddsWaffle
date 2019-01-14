using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EddsWaffle.Models;
using Kendo.Mvc.UI;
using System.Data.Entity;
using Kendo.Mvc.Extensions;


namespace EddsWaffle.Controllers
{
    public class CashierController : Controller
    {
        private DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();
        // GET: Cashier
        public ActionResult Transaction()
        {
            var getlatest = db.TransactionHeader.ToList();
            var ordering = getlatest.OrderByDescending(a => a.code_transaction).Select(a=>a.code_transaction).Take(1);
            Int64 codetransaction = 0;
            foreach (var item in ordering)
            {
                codetransaction = (Int64) item;
            }
            Int64 modifycode = codetransaction + 1;
            string modify = modifycode.ToString();

            DateTime today = DateTime.Today;
            var current = today.ToString("yyyyMMdd");

            var modified = new StringBuilder(modify);
            modified.Remove(1, 8);
            modified.Insert(1, current);

            modify = modified.ToString();

            Int64 transactioncode = Convert.ToInt64(modify);
            ViewData["code"] = transactioncode;

            return View();
        }
        public JsonResult GetProductValue(string search)
        {
            DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();
            List<ProductModel> allsearch = db.Product.Where(x => x.code_product.Contains(search)).Select(x => new ProductModel
            {
                code_product = x.code_product,
                name_product = x.name_product,
                category_product = x.category_product,
                price_product = x.price_product
            }).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult AddTransaction(TransactionHeader transaction)
        {
            try
            {
                
                List<TransactionHeader> list = db.TransactionHeader.ToList();
               
                TransactionHeader header = new TransactionHeader();
                header.code_transaction = transaction.code_transaction;
                header.date_time = DateTime.Today;
                header.discount_transaction = transaction.discount_transaction;
                header.payment_method = transaction.payment_method;
                header.payment_total = transaction.payment_total;
                db.TransactionHeader.Add(header);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Transaction", "Cashier");
        }
        [HttpPost]
        public JsonResult InsertTableCart(TableCartModel[] itemlist)
        {

            DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();
            OrderDetails table1 = new OrderDetails();

            var getlatest = db.TransactionHeader.ToList();
            var ordering = getlatest.OrderByDescending(a => a.code_transaction).Select(a => a.code_transaction).Take(1);
            Int64 codetransaction = 0;

            foreach (var item in ordering)
            {
                codetransaction = (Int64)item;
            }

            Int64 modifycode = codetransaction + 1;
            string modify = modifycode.ToString();

            DateTime today = DateTime.Today;
            var current = today.ToString("yyyyMMdd");

            var modified = new StringBuilder(modify);
            modified.Remove(1, 8);
            modified.Insert(1, current);

            modify = modified.ToString();

            Int64 transactioncode = Convert.ToInt64(modify);

            foreach (TableCartModel i in itemlist)
            {
                table1.code_transaction = transactioncode;
                table1.code_product = i.code_product;
                table1.name_product = i.name_product;
                table1.quantity_product = i.quantity_product;
                table1.price_product = i.totalprice_product;
                db.OrderDetails.Add(table1);
                db.SaveChanges();
            }
            return Json("Transaction Successfully Added!");
            
        }

        public ActionResult Membership()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddMember(Membership processmember)
        {
            using (DB_EddsWaffleEntities db = new DB_EddsWaffleEntities())
            {
                db.Membership.Add(processmember);
                db.SaveChanges();
            }
            return RedirectToAction("Membership", "Cashier");

        }
        public ActionResult Membership_Data([DataSourceRequest] DataSourceRequest member)
        {
            List<Membership> show = new List<Membership>();
            DB_EddsWaffleEntities dbmembership = new DB_EddsWaffleEntities();
            var showmembership = dbmembership.Membership.ToArray();
            for (int i = 0; i < dbmembership.Membership.Count(); i++)
            {
                show.Add(showmembership.ElementAt(i));
            }
            return Json(show.ToDataSourceResult(member));
        }

        public ActionResult UpdateMembership([DataSourceRequest] DataSourceRequest request, Membership member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new[] { member }.ToDataSourceResult(request, ModelState));
                }
                else
                {
                    return Json(db.Membership.ToList());
                }

            }
            catch (Exception ex){
                return Json(ex.Message);
            }


        }

        
    }
}