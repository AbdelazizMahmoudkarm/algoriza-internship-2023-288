﻿using algoriza_internship_288.Core.Models;
using algoriza_internship_288.Core.Models.Enums;
using algoriza_internship_288.Ef.DAL;
using Domain.DtoClasses.Coupon;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;
        private readonly IBookingRepository _booking;
        public CouponRepository(AppDbContext context, IBookingRepository booking)
        {
            _context = context;
            _booking = booking;
        }
        public async Task<Coupon> GetCouponByCode(string code)
        {
            return (await _context.Coupons.
                FirstOrDefaultAsync(x =>x.Code==code && x.Avaliable));
        }
        public int GetIdOrDefault()
        {
            Coupon coupon = _context.Coupons
                .Where(x => x.Avaliable).FirstOrDefault();
            if (coupon is not null)
            {
                return coupon.Id;
            }
            return default;
        }
        public bool Add(AddCouponDto couponModel)
        {
            if (couponModel is not null)
            {
                _context.Coupons.Add(new()
                {
                    Avaliable = true,
                    Code = couponModel.Code,
                    Value = couponModel.Value,
                    DiscountType = couponModel.DiscountType,
                    NumberOfRequestCompleted = couponModel.NumberOfRequestCompleted,
                });
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(EditCouponDto couponModel)
        {
            if (couponModel is not null)
            {
                bool IsCouponExistInBooking = await _booking.CheckIfCouponExistBookingAsync(couponModel.Id);
                if (!IsCouponExistInBooking)
                {
                    _context.Coupons.Update(new()
                    {
                        Id = couponModel.Id,
                        Avaliable = couponModel.Avaliable,
                        DiscountType = couponModel.DiscountType,
                        Code= couponModel.Code,
                        Value = couponModel.Value,
                        NumberOfRequestCompleted = couponModel.NumberOfRequestCompleted,
                    });
                    return true;
                }
            }
            return false;
        }
        public double GetDiscountAsync(int? couponId, double price)
        {
            if (couponId.HasValue && couponId.Value > 0)
            {
                var coupon = _context.Coupons.Find(couponId);
                if (coupon is not null)
                {
                    if (coupon.DiscountType.Equals(DiscountType.Value))
                        return price - coupon.Value;

                    else
                        return price - ((coupon.Value / 100) * price);
                }
            }
            return default;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            if (id != 0)
            {
                bool isExist = await _booking.CheckIfCouponExistBookingAsync(id);
                if (!isExist)
                {
                    _context.Coupons.Where(x => x.Id == id).ExecuteDelete();
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> DeactiveAsync(int couponId)
        {
            Coupon coupon = (await _context.Coupons.FindAsync(couponId));
            if(coupon is not null)
            {
                coupon.Avaliable = false;
            }
            return false;
        }
    }
}
