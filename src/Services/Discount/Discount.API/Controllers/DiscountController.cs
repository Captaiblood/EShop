using Discount.API.Entities;
using Discount.API.Repository.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly ICouponRepository couponRepository;

        public DiscountController(ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository?? throw new NullReferenceException(nameof(couponRepository));
        }

        [HttpGet("{productName}",Name ="GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)System.Net.HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            var discount = await couponRepository.GetDiscountAsync(productName);
            return Ok(discount);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)System.Net.HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            var discountCreated =  await couponRepository.CreateDiscountAsync(coupon);
            return Ok(discountCreated);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)System.Net.HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
        {
            var _couponUpdated = await couponRepository.UpdateDiscountAsync(coupon);
            return Ok(_couponUpdated);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)System.Net.HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount([FromBody] Coupon coupon, string productname)
        {           
            var deleteDisocunt = await couponRepository.DeleteDiscountAsync(coupon);
            return Ok(deleteDisocunt);
        }
    }
}
