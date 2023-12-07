using algoriza_internship_288.Domain.Models.Enums;
using Domain.Models.Enums;

namespace Repository
{
    public static class ConvertEnumsBetweenArAndEn
    {
        public static string GetDay(this Days day,bool arabic)
        => arabic ? ((ArDays)(int)day).ToString() : day.ToString();

        public static string GetStatus(this RequestType request,bool arabic)
        => arabic ? ((ArRequestType)(int)request).ToString() : request.ToString();


        public static string GetGender(this Gender gender, bool arabic)
      => arabic ? ((ArGender)((int)gender)).ToString() : gender.ToString();

    }
}
