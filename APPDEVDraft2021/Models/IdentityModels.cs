using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace APPDEVDraft2021.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
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
        public DbSet<CustomerTbl> CustomerTbls { get; set; }
        public DbSet<CustomerVehicle> CustomerVehicles { get; set; }
        public DbSet<BookingTbl> BookingTbls { get; set; }
        public DbSet<CategoryTbl> CategoryTbls { get; set; }
        public DbSet<WorkshopBayTbl> WorkshopBayTbls { get; set; }
        public DbSet<StockServiceTbl> StockServiceTbls { get; set; }
        public DbSet<QuotationTbl> QuotationTbls { get; set; }
        public DbSet<QuoteDetailTbl> QuoteDetailTbls { get; set; }
        public DbSet<Product> ProductDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<QuoteCart> QuoteCarts { get; set; }
        public DbSet<RecordQuote> RecordQuotes { get; set; }
        public DbSet<ReportTbl> ReportTbls { get; set; }
        public DbSet<ReportDetailTbl> ReportDetailTbls { get; set; }
        public DbSet<ReportCart> ReportCarts { get; set; }
        public DbSet<InvoiceTbl> InvoiceTbls { get; set; }
        public DbSet<MechanicTbl> MechanicTbls { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}