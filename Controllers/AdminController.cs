using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EddsWaffle.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Data;
using System.Data.Entity;
using EddsWaffle.Util;
using System.Globalization;

namespace EddsWaffle.Controllers
{
    public class AdminController : Controller
    {
        private DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();
        // GET: Admin
        public ActionResult Accounts()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAccount(User account)
        {
            db.User.Add(account);
            db.SaveChanges();
            return RedirectToAction("Accounts", "Admin");
        }

        public ActionResult User_Data([DataSourceRequest] DataSourceRequest product)
        {
            List<User> show = new List<User>();
            var showuser = db.User.ToArray();
            for (int i = 0; i < db.User.Count(); i++)
            {
                show.Add(showuser.ElementAt(i));
            }
            return Json(show.ToDataSourceResult(product));
        }

        public ActionResult DeleteUser([DataSourceRequest] DataSourceRequest request, User user)
        {
            User deleteuser = db.User.Find(user.username);
            if (deleteuser == null)
            {
                return Json("Product Not Found");
            }
            db.User.Remove(deleteuser);
            db.SaveChanges();
            return Json(db.User.ToList());
        }
        public ActionResult MiningPrediction()
        {
            var getupdate = db.PredictParameter.ToList();
            var setalpha = getupdate.Select(a => a.alpha_value).Take(1);
            var setbeta = getupdate.Select(a => a.beta_value).Take(1);

            double alphafix = 0;
            double betafix = 0;

            foreach (var item in setalpha)
            {
                alphafix = item;
            }
            foreach (var item2 in setbeta)
            {
                betafix = item2;
            }
            ViewData["alphaval"] = alphafix;
            ViewData["betaval"] = betafix;
            
            return View();
        }
        [HttpPost]
        public ActionResult AddPredictParameter(PredictParameter predict)
        {
            var all = from c in db.PredictParameter select c;
            db.PredictParameter.RemoveRange(all);
            db.SaveChanges();

            PredictParameter predictparameter = new PredictParameter();
            var alpha = Convert.ToDouble(predict.alpha_value);
            var beta = Convert.ToDouble(predict.beta_value);

            predictparameter.alpha_value = alpha;
            predictparameter.beta_value = beta;
            
            db.PredictParameter.Add(predictparameter);
            db.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('New Parameter Value Submitted !'); window.location.href = '/Admin/MiningPrediction';</script>");
        }
        public ActionResult MiningTrends()
        {
            DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();

            var getnewest = db.TrendsParameter.ToList();
            var setminsupp = getnewest.Select(a => a.minsupport_value).Take(1);
            int minsupp = 0;
            
            foreach (int item2 in setminsupp)
            {
                minsupp = item2;
            }

            ViewData["MinimumSupport"] = minsupp;

            ArrayList collectionitem = new ArrayList();
            ArrayList collectionquantity = new ArrayList();
            ArrayList setitem = new ArrayList();


            var itemset = db.OrderDetails.ToList();
            var listofitem = itemset.GroupBy(x => x.code_transaction)
                        .ToArray();
            double totaltransaction = itemset.GroupBy(x => x.code_transaction).Count();

            double minsupport = minsupp * 0.01;

            double selection = minsupport * totaltransaction;


            var apriori1 = itemset
                           .GroupBy(a => a.name_product)
                           .Select(a1 => new OrderDetails
                           {
                               name_product = a1.First().name_product,
                               quantity_product = a1.Sum(b => b.quantity_product)
                           }).ToArray();

            foreach (var item in apriori1)
            {
                collectionitem.Add(item.name_product);
                collectionquantity.Add(item.quantity_product);
            }

            for (int i = 0; i < collectionquantity.Count; i++)
            {
                double total = Convert.ToDouble(collectionquantity[i]);
                var cart = collectionitem[i];
                if (total > selection)
                {
                    setitem.Add(cart);
                }
            }

            int itempassed = setitem.Count;
            ViewData["itempass"] = itempassed;
            ViewData["selection"] = selection;


            return View();

        }

        [HttpPost]
        public ActionResult AddTrendsParameter(TrendsParameter support)
        {
            var all = from c in db.TrendsParameter select c;
            db.TrendsParameter.RemoveRange(all);
            db.SaveChanges();

            TrendsParameter trendsupport = new TrendsParameter();
            var minsupport = Convert.ToInt16(support.minsupport_value);

            trendsupport.minsupport_value = minsupport;

            db.TrendsParameter.Add(trendsupport);
            db.SaveChanges();

            return Content("<script language='javascript' type='text/javascript'>alert('New Parameter Value Submitted !'); window.location.href = '/Admin/MiningTrends';</script>");
        }
        public ActionResult Potential_Data([DataSourceRequest] DataSourceRequest potential)
        {
            DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();

            var getnewest = db.TrendsParameter.ToList();
            var setminsupp = getnewest.Select(a => a.minsupport_value).Take(1);
            int minsupp = 0;

            foreach (int item2 in setminsupp)
            {
                minsupp = item2;
            }

            double minsupport = minsupp * 0.01;
            ArrayList collectionitem = new ArrayList();
            ArrayList collectionquantity = new ArrayList();
            ArrayList setitem = new ArrayList();
            ArrayList setquantity = new ArrayList();
         
            var itemset = db.OrderDetails.ToList();
            var listofitem = itemset.GroupBy(x => x.code_transaction)
                        .ToArray();
            double totaltransaction = itemset.GroupBy(x => x.code_transaction).Count();
            double selection = minsupport * totaltransaction;
         
            var apriori1 = itemset
                           .GroupBy(a => a.name_product)
                           .Select(a1 => new OrderDetails
                           {
                               name_product = a1.First().name_product,
                               quantity_product = a1.Sum(b => b.quantity_product)
                           }).ToArray();

            foreach (var item in apriori1)
            {
                collectionitem.Add(item.name_product);
                collectionquantity.Add(item.quantity_product);
            }

            for (int i = 0; i < collectionquantity.Count; i++)
            {
                double total = Convert.ToDouble(collectionquantity[i]);
                var cart = collectionitem[i];
                var numbers = collectionquantity[i];
                if (total > selection)
                {
                    setitem.Add(cart);
                    setquantity.Add(numbers);
                }
            }


            var all = from c in db.PotentialProduct select c;
            db.PotentialProduct.RemoveRange(all);
            db.SaveChanges();

            PotentialProduct potent = new PotentialProduct();
            potent = new PotentialProduct();

            for (int x = 0; x < setitem.Count; x++)
            {
                potent.product_potential = (string)setitem[x];
                potent.product_number = Convert.ToInt16(setquantity[x]);
                db.PotentialProduct.Add(potent);
                db.SaveChanges();
                potent = new PotentialProduct();
            }

            List<PotentialProduct> show = new List<PotentialProduct>();
            var showpotential = db.PotentialProduct.ToArray();
            for (int i = 0; i < db.PotentialProduct.Count(); i++)
            {
                show.Add(showpotential.ElementAt(i));
            }
            return Json(show.ToDataSourceResult(potential));
        }
    }
    }
