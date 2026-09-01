using System;
using System.ComponentModel.DataAnnotations;

namespace TourAgencyApp.Models
{
    public class Country
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public List<Tour> Tours { get; set; }
    }
}
