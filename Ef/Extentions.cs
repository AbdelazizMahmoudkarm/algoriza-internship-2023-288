using algoriza_internship_288.Domain.Models.Enums;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace Repository
{
    public static class Extentions
    {
        public static string GetDay(this Days day,bool arabic)
        => arabic ? ((ArDays)(int)day).ToString() : day.ToString();

        public static string GetStatus(this RequestType request,bool arabic)
        => arabic ? ((ArRequestType)(int)request).ToString() : request.ToString();


        public static string GetGender(this Gender gender, bool arabic)
      => arabic ? ((ArGender)((int)gender)).ToString() : gender.ToString();

        public static int GetAge(this DateTime dob)
        {
            int age = DateTime.Now.Year - dob.Year;
            if (DateTime.Now.DayOfYear < dob.DayOfYear)
                age -= 1;
            return age;
        }

        public static string ProcessImage(this IFormFile photo)
        {
            string uniqueName = default;
            if (photo is not null)
            {
                string path = Path.Combine("~\\..\\wwwroot", "Images");
                uniqueName = Guid.NewGuid() + "_" + photo.FileName;
                string Fullpath = Path.Combine(path, uniqueName);
                using FileStream fileStream = new(Fullpath, FileMode.Create);
                photo.CopyTo(fileStream);
            }
            return uniqueName;
        }

    }
}
