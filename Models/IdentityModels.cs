using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace DS3_Sprint1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string name { get; set; }

        public string lastname { get; set; }

        public string fullname { get; set; }

        public string contactnumber { get; set; }

        public string address { get; set; }

        public string businessname { get; set; }

        public string businessnumber { get; set; }

        public bool Active { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Products> Products { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Orderd> Orderss { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.OrderDetail> OrderDetails { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.ReceiveStock> receivestock { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Returns> returns { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Suppliers> suppliers { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Supplies> supplies { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.TotalReturns> totalreturns { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Cart> Carts { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.CustomerInvoice> customerInvoice { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.DailyTasks> dailyTasks { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Delivery> delivery { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Message> Messages { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Reply> Replies { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.SalesReport> salesReport { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Attendance> attendance { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Issues> issues { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Recommendations> recommendations { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.DreamProducts> dreamProducts { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.DreamProdIngredients> dreamProdIngredients { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Ingredients> ingredients { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.ReturnItems> returnItems { get; set; }

        public System.Data.Entity.DbSet<DS3_Sprint1.Models.Points> points { get; set; }
    }
}