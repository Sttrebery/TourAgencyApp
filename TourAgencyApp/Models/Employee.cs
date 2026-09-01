using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourAgencyApp.Models
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Patronimyc { get; set; }

        //[Required] //Проверку валидации узнать как делать в WPF
        public string PhoneNumber { get; set; }

        [Required]  //Проверку валидации узнать как делать в WPF
        public string Email { get; set; }

        public int? PhotoID { get; set; }
        [ForeignKey("PhotoID")]
        public Photo? Photo { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]  //преобразование в OnModelCreating в DbContext
        public PositionEnum Position { get; set; }

        public List<Tour> Tours { get; set; }
    }

    public enum PositionEnum { Director, SeniorManager, Manager }
}
