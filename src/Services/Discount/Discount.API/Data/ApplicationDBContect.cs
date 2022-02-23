using Discount.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discount.API.Data
{
    public class ApplicationDBContect:DbContext
    {
        public ApplicationDBContect(DbContextOptions<ApplicationDBContect> options):base(options)
        {

        }

        //it is possible to simply initialize the property to null with the help of the null-forgiving operator (!):
        //public DbSet<Coupon>  Coupons { get; set; } = null!;
        public DbSet<Coupon> Coupons => Set<Coupon>();
    }
}
