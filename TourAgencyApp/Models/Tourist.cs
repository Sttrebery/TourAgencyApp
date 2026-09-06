using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourAgencyApp.Models
{
    public class Tourist
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Patronimyc { get; set; }

        public string PhoneNumber { get; set; }

        public int? PhotoID { get; set; }
        [ForeignKey("PhotoID")]
        public Photo? Photo { get; set; }

        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }

        public List<Tour> ClientTours { get; set; }
    }
}
