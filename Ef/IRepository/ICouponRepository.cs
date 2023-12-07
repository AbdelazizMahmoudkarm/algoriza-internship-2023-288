﻿using algoriza_internship_288.Domain.Models;
using Domain.DtoClasses.Coupon;

namespace Repository.IRepository
{
    public  interface ICouponRepository
    {
        public double GetDiscount(int? couponId, double price);
        public int GetIdOrDefault();
        public Task<bool> UpdateAsync(EditCouponDto couponModel);
        public  Task<bool> DeleteAsync(int id);
        public  Task<Coupon> GetCouponByCode(string code);
        public bool Add(AddCouponDto couponModel);
        public  Task<bool> DeactiveAsync(int couponId);
    }
}
