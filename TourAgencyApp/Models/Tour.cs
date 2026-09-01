using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourAgencyApp.Models
{
    public class Tour
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Cost { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int MaxTouristCount { get; set; }

        [Required]
        public int? ResponsibleEmployeeID { get; set; }
        [ForeignKey("ResponsibleEmployeeID")]
        public Employee? ResponsibleEmployee { get; set; }

        [Required]
        public int? CountyID { get; set; }
        [ForeignKey("CountyID")]
        public Country? Country { get; set; }

        [Required]
        public int? TransoprtTypeID { get; set; }
        [ForeignKey("TransoprtTypeID")]
        public Transport? TransoprtType { get; set; }

        [Required]
        public int? HotelID { get; set; }
        [ForeignKey("HotelID")]
        public Hotel? Hotel { get; set; }

        public List<Tourist> Tourists { get; set; }
        public List<Photo> Photos { get; set; }

        /// вот тут подумать
        //public bool HasAvailableSpots { get; set; } = true;
        public bool HasAvailableSpots { get { return (MaxTouristCount > Tourists.Count); } }
        public bool IsOnTour { get; set; } = false;
        public bool IsConducted { get; set; } = false;

    }
}
