using Discount.API.Data;
using Discount.API.Entities;
using Discount.API.Repository.Contract;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Discount.API.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDBContect _dbContext;

        public CouponRepository(ApplicationDBContect applicationDbContext)
        {
            this._dbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        }
        /// <summary>
        /// Add a new coupon record.   
        /// </summary>
        /// <param name="coupon">Entity Company</param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="coupon"/> is null.
        /// </exception>
        public async Task<Coupon> CreateDiscountAsync(Coupon coupon)
        {
            if (coupon == null) throw new ArgumentNullException(nameof(coupon));

            await _dbContext.AddAsync<Coupon>(coupon);
            await Save();
            return coupon;
        }

        ///<summary>
        /// Remove a coupon record.   
        /// </summary>
        /// <param name="coupon">Coupon Model</param>
        /// <remarks>coupon:<code>coupon == null throw exception</code></remarks>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// When <paramref name="coupon"/> is null.
        /// </exception>
        public async Task<bool> DeleteDiscountAsync(Coupon coupon)
        {
            if (coupon == null) throw new ArgumentNullException(nameof(coupon));

            var couponToDelete = _dbContext.Coupons.Find(coupon.Id);

            if (couponToDelete != null)
            {
                _dbContext.Coupons.Remove(couponToDelete);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return Coupon. 
        /// </summary>
        /// <param name="productName">int CompanyId from DB</param>  
        /// <remarks>productName:<code>productName == "" Or null throw exception </code></remarks>
        /// <returns>Coupon Model Or Empty Model</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// When <paramref name="productName"/> is 0 or negative.
        /// </exception>
        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))  throw new ArgumentNullException(nameof(productName));

            var couponModel = new Coupon();

            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == productName);

            if (coupon == null)
                return couponModel;

            return coupon;
        }

        /// <summary>
        /// Update a coupon record.   
        /// </summary>
        /// <param name="updateCoupon">Entity Company</param>
        /// <remarks>updateCoupon:<code>updateCoupon == null throw exception </code></remarks>
        /// <returns>Coupon updateCoupon Or Empty Coupon Model</returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="updateCoupon"/> is null.
        /// </exception>
        public async Task<Coupon> UpdateDiscountAsync(Coupon updateCoupon)
        {
            if (updateCoupon == null) throw new ArgumentNullException(nameof(updateCoupon));

            var couponModel = new Coupon();

            var getCcouponToUpdate = await _dbContext.Coupons.FindAsync(updateCoupon.Id);

            if (getCcouponToUpdate == null)
            {
                return couponModel;
            }

            getCcouponToUpdate.Amount = updateCoupon.Amount;
            getCcouponToUpdate.ProductDiscription = updateCoupon.ProductDiscription;
            await Save();


            return getCcouponToUpdate;


        }

        private async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
