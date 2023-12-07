﻿using algoriza_internship_288.Domain.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.DtoClasses
{
    public  class BaseDto
    {
        public IFormFile Image { get; set; }
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        [EmailAddress]
       // [RegularExpression("[A-Za-z0-9]{6,}[@][a-z]+[.][a-z]+")]
        public string Email { get; set; }
        [MinLength(11),MaxLength(20)]
        public string Phone { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        [Compare("ConfirmPassword"),PasswordPropertyText(true)]
        //[RegularExpression("[A-Z]+[A-za-z]{6,}[-_$@]{1}[a-z09]+")]
        public string Password { get; set; }
        [PasswordPropertyText(true)]
        public string ConfirmPassword { get; set; }
    }
}
