using algoriza_internship_288.Core.Models;
using Domain.DtoClasses.Coupon;
using Domain.DtoClasses.Doctor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SettingController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        [HttpPost("AddCoupon")]
        public async Task<IActionResult> AddAsync(AddCouponDto couponDto)
        {
            bool isOk = false;
            if(couponDto is null)
                return BadRequest();
            if(ModelState.IsValid)
            {
                isOk = _unitOfWork.Coupon.Add(couponDto);
                if (isOk)
                    await _unitOfWork.SaveAsync();
            }
            return Ok(isOk);
        }
        [HttpPut("UpdateCoupon")]
        public async Task<IActionResult> Update(EditCouponDto couponDto)
        {
            bool isOk = false;
            if (couponDto is null)
                return BadRequest();
            if (ModelState.IsValid)
            {
                isOk =await _unitOfWork.Coupon.UpdateAsync(couponDto);
                if(isOk)
                await _unitOfWork.SaveAsync();
            }
            return Ok(isOk);
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
            bool reault =await  _unitOfWork.Coupon.DeactiveAsync(couponId);
            return Ok(reault);
        }

    }
}
