using DS3_Sprint1.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DS3_Sprint1.Models
{
    public class DreamProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DreamProducts
        public ActionResult Index(string sortDate)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortDate) ? "name_desc" : "";
            ViewBag.DateSortParm = sortDate == "Date" ? "date_desc" : "Date";
            var asd = from s in db.dreamProducts
                      select s;

            switch (sortDate)
            {
                case "name_desc":
                    asd = asd.OrderByDescending(s => s.UserName);
                    break;
                case "Date":
                    asd = asd.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    asd = asd.OrderByDescending(s => s.Date);
                    break;
                default:
                    asd = asd.OrderBy(s => s.UserName);
                    break;
            }
            return View(asd.ToList());

            //return View(db.dreamProducts.ToList());
        }

        public ActionResult Successful()
        {
            return View();
        }

        // GET: DreamProducts/Details/5
        public ActionResult Details(int? id)
        {
            DreamProducts dreamProducts = db.dreamProducts.Find(id);

            Session["id"] = dreamProducts.Id.ToString();
            Session["name"] = dreamProducts.ProdName;
            Session["type"] = dreamProducts.Type;
            Session["desc"] = dreamProducts.Description;

            return View(dreamProducts);
        }

        // GET: DreamProducts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DreamProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProdName,Type,Description")] DreamProducts dreamProducts)
        {
            if (ModelState.IsValid)
            {
                dreamProducts.UserName = User.Identity.Name;
                dreamProducts.Date = DateTime.Now.Date;

                db.dreamProducts.Add(dreamProducts);
                db.SaveChanges();

                Session["id"] = dreamProducts.Id.ToString();

                return RedirectToAction("AddIngredients");
            }

            return View(dreamProducts);
        }

        public ActionResult AddIngredients(/*int? id*/)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            var pid = (string)Session["id"];
            var id = Convert.ToInt16(pid);

            var Ingredients = new DreamProdIngredientsViewModel
            {
                DreamProduct = db.dreamProducts.Include(i => i.Supplies).First(i => i.Id == id),
            };

            if (Ingredients.DreamProduct == null)
                return HttpNotFound();

            var inglist = db.supplies.ToList();
            Ingredients.SupplyList = inglist.Select(o => new SelectListItem
            {
                Text = o.SupplyName,
                Value = o.SupplyId.ToString()
            });

            //ViewBag.Employee =
            //        new SelectList(db.Users, "Id", "FullName", Ingredients.DailyTask.Name);

            return View(Ingredients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//[Bind(Include = "Title,Id,EmployerID,SelectedJobTags")]
        public ActionResult AddIngredients(DreamProdIngredientsViewModel ingView)
        {

            if (ingView == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);



            if (ModelState.IsValid)
            {
                var ToUpdate = db.dreamProducts
                    .Include(i => i.Supplies).First(i => i.Id == ingView.DreamProduct.Id);

                if (TryUpdateModel(ToUpdate, "DreamProducts", new string[] { "ProdName", "Type", "Description", "UserName" }))
                {
                    var newIngredients = db.supplies.Where(
                        m => ingView.supplies.Contains(m.SupplyId)).ToList();
                    var updatedIngredients = new HashSet<int>(ingView.supplies);
                    foreach (Supplies supplies in db.supplies)
                    {
                        if (!updatedIngredients.Contains(supplies.SupplyId))
                        {
                            ToUpdate.Supplies.Remove(supplies);
                        }
                        else
                        {
                            ToUpdate.Supplies.Add((supplies));
                        }
                    }

                    ApplicationDbContext cont = new ApplicationDbContext();
                    var list = new List<DreamProdIngredients>();
                    foreach (var item in newIngredients)
                    {
                        list.Add(new DreamProdIngredients
                        {
                            SupplyId = item.SupplyId,
                            SupplyName = item.SupplyName,
                            DreamProdId = ToUpdate.Id
                        });
                        db.dreamProdIngredients.AddRange(list);
                    }

                    db.Entry(ToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Successful");
                //return RedirectToAction("Index");
            }
            //ViewBag.Employee = new SelectList(db.Users, "Id", "FullName", ingView.DailyTask.Name);
            return View(ingView);
        }

        public ActionResult Ingredients(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dreamProducts = db.dreamProdIngredients.ToList().Where(x => x.DreamProdId == id);

            if (dreamProducts == null)
            {
                return HttpNotFound();
            }
            return View(dreamProducts);
        }

        // GET: DreamProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DreamProducts dreamProducts = db.dreamProducts.Find(id);
            if (dreamProducts == null)
            {
                return HttpNotFound();
            }
            return View(dreamProducts);
        }

        // POST: DreamProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProdName,Type,Description")] DreamProducts dreamProducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dreamProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dreamProducts);
        }

        // GET: DreamProducts/Delete/5
        public ActionResult Delete()
        {
            var pid = (string)Session["id"];
            var id = Convert.ToInt16(pid);

            DreamProducts dreamProducts = db.dreamProducts.Find(id);
   
            return View(dreamProducts);
        }

        // POST: DreamProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            var pid = (string)Session["id"];
            var id = Convert.ToInt16(pid);

            DreamProducts dreamProducts = db.dreamProducts.Find(id);

            var user = db.Users.ToList().Find(x => x.UserName == dreamProducts.UserName);

            try
            {
                var senderEmail = new MailAddress("evan.gov07@gmail.com", "Elenas Delicacies");
                var recieveremail = new MailAddress(user.UserName, "Reciever");
                var password = "hzea bfpq aihm vyiz";

                int date = DateTime.Now.Year;
                string thisyear = Convert.ToString(date);

                var sub = "Decision for Dream Product Idea";
                var body = "Dear " + user.fullname + " , Your Dream Product idea (" + dreamProducts.ProdName + ") has been rejected." + "<br />" + "We thank you for your idea though and hope to get new ideas from you in the future!" + "<br />" + "Regards" + "<br />" + "Elenas Delicacies";

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

            db.dreamProducts.Remove(dreamProducts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
