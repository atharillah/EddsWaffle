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
    public class SupervisorController : Controller
    {
        private DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();
        // GET: Supervisor
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult SetProduct()
        {
            ArrayList categories = new ArrayList();
            var getcategorylist = db.Category.ToList();
            foreach (var item in getcategorylist)
            {
                categories.Add(item.category_name);
            }
            var categorylist = categories.ToArray();
            ViewBag.Categories = categorylist;
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product processmodel)
        {
            var codecheck = db.Product.FirstOrDefault(a => a.code_product == processmodel.code_product);

            if (codecheck != null)
            {

                return Content("<script language='javascript' type='text/javascript'>alert('Adding Product Failed (Product Code Already Exist) - Please Try Again'); window.location.href = '/Supervisor/SetProduct';</script>");
            }
            else if (processmodel.code_product == null| processmodel.name_product == null | processmodel.price_product == 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Error - Please Try Again and Fill The Form Completely'); window.location.href = '/Supervisor/SetProduct';</script>");
            }
           
            else
            {
                using (DB_EddsWaffleEntities db = new DB_EddsWaffleEntities())
                {
                    db.Product.Add(processmodel);
                    db.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>alert('New Product Sucessfully Added !'); window.location.href = '/Supervisor/SetProduct';</script>");
                }
            }
            
         
        }
        
        public ActionResult Product_Data([DataSourceRequest] DataSourceRequest product)
        {
            List<Product> show = new List<Product>();
            DB_EddsWaffleEntities dbproduct = new DB_EddsWaffleEntities();
            var showproduct = dbproduct.Product.ToArray();
            for (int i = 0; i < dbproduct.Product.Count(); i++)
            {
                show.Add(showproduct.ElementAt(i));
            }
            return Json(show.ToDataSourceResult(product));
        }

        public ActionResult UpdateProduct([DataSourceRequest] DataSourceRequest request, Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new[] { product }.ToDataSourceResult(request, ModelState));
            }
            else
            {
                return Json(db.Product.ToList());
            }

        }
        public ActionResult DeleteProduct([DataSourceRequest] DataSourceRequest request, Product product)
        {
            Product deleteproduct = db.Product.Find(product.code_product);
            if (deleteproduct == null)
            {
                return Json("Product Not Found");
            }
            db.Product.Remove(deleteproduct);
            db.SaveChanges();
            return Json(db.Product.ToList());
        }

        public ActionResult Category()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category processcategory)
        {
            var categorycheck = db.Category.FirstOrDefault(a => a.category_name == processcategory.category_name);

            if (categorycheck != null)
            {

                return Content("<script language='javascript' type='text/javascript'>alert('Category Is Already Exist - Please Try Again'); window.location.href = '/Supervisor/Category';</script>");
            }
            else if (processcategory.category_name == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Error - Please Try Again and Fill The Category Name Correctly'); window.location.href = '/Supervisor/Category';</script>");
            }

            else
            {
                using (DB_EddsWaffleEntities db = new DB_EddsWaffleEntities())
                {
                    db.Category.Add(processcategory);
                    db.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>alert('New Category Succesfully Added !'); window.location.href = '/Supervisor/Category';</script>");
                }
            }
        }
        public ActionResult Category_Data([DataSourceRequest] DataSourceRequest category)
        {
            List<Category> show = new List<Category>();
            DB_EddsWaffleEntities dbcategory = new DB_EddsWaffleEntities();
            var showcategory = dbcategory.Category.ToArray();
            for (int i = 0; i < dbcategory.Category.Count(); i++)
            {
                show.Add(showcategory.ElementAt(i));
            }
            return Json(show.ToDataSourceResult(category));
        }

        public ActionResult UpdateCategory([DataSourceRequest] DataSourceRequest request, Category category)
        {
            var categorycheck = db.Category.FirstOrDefault(a => a.category_name == category.category_name);
            if (categorycheck == null)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new[] { category }.ToDataSourceResult(request, ModelState));
            }

            else
            {
                return Json(db.Category.ToList());
            }

        }
        public ActionResult DeleteCategory([DataSourceRequest] DataSourceRequest request, Category category)
        {
            Category deletecategory = db.Category.Find(category.id_category);
            if (deletecategory == null)
            {
                return Json("Category Not Found");
            }
            db.Category.Remove(deletecategory);
            db.SaveChanges();
            return Json(db.Category.ToList());
        }
        
        public ActionResult History()
        {
            return View();
        }
        public ActionResult History_Data([DataSourceRequest] DataSourceRequest history)
        {
            List<TransactionHeader> show = new List<TransactionHeader>();
            DB_EddsWaffleEntities dbhistory = new DB_EddsWaffleEntities();

            var showhistory = dbhistory.TransactionHeader.ToArray();
            for (int i = 0; i < dbhistory.TransactionHeader.Count(); i++)
            {
                show.Add(showhistory.ElementAt(i));
            }
            return Json(show.ToDataSourceResult(history));
        }
        public ActionResult DeleteHistory([DataSourceRequest] DataSourceRequest request, TransactionHeader history)
        {
            TransactionHeader deletehistory = db.TransactionHeader.Find(history.code_transaction);
            db.Configuration.ProxyCreationEnabled = false;
            if (deletehistory == null)
            {
                return Json("Member Not Found");
            }
            db.TransactionHeader.Remove(deletehistory);
            db.SaveChanges();
            return Json(db.TransactionHeader.ToList());
        }

        public ActionResult DetailHistory_Data(Int64 id, [DataSourceRequest] DataSourceRequest details)
        {
            List<DetailTransaction> show = new List<DetailTransaction>();
            DB_EddsWaffleEntities dbdetails = new DB_EddsWaffleEntities();
            db.Configuration.ProxyCreationEnabled = false;
            var showdetails = dbdetails.DetailTransaction.ToArray();
            for (int i = 0; i < dbdetails.DetailTransaction.Count(); i++)
            {
                show.Add(showdetails.ElementAt(i));
            }
            return Json(show.Where(set => set.transaction_code == id).ToDataSourceResult(details));
        }

        public ActionResult SalesMonitoring()
        {

            return View();
        }
        public ActionResult Chart_Data([DataSourceRequest]DataSourceRequest request)
        {
            List<TransactionHeader> result = new List<TransactionHeader>();
            DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();
            var showchart = db.TransactionHeader.ToArray();

            for (int i = 0; i < db.TransactionHeader.Count(); i++)
            {
                result.Add(showchart.ElementAt(i));
            }
            return Json(result);
        }

        public ActionResult Comparison_Data([DataSourceRequest]DataSourceRequest request)
        {

            var revenue = db.TransactionHeader.ToArray();
            var Forecasting = db.Prediction.ToArray();
            List<Prediction> estimate = new List<Prediction>();
            Prediction predict = new Prediction();

            var weekly = db.TransactionHeader.ToList().GroupBy(x => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(x.date_time, CalendarWeekRule.FirstDay, DayOfWeek.Saturday)).ToArray();
            int counter = 0;
            int[] income = new int[weekly.Count()];
            double[,] Length = new double[weekly.Count() + 4, 4];

            var getupdate = db.PredictParameter.ToList();
            var setalpha = getupdate.Select(a => a.alpha_value).Take(1);
            var setbeta = getupdate.Select(a => a.beta_value).Take(1);

            double alphavalue = 0;
            double betavalue = 0;

            foreach (var item in setalpha)
            {
                alphavalue = item;
            }
            foreach (var item2 in setbeta)
            {
                betavalue = item2;
            }
            

            foreach (var item in weekly)
            {
                foreach (var item2 in item)
                {
                    income[counter] = income[counter] + (int)item2.payment_total;
                }
                counter++;
            }
            

            for (int i = 0; i < counter + 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i < weekly.Count())
                    {
                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                var income1 = income[0];
                                Length[i, j] = income1;
                                //System.Diagnostics.Debug.WriteLine(income1);
                            }
                            else if (j == 1)
                            {
                                var actual1 = income[0];
                                Length[i, j] = actual1;
                                // System.Diagnostics.Debug.WriteLine(actual1);
                            }
                            else if (j == 2)
                            {
                                var index4 = income[i + 3];
                                var index3 = income[i + 2];
                                var index2 = income[i + 1];
                                var index1 = income[i + 0];
                                var Trend = ((index2 - index1) + (index4 - index3)) / 2;
                                Length[i, j] = Trend;
                                //System.Diagnostics.Debug.WriteLine(Trend);
                            }
                            else if (j == 3)
                            {
                                var forecast = 0;
                                Length[i, j] = forecast;
                                //System.Diagnostics.Debug.WriteLine(forecast);
                            }
                        }
                        // Holt Calculation
                        else
                        {
                            if (j == 0)
                            {
                                var dataactual = income[i];
                                Length[i, j] = dataactual;
                            }
                            else if (j == 1)
                            {
                                double Stbefore = Length[i - 1, j];
                                //System.Diagnostics.Debug.WriteLine(Stbefore);
                                double Ttbefore = Length[i - 1, j + 1];
                                //System.Diagnostics.Debug.WriteLine(Ttbefore);

                                double At = alphavalue * income[i] + (1 - alphavalue) * (Stbefore + Ttbefore);
                                Length[i, j] = At;
                                // System.Diagnostics.Debug.WriteLine(At);
                            }
                            else if (j == 2)
                            {
                                double Stnow = Length[i, j - 1];
                                double Stbefore = Length[i - 1, j - 1];
                                double Ttbefore = Length[i - 1, j];
                                double Tt = betavalue * (Stnow - Stbefore) + (1 - betavalue) * Ttbefore;
                                Length[i, j] = Tt;

                                // double Stbatas = Length[j - 2];
                                //  System.Diagnostics.Debug.WriteLine(Tt);
                            }   
                            else if (j == 3)
                            {
                                double Stbefore = Length[i - 1, j - 2];
                                double Ttbefore = Length[i - 1, j - 1];
                                //System.Diagnostics.Debug.WriteLine(Stbefore);
                                //System.Diagnostics.Debug.WriteLine(Ttbefore);
                                double Forecast = Stbefore + Ttbefore;
                                Length[i, j] = Forecast;
                            }
                        }
                    }
                    // Holt Forecasting
                    else
                    {
                        if (j == 3)
                        {
                            var Stlimit = Length[weekly.Count() - 1, j - 2];
                            //System.Diagnostics.Debug.WriteLine(Stlimit);
                            //System.Diagnostics.Debug.WriteLine("stlimit");
                            var Ttlimit = Length[weekly.Count() - 1, j - 1];
                            //System.Diagnostics.Debug.WriteLine(Ttlimit);
                            //System.Diagnostics.Debug.WriteLine("ttlimit");
                            var Prediction1 = Stlimit + Ttlimit * (i + 1 - weekly.Count());
                            var Prediction2 = Stlimit + Ttlimit * (i + 2 - weekly.Count());
                            var Prediction3 = Stlimit + Ttlimit * (i + 3 - weekly.Count());
                            var Prediction4 = Stlimit + Ttlimit * (i + 4 - weekly.Count());
                            var getdate = revenue.OrderByDescending(x => x.date_time).Select(x => x.date_time).FirstOrDefault();
                            var date1 = getdate.AddDays(7);
                            var date2 = getdate.AddDays(14);
                            var date3 = getdate.AddDays(21);
                            var date4 = getdate.AddDays(28);

                            double[] CollectionPrediction = { Prediction1, Prediction2, Prediction3, Prediction4 };
                            DateTime[] CollectionDate = { date1, date2, date3, date4 };

                            var all = from c in db.Prediction select c;
                            db.Prediction.RemoveRange(all);
                            db.SaveChanges();


                            for (int x = 0; x < 4; x++)
                            {
                                predict.id_prediction = x + 1;
                                predict.revenue_prediction = CollectionPrediction[x];
                                predict.date = CollectionDate[x];
                                db.Prediction.Add(predict);
                                db.SaveChanges();
                                predict = new Prediction();
                            }

                        }

                    }


                }

            }

            for (int i = 0; i < db.Prediction.Count(); i++)
            {
                estimate.Add(Forecasting.ElementAt(i));
            }
            return Json(estimate);
        }

        public ActionResult TopIncome_Data([DataSourceRequest]DataSourceRequest income)
        {
            DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();
            List<TopIncome> earn = new List<TopIncome>();
            var showincome = db.TopIncome.ToArray();

            var add = 0;
            var counter = 0;
            var grouping = db.TransactionHeader.ToArray();
            decimal[] collectionincome = new decimal[10];
            string[] collectiondate = new string[10];
            var topincome = (from h in grouping
                             group h by new { h.date_time } into hh
                             select new
                             {
                                 Top = hh.Sum(s => s.payment_total),
                                 Date = hh.GroupBy(s => s.date_time),
                             }).OrderByDescending(a => a.Top).Take(10).ToArray();


            foreach (var item in topincome)
            {
                collectionincome[add] = item.Top;
                foreach (var item2 in item.Date)
                {
                    collectiondate[counter] = item2.Key.Date.ToString(("MM:dd:yyyy"));
                }
                counter++;
                add++;
            }

            var all = from c in db.TopIncome select c;
            db.TopIncome.RemoveRange(all);
            db.SaveChanges();

            TopIncome Top = new TopIncome();
            Top = new TopIncome();

            for (int x = 0; x < 10; x++)
            {
                Top.ranking_income = x + 1;
                Top.total_income = collectionincome[x];
                Top.date_income = collectiondate[x];
                db.TopIncome.Add(Top);
                db.SaveChanges();
                Top = new TopIncome();
            }

            for (int i = 0; i < db.TopIncome.Count(); i++)
            {
                earn.Add(showincome.ElementAt(i));
            }
            return Json(earn);
        }

        public ActionResult LowIncome_Data([DataSourceRequest]DataSourceRequest income)
        {
            DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();
            List<LowIncome> lowest = new List<LowIncome>();
            var showincome = db.LowIncome.ToArray();

            var add = 0;
            var counter = 0;
            var grouping = db.TransactionHeader.ToArray();
            decimal[] collectionincome = new decimal[10];
            String[] collectiondate = new String[10];
            var lowincome = (from h in grouping
                             group h by new { h.date_time } into hh
                             select new
                             {
                                 Top = hh.Sum(s => s.payment_total),
                                 Date = hh.GroupBy(s => s.date_time),
                             }).OrderBy(a => a.Top).Take(10).ToArray();

            foreach (var item in lowincome)
            {
                collectionincome[add] = item.Top;
                foreach (var item2 in item.Date)
                {
                    collectiondate[counter] = item2.Key.Date.ToString("MM:dd:yyyy");
                }
                counter++;
                add++;
            }

            var all = from c in db.LowIncome select c;
            db.LowIncome.RemoveRange(all);
            db.SaveChanges();

            LowIncome Low = new LowIncome();
            Low = new LowIncome();

            for (int x = 0; x < 10; x++)
            {
                Low.ranking_lowincome = x + 1;
                Low.total_lowincome = collectionincome[x];
                Low.date_lowincome = collectiondate[x];
                db.LowIncome.Add(Low);
                db.SaveChanges();
                Low = new LowIncome();
            }

            for (int i = 0; i < db.LowIncome.Count(); i++)
            {
                lowest.Add(showincome.ElementAt(i));
            }
            return Json(lowest);
        }

        public ActionResult Trends()
        {
            return View();
        }

        public ActionResult Recommendation_Data([DataSourceRequest] DataSourceRequest recommendation)
        {
            DB_EddsWaffleEntities db = new DB_EddsWaffleEntities();
            var getnewest = db.TrendsParameter.ToList();
            var setminsupp = getnewest.Select(a => a.minsupport_value).Take(1);
            int minsupp = 0;

            foreach (int item2 in setminsupp)
            {
                minsupp = item2;
            }

            ArrayList setitem = new ArrayList();
            ArrayList setquantity = new ArrayList();
            ArrayList collectionitem = new ArrayList();
            ArrayList collectionquantity = new ArrayList();
            ArrayList collectioncombination = new ArrayList();
            ArrayList collectiontotals = new ArrayList();

            

            var itemset = db.OrderDetails.ToList();
            var listofitem = itemset.GroupBy(x => x.code_transaction)
                        .ToArray();
            double totaltransaction = itemset.GroupBy(x => x.code_transaction).Count();

            double minsupport = minsupp * 0.01;

            double selection = minsupport * totaltransaction;
            var counter = 0;
            string matching = "";
            var totals = 0;

            

            var apriori1 = itemset
                           .GroupBy(a => a.name_product)
                           .Select(a1 => new OrderDetails
                           {
                               name_product = a1.First().name_product,
                               quantity_product = a1.Sum(b=>b.quantity_product)
                           }).ToArray();

            var apriori2 = (from h in itemset
                            group h by new { h.code_transaction } into hh
                            select new
                            {
                                Product = hh.Select(s => s.name_product)
                            }).ToArray();

            // Step 1

            foreach (var item in apriori1)
            {
                collectionitem.Add(item.name_product);
                collectionquantity.Add(item.quantity_product);
            }

            // Step 2
            // Passing Selection
            for (int i = 0; i < collectionquantity.Count; i++)
            {
                double total = Convert.ToDouble(collectionquantity[i]);
                var cart = collectionitem[i];
                if (total > selection)
                {
                    setitem.Add(cart);
                }
            }

            for (int x = 0; x < setitem.Count; x++)
            {
                for (int y = x + 1; y < setitem.Count-1; y++)
                {
                    var itempair = setitem[x];
                    var pairitem = setitem[y];

                    //System.Diagnostics.Debug.WriteLine(itempair + "->" + pairitem);
                    var counting = apriori2.Count();


                    for (int z = 0; z < counting; z++)
                    {
                        foreach (var item in apriori2[z].Product)
                        {
                            foreach (var items in apriori2[z].Product)
                            {
                                if ((string)itempair == item && (string)pairitem == items)
                                {
                                    matching = itempair + " and " + pairitem;
                                    totals = ++counter;
                                }
                            }
                        }

                    }
                    collectioncombination.Add(matching);
                    collectiontotals.Add(totals);
                    counter = 0;

                }
            }

            // Removing and Adding To Database
            
            var all = from c in db.Recommendation select c;
            db.Recommendation.RemoveRange(all);
            db.SaveChanges();

            Recommendation recom = new Recommendation();
            recom = new Recommendation();

            for (int x = 0; x < collectioncombination.Count; x++)
            {
                recom.item_combination = (string) collectioncombination[x];
                recom.total_value = (int) collectiontotals[x];
                db.Recommendation.Add(recom);
                db.SaveChanges();
                recom = new Recommendation();
            }

            List<Recommendation> show = new List<Recommendation>();
            var showrecommendation = db.Recommendation.ToArray();
            for (int i = 0; i < db.Recommendation.Count(); i++)
            {
                show.Add(showrecommendation.ElementAt(i));
            }
            return Json(show.ToDataSourceResult(recommendation));
        }

        public ActionResult TotalSold_Data([DataSourceRequest] DataSourceRequest totalsold)
        {
            ArrayList collectionproduct = new ArrayList();
            ArrayList collectiontotal = new ArrayList();

            var setitem = db.OrderDetails.ToList();
            var bestselling = setitem
                          .GroupBy(a => a.name_product)
                          .Select(a1 => new OrderDetails
                          {
                              name_product = a1.First().name_product,
                              quantity_product = a1.Sum(b => b.quantity_product)
                          }).ToArray();

            foreach (var item in bestselling)
            {
                collectionproduct.Add(item.name_product);
                collectiontotal.Add(item.quantity_product);
            }

            var all = from c in db.ItemSold select c;
            db.ItemSold.RemoveRange(all);
            db.SaveChanges();

            ItemSold Sold = new ItemSold();
            Sold = new ItemSold();

            for (int x = 0; x < collectionproduct.Count; x++)
            {
                Sold.item_bestselling = (string)collectionproduct[x];
                Sold.value_bestselling = Convert.ToInt16(collectiontotal[x]);
                db.ItemSold.Add(Sold);
                db.SaveChanges();
                Sold = new ItemSold();
            }

            List<ItemSold> sell = new List<ItemSold>();
            var showtotal = db.ItemSold.ToArray();
            for (int i = 0; i < db.ItemSold.Count(); i++)
            {
                sell.Add(showtotal.ElementAt(i));
            }
            return Json(sell.ToDataSourceResult(totalsold));
        }
        public ActionResult Accounts()
        {
            return View();
        }
      
    }
    }   