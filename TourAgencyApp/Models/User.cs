using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourAgencyApp.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; } // hash с шифрованием SHA256 например

        [Required]  //Проверку валидации узнать как делать в WPF
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public RoleEnum Role { get; set; } //преобразование в OnModelCreating в DbContext

        //для один-к-одному
        public Tourist? Client { get; set; }
        public Employee? Employee { get; set; }
    }

    public enum RoleEnum { Client, Employee }
}
