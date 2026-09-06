using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgencyApp.Services;

namespace TourAgencyApp.Models
{
    [NotMapped]
    public class Profile
    {
        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronimyc { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public int? PhotoID { get; set; }

        public byte[]? Photo { get; set; }

        //default 
        public Profile()
        {

        }

        public Profile(int user_id)
        {
            var data = new DataService();
            Profile founded = data.GetProfileByUserID(user_id);
            if(founded != null)
            {
                Surname = founded.Surname;
                Name = founded.Name;
                Patronimyc = founded.Patronimyc;
                PhoneNumber = founded.PhoneNumber;
                PhotoID = founded.PhotoID;
                Photo = founded.Photo;
                Email = founded.Email;
            }
        }
    }
}
