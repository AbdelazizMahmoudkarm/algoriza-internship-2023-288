using algoriza_internship_288.Domain.Models.Enums;
using Domain.DtoClasses.Coupon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserType.Admin))]
    public class SettingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public SettingController(IUnitOfWork unitOfWork)
        =>  _unitOfWork = unitOfWork;
        
        [HttpPost("AddCoupon")]
        public async Task<IActionResult> AddAsync(AddCouponDto couponDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
               bool result = _unitOfWork.Coupon.Add(couponDto);
            if (result)
                await _unitOfWork.SaveAsync();
            return Ok(result);
        }
        [HttpPut("UpdateCoupon")]
        public async Task<IActionResult> Update(EditCouponDto couponDto)
        {
            
            if (!ModelState.IsValid)
                return BadRequest();
            bool  result =await _unitOfWork.Coupon.UpdateAsync(couponDto);
                if(result)
                await _unitOfWork.SaveAsync();
            
            return Ok(result);
        }
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if(id == 0)
                return BadRequest();
           bool result= await _unitOfWork.Coupon.DeleteAsync(id);
            if (result)
                await _unitOfWork.SaveAsync();
            
            return Ok(result);
        }
        [HttpPut("Deactive")]
        public async Task<IActionResult> DeactiveAsync(int couponId)
        {
            if(couponId == 0)
                return BadRequest();
            bool result =await _unitOfWork.Coupon.DeactiveAsync(couponId);
            if (result)
                await _unitOfWork.SaveAsync();
            return Ok(result);
        }

    }
}
