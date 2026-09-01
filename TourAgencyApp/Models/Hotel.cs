using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TourAgencyApp.Models
{
    public class Hotel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Description { get; set; }

        public List<Photo> Photos { get; set; }
        public List<Tour> Tours { get; set; }
    }
}
