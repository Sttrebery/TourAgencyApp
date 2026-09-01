using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TourAgencyApp.Models
{
    public class Photo
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public byte[] PhotoValue { get; set; }

        //todo: тут связи с другими таблицами tour, client

        public List<Hotel> Hotels { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Tourist> Clients { get; set; }
        public List<Tour> Tours { get; set; }
    }
}
