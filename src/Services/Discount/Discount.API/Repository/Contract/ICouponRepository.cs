using Discount.API.Entities;

namespace Discount.API.Repository.Contract
{
    public interface ICouponRepository
    {
        Task<Coupon> GetDiscountAsync(string productName);
        Task<Coupon> CreateDiscountAsync(Coupon coupon);
        Task<bool> DeleteDiscountAsync(Coupon coupon);
        Task <Coupon> UpdateDiscountAsync(Coupon updateCoupon);
    }
}
