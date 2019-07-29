using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DS3_Sprint1.Models;
using System.Net.Mail;
using DS3_Sprint1.ViewModels;

namespace DS3_Sprint1.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult IndexS(bool cChili = false, bool cNuts = false, bool cDairy = false, bool cOlives = false)
        {

            IList<Products> pList = new List<Products>();
            var prod = from p in db.Products.ToList()
                       select p;

            if (cChili)
            {
                prod = prod.Where(s => s.ChiliStat == "Yes");
            }

            if (cDairy)

            {

                prod = prod.Where(s => s.DairyStat == "Yes");

            }

            if (cNuts)
            {

                prod = prod.Where(s => s.NutsStat == "Yes");

            }
            if (cOlives)
            {
                prod = prod.Where(s => s.OliveStat == "Yes");
            }

            var myProdList = prod.ToList();
            foreach (var prodData in myProdList)
            {

                pList.Add(new Products()
                {
                    ChiliStat = prodData.ChiliStat,
                    DairyStat = prodData.DairyStat,
                    Desc = prodData.Desc,
                    NetWeight = prodData.NetWeight,
                    NutsStat = prodData.NutsStat,
                    OliveStat = prodData.OliveStat,
                    Price = prodData.Price,
                    ProductId = prodData.ProductId,
                    ProductName = prodData.ProductName,  
                    //Qty = prodData.Qty,
                    Type = prodData.Type
                });

            }
            return View(prod);
        }

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList().Where(x => x.image != null && x.Ibarcode != null));
        }

        public ActionResult IndexView()
        {
            return View(db.Products.ToList());
        }

        public ActionResult LowStock()
        {
            var List = db.Products.ToList().FindAll(x => x.qtyh <= 10);

            return View(List);
        }

        public ActionResult Menu()
        {
            return View(db.Products.ToList().Where(x => x.image != null && x.Ibarcode != null));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Barcode Scanner Code ... Copy entire method 
        public ActionResult Create([Bind(Include = "ProductId,Ibarcode,ProductName,Type,Price,Desc,NetWeight,ChiliStat,DairyStat,NutsStat,OliveStat,image")] Products products, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Server.MapPath("~/Images/" + ImageName);
                    file.SaveAs(physicalPath);

                    products.image = ImageName;
                }
                var product = db.Products.ToList().Find(x => x.Ibarcode == products.Ibarcode);

                if (product == null)
                {
                    db.Products.Add(products);
                    db.SaveChanges();

                    Session["id"] = products.ProductId.ToString();
                    return RedirectToAction("editList");
                }

                else
                {
                    if (product != null)
                    {
                        ViewBag.Message = "Barcode In Use";
                    }
                }
            }
            return View(products);
        }

        public ActionResult AddDreamProduct()
        {
            var id = (string)Session["id"];
            var dp = Convert.ToInt16(id);
            var name = (string)Session["name"];
            var type = (string)Session["type"];
            var desc = (string)Session["desc"];

            var deliveries = db.delivery.ToList().Find(x => x.OrderId == dp);

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Barcode Scanner Code ... Copy entire method 
        public ActionResult AddDreamProduct([Bind(Include = "ProductId,ProductName,Type,Price,Desc,NetWeight,ChiliStat,DairyStat,NutsStat,OliveStat")] Products products)
        {
            if (ModelState.IsValid)
            {
                var id = (string)Session["id"];
                var dp = Convert.ToInt16(id);
                var name = (string)Session["name"];
                var type = (string)Session["type"];
                var desc = (string)Session["desc"];

                var dreamprod = db.dreamProducts.ToList();
                var ingredients = db.dreamProdIngredients.ToList();

                //string chili = "";
                //string dairy = "";
                //string nuts = "";
                //string olives = "";

                //foreach (var item in dreamprod)
                //{
                //    foreach (var ing in ingredients)
                //    {
                //        if (item.Id == ing.DreamProdId)
                //        {
                //            if (item.Id == dp)
                //            {
                //                if (ing.SupplyName == "Jalapenos Whole" || ing.SupplyName == "Chilies")
                //                {
                //                    products.ChiliStat = "Yes";
                //                    chili = "Yes";
                //                }
                //                else if (ing.SupplyName != "Jalapenos Whole" && ing.SupplyName != "Chilies")
                //                {
                //                    products.ChiliStat = "No";
                //                    chili = "No";
                //                }
                //                if (ing.SupplyName == "Danish Feta Cheese" || ing.SupplyName == "Cream Cheese" || ing.SupplyName == "Parmesan Cheese")
                //                {
                //                    products.DairyStat = "Yes";
                //                    dairy = "Yes";
                //                }
                //                else if (ing.SupplyName != "Danish Feta Cheese" && ing.SupplyName != "Cream Cheese" && ing.SupplyName != "Parmesan Cheese")
                //                {
                //                    products.DairyStat = "No";
                //                    dairy = "No";
                //                }
                //                if (ing.SupplyName == "Cashew Nuts")
                //                {
                //                    products.NutsStat = "Yes";
                //                    nuts = "Yes";
                //                }
                //                else if (ing.SupplyName != "Cashew Nuts")
                //                {
                //                    products.NutsStat = "No";
                //                    nuts = "No";
                //                }
                //                if (ing.SupplyName == "Green olives" || ing.SupplyName == "Black Olives" || ing.SupplyName == "Kalamata Olives")
                //                {
                //                    products.OliveStat = "Yes";
                //                    olives = "Yes";
                //                }
                //                else if (ing.SupplyName != "Green olives" && ing.SupplyName != "Black Olives" && ing.SupplyName != "Kalamata Olives")
                //                {
                //                    products.OliveStat = "No";
                //                    olives = "No";
                //                }
                //            }
                //        }
                //    }
                //}

                //Session["chili"] = chili;
                //Session["dairy"] = dairy;
                //Session["nuts"] = nuts;
                //Session["olives"] = olives;

                var dreampr = db.dreamProducts.ToList().Find(x => x.Id == dp);

                var find = db.dreamProducts.ToList().Find(x => x.Id == dp);
                string user = find.UserName;
                var nm = db.Users.ToList().Find(x => x.UserName == user);

                try
                {
                    var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
                    var recieveremail = new MailAddress(user, "Reciever");
                    var password = "hzea bfpq aihm vyiz";

                    int date = DateTime.Now.Year;
                    string thisyear = Convert.ToString(date);

                    var sub = "Decision for Dream Product Idea";
                    var body = "Dear " + nm.fullname + " , Your Dream Product idea (" + find.ProdName + ") has been accepted and it will soon begin production at our shop. Thank you for your great idea!";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };

                    using (var msg = new MailMessage(senderEmail, recieveremail)
                    {
                        Subject = sub,
                        Body = body
                    }
                    )
                    {
                        smtp.Send(msg);
                    }


                }
                catch (Exception)
                {

                }

                products.ProductName = name;
                products.Type = type;
                products.Desc = desc;

                db.Products.Add(products);

                var list = new List<Ingredients>();
                foreach (var dreamProd in dreamprod)
                {
                    foreach (var ing in ingredients)
                    {
                        if (dreamProd.Id == dp)
                        {
                            if (dreamProd.Id == ing.DreamProdId)
                            {
                                list.Add(new Ingredients
                                {
                                    ProductId = products.ProductId,
                                    Ingredient = ing.SupplyId,
                                    SupplyName = ing.SupplyName
                                });
                                db.ingredients.AddRange(list);
                            }
                        }
                    }
                }
                if (dreampr != null)
                {
                    db.dreamProducts.Remove(dreampr);
                }
                db.SaveChanges();
                    return RedirectToAction("IndexView");
                }
                 return View(products);
        }

        public ActionResult editList(/*int? id*/)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            var pid = (string)Session["id"];
            var id = Convert.ToInt16(pid);

            var ProdIngredients = new ProductIngredientsViewModel
            {
                products = db.Products.Include(i => i.supplies).First(i => i.ProductId == id),
            };

            if (ProdIngredients.products == null)
                return HttpNotFound();

            var list = db.supplies.ToList();
            ProdIngredients.IngredientsList = list.Select(o => new SelectListItem
            {
                Text = o.SupplyName,
                Value = o.SupplyId.ToString()
            });

            return View(ProdIngredients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//[Bind(Include = "Title,Id,EmployerID,SelectedJobTags")]
        public ActionResult editList(ProductIngredientsViewModel ingredients)
        {

            if (ingredients == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);



            if (ModelState.IsValid)
            {
                var ToUpdate = db.Products
                    .Include(i => i.supplies).First(i => i.ProductId == ingredients.products.ProductId);

                if (TryUpdateModel(ToUpdate, "Products", new string[] { "ProductId", "ProductName" }))
                {
                    var newSupplies = db.supplies.Where(
                        m => ingredients.supplies.Contains(m.SupplyId)).ToList();
                    var updatedSupplies = new HashSet<int>(ingredients.supplies);
                    foreach (Supplies supplies in db.supplies)
                    {
                        if (!updatedSupplies.Contains(supplies.SupplyId))
                        {
                            ToUpdate.supplies.Remove(supplies);
                        }
                        else
                        {
                            ToUpdate.supplies.Add((supplies));
                        }
                    }

                    ApplicationDbContext cont = new ApplicationDbContext();
                    var prodIngredients = new List<Ingredients>();
                    foreach (var item in newSupplies)
                    {
                        prodIngredients.Add(new Ingredients
                        {
                            ProductId = ToUpdate.ProductId,
                            Ingredient = item.SupplyId,
                            SupplyName = item.SupplyName
                        });
                        db.ingredients.AddRange(prodIngredients);
                    }

                    db.Entry(ToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("IndexView");
            }
            return View(ingredients);
        }

        public ActionResult Ingredients(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var list = db.ingredients.ToList().Where(x => x.ProductId == id);

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        public ActionResult AddIngredient(string sup)
        {
            var supply = new List<string>();
            var Query2 = from q in db.supplies orderby q.SupplyId select q.SupplyName;
            supply.AddRange(Query2);
            ViewBag.sup = new SelectList(supply);

            var pid = (string)Session["ID"];
            var id = Convert.ToInt16(pid);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddIngredient([Bind(Include = "Id,ProductId,Ingredient,SupplyName")] Ingredients ingredients, string sup)
        {
            if (ModelState.IsValid)
            {
                var supply = new List<string>();
                var Query2 = from q in db.supplies orderby q.SupplyId select q.SupplyName;
                supply.AddRange(Query2);
                ViewBag.sup = new SelectList(supply);

                var pid = (string)Session["ID"];
                var id = Convert.ToInt16(pid);

                Products products = db.Products.Find(id);
                var s = db.supplies.ToList().Find(x => x.SupplyName == sup);

                ingredients.ProductId = products.ProductId;
                ingredients.SupplyName = sup;
                ingredients.Ingredient = s.SupplyId;

                db.ingredients.Add(ingredients);
                db.SaveChanges();
                return RedirectToAction("Ingredients", new { id = ingredients.ProductId });
            }

            return View(ingredients);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Ibarcode,ProductName,Type,Price,Desc,NetWeight,qtyh,ChiliStat,DairyStat,NutsStat,OliveStat")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexView");
            }
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("IndexView");
        }

        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ingredients ingredients = db.ingredients.Find(id);
            if (ingredients == null)
            {
                return HttpNotFound();
            }
            return View(ingredients);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(int? id)
        {
            Ingredients ingredients = db.ingredients.Find(id);
            db.ingredients.Remove(ingredients);
            db.SaveChanges();
            return RedirectToAction("Ingredients", new { id = ingredients.ProductId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Buy(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products item = db.Products.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }



        //public ActionResult ViewCart(String id, String q)
        //{

        //    if (!String.IsNullOrEmpty(q))
        //    {
        //        if (String.IsNullOrEmpty(id))
        //        {
        //            try
        //            {
        //                id = TempData["ItemID"].ToString();
        //            }
        //            catch
        //            {
        //                return View();

        //            }
        //        }

        //        String quant = q;

        //        AddItemToCart(id, quant);

        //        return RedirectToAction("Index");
        //    }

        //    return View();
        //}

        //public ActionResult Remove(String ID, String note)
        //{
        //    if (Session["cart"] != null)
        //    {
        //        List<OrderItems> cart = (List<OrderItems>)Session["cart"];

        //        try
        //        {
        //            cart.Remove(cart.Where(x => x.ProductID.Equals(ID)).First());

        //            Session["cart"] = cart;

        //            return RedirectToAction("ViewCart", "Products");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message);
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("ViewCart");
        //    }
        //    TempData.Clear();
        //    return View();
        //}

        //public void AddItemToCart(String id, String quant)
        //{
        //    if (id != null)
        //    {
        //        if (Session["cart"] != null)
        //        {
        //            if (id == null)
        //            {
        //                Session["cart"] = (List<OrderItems>)Session["cart"];
        //            }
        //            else
        //            {
        //                List<OrderItems> cart = (List<OrderItems>)Session["cart"];

        //                Products it = db.Products.Where(x => x.ProductId.Equals(id)).ToList().First();

        //                //===============================Create Order Item===================================
        //                OrderItems item = new OrderItems();
        //                item.ItemNumber = 1;
        //                item.ItemName = it.ProductName;
        //                item.ProductID = it.ProductName;
        //                item.Quantity = Convert.ToInt32(quant);
        //                item.Price = Convert.ToInt32(it.Price);

        //                try
        //                {
        //                    cart.Add(item);

        //                    RedirectToAction("ViewCart");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.Message);
        //                }

        //            }
        //        }
        //        //=================================No cart exists=================================
        //        if (Session["cart"] == null)
        //        {
        //            Products ite = db.Products.Where(x => x.ProductId.Equals(id)).ToList().First();
        //            //==================================================
        //            OrderItems itemb = new OrderItems();
        //            itemb.ItemNumber = 1;
        //            itemb.ItemName = ite.ProductName;
        //            itemb.ProductID = ite.ProductId;
        //            itemb.Quantity = Convert.ToInt32(quant);
        //            itemb.Price = Convert.ToInt32(ite.Price);

        //            List<OrderItems> cart = new List<OrderItems>();
        //            cart.Add(itemb);

        //            Session["cart"] = cart;

        //            RedirectToAction("ViewCart");
        //        }

        //    }

        //}


    }
}