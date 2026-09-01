using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgencyApp.Data;
using TourAgencyApp.Models;

namespace TourAgencyApp
{
    public class ToursEqualityComparer : IEqualityComparer<Tour>
    {
        public bool Equals(Tour x, Tour y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            // Сравнение по Id
            return x.ID == y.ID;
        }

        public int GetHashCode(Tour obj)
        {
            if (obj == null)
                return 0;

            return obj.ID.GetHashCode();
        }
    }
}
