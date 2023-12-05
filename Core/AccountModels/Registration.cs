using algoriza_internship_288.Core.Models.Enums;

namespace algoriza_internship_288.Core.AccountModels
{
    public class Registration
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public Gender? Gender { get; set; }
        public ArGender? ArGender { get; set; }
        public string? Image { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public UserType Personality { get; set; }
       // public ArPersonality ArPersonality { get; set; }
        public string Password { get; set; }
    }
}
