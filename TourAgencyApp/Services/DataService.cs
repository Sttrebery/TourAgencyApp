using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;
using System.Configuration;
using TourAgencyApp.Models;
using TourAgencyApp.Views.Client.Pages;

namespace TourAgencyApp.Services
{
    public class DataService
    {
        #region Get data From Database
        
        // получение туров
        public IEnumerable<Tour> GetTours()
        {
            List<Tour> tours = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    tours = db.Tours.Include("Photos").ToList();
                }
            }
            catch (Exception ex)
            {
                tours = new();
            }
            return tours;
        }


        //получение Актуальных туров
        public IEnumerable<Tour> GetActualTours()
        {
            List<Tour> tours = null!;

            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    tours = db.Tours.Include(t => t.Country)
                        .Include(t => t.Photos)
                        .Include(t => t.TransoprtType)
                        .Include(t => t.Hotel)
                        .ToList();
                    tours = tours.Where(t => t.IsConducted == false && t.IsOnTour == false).ToList();
                }
            }
            catch (Exception ex)
            {
                tours = new();
            }
            return tours;
        }
        //получение Актуальных туров async
        public async Task<IEnumerable<Tour>> GetActualToursAsync()
        {
            List<Tour> tours = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    tours = await db.Tours.Include(t => t.Country)
                        .Include(t => t.Photos)
                        .Include(t => t.TransoprtType)
                        .Include(t => t.Hotel)
                        .ToListAsync();
                    tours = tours.Where(t => t.IsConducted == false && t.IsOnTour == false).ToList();
                }
            }
            catch (Exception ex)
            {
                tours = new();
            }
            return tours;
        }

        // получение туров async
        public async Task<IEnumerable<Tour>> GetToursAsync()
        {
            List<Tour> actualTours = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    actualTours = await db.Tours.Include("Photos").ToListAsync();
                }
            }
            catch (Exception ex)
            {
                actualTours = new();
            }
            return actualTours;
        }

        //получение всех сотрудников
        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> emps = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    emps = db.Employees.ToList();
                }
            }
            catch
            {
                emps = new List<Employee>();
            }
            return emps;
        }

        //получение всех сотрудников async
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            List<Employee> emps = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    emps = await db.Employees.ToListAsync();
                }
            }
            catch
            {
                emps = new List<Employee>();
            }
            return emps;
        }

        //получение всех отелей
        public IEnumerable<Hotel> GetHotels()
        {
            List<Hotel> hotels = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    hotels = db.Hotels.Include("Photos").ToList();
                }
            }
            catch
            {
                hotels = new List<Hotel>();
            }
            return hotels;
        }

        /// <summary>
        /// Получение списка отелей асинхронно
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Hotel>> GetHotelsAsync()
        {
            List<Hotel> hotels = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    hotels = await db.Hotels.Include("Photos").ToListAsync();
                }
            }
            catch
            {
                hotels = new List<Hotel>();
            }
            return hotels;
        }

        //получение всех клиентов
        public IEnumerable<Tourist> GetAllClients()
        {
            List<Tourist> clients = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    clients = db.Clients.ToList();
                }
            }
            catch
            {
                clients = new List<Tourist>();
            }
            return clients;
        }

        //получение всех стран
        public IEnumerable<Country> GetAllCountries()
        {
            List<Country> countries = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    countries = db.Countries.ToList();
                }
            }
            catch
            {
                countries = new List<Country>();
            }
            return countries;
        }

        //получение всех стран async
        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            List<Country> countries = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    countries = await db.Countries.ToListAsync();
                }
            }
            catch
            {
                countries = new List<Country>();
            }
            return countries;
        }

        //получение всех способов передвижения
        public IEnumerable<Transport> GetAllTransports()
        {
            List<Transport> tr = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    tr = db.Transports.ToList();
                }
            }
            catch
            {
                tr = new List<Transport>();
            }
            return tr;
        }

        //получение всех типо трансопрта async
        public async Task<IEnumerable<Transport>> GetAllTransportsAsync()
        {
            List<Transport> tr = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    tr = await db.Transports.ToListAsync();
                }
            }
            catch
            {
                tr = new List<Transport>();
            }
            return tr;
        }

        public User GetUserByUsername(string username)
        {
            using var db = new TourAgencyDbContext();
            User user = db.Users.FirstOrDefault(x => x.Username == username);
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using var db = new TourAgencyDbContext();
            User user = await db.Users.FirstOrDefaultAsync(x => x.Username == username);
            return user;
        }

        public async Task<Tourist> GetClientByID(int id)
        {
            using var db = new TourAgencyDbContext();
            Tourist c = await db.Clients.Include("User").FirstOrDefaultAsync(cl => cl.UserID == id);
            return c;
        }

        public async Task<Employee> GetEmployeeByID(int id)
        {
            using var db = new TourAgencyDbContext();
            Employee emp = await db.Employees.Include("User").FirstOrDefaultAsync(e => e.UserID == id);
            return emp;
        }

        //Туры пользователя
        public IEnumerable<Tour> GetClientTours(int user_id)
        {
            List<Tour> tours = new List<Tour>();
            using (var db = new TourAgencyDbContext())
            {
                var client = db.Clients.Include(c=>c.ClientTours).FirstOrDefault(c => c.UserID == user_id);                        
                if (client != null) tours = client.ClientTours.ToList();
            }
            return tours;
        }

        //Туры пользователя async
        public async Task<IEnumerable<Tour>> GetClientToursAsync(int user_id)
        {
            List<Tour> tours = new List<Tour>();
            using (var db = new TourAgencyDbContext())
            {
                var client = await db.Clients.Include(c => c.ClientTours).FirstOrDefaultAsync(c => c.UserID == user_id);
                if (client != null) tours = client.ClientTours.ToList();
            }
            return tours;
        }

        //Получение данных профиля пользователя
        public Profile GetProfileByUserID(int id)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TourAgencyDB"].ConnectionString;
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = new SqlCommand("st_GetProfileByUserID", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userID", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Profile()
                {
                    Name = reader.GetString(0),
                    Surname = reader.GetString(1),
                    Patronimyc = reader.GetString(2),
                    PhoneNumber = reader.GetString(3),
                    PhotoID = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    Photo = reader.IsDBNull(5) ? null : ((byte[]?)reader.GetSqlBinary(5)),
                    Email = reader.GetString(6)
                };
            }
            return null;
        }

        // Получение популярной страны
        public (string, int?) GetTopCountry()
        {
            return GetTopCountryAsync().GetAwaiter().GetResult();
        }

        // Получение популярной страны async
        public async Task<(string, int?)> GetTopCountryAsync()
        {
            string name = null; int? count = null;
            var connectionString = ConfigurationManager.ConnectionStrings["TourAgencyDB"].ConnectionString;
            string sql = "SELECT * FROM GetTopCountry()";
            using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (await reader.ReadAsync())
                {
                    name = reader.GetString(0);
                    count = reader.GetInt32(1);
                }
            }
            return (name, count);
        }

        //  Получение популярных туров 
        public IEnumerable<Tour> GetTopActualTour()
        {
            return GetTopActualTourAsync().GetAwaiter().GetResult();
        }

        //  Получение популярных туров async 
        public async Task<IEnumerable<Tour>> GetTopActualTourAsync()
        {
            var result = new List<Tour>();
            var connectionString = ConfigurationManager.ConnectionStrings["TourAgencyDB"].ConnectionString;
            string sql = "SELECT * FROM PopularTours";
            using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    result.Add(new Tour
                    {
                        ID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                        Cost = reader.GetDecimal(3),
                        StartDate = reader.GetDateTime(4),
                        EndDate = reader.GetDateTime(5),
                        Description = reader.GetString(6),
                        MaxTouristCount = reader.GetInt32(7),
                        ResponsibleEmployeeID = reader.GetInt32(8),
                        CountyID = reader.GetInt32(9),
                        TransoprtTypeID = reader.GetInt32(10),
                        HotelID = reader.GetInt32(11),
                        IsConducted = reader.GetBoolean(12)
                    });
                }
            }
            return result;
        }

        //  Получение архивных туров по популярности async 
        public IEnumerable<Tour> GetTopArchiveTour()
        {
            return GetTopArchiveTourAsync().GetAwaiter().GetResult();
        }
        //  Получение архивных туров по популярности async 
        public async Task<IEnumerable<Tour>> GetTopArchiveTourAsync()
        {
            var result = new List<Tour>();
            var connectionString = ConfigurationManager.ConnectionStrings["TourAgencyDB"].ConnectionString;
            string sql = "SELECT * FROM PopularArchiveTours";
            using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();
            using SqlCommand cmd = new SqlCommand(sql, conn);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    result.Add(new Tour
                    {
                        ID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                        Cost = reader.GetDecimal(3),
                        StartDate = reader.GetDateTime(4),
                        EndDate = reader.GetDateTime(5),
                        Description = reader.GetString(6),
                        MaxTouristCount = reader.GetInt32(7),
                        ResponsibleEmployeeID = reader.GetInt32(8),
                        CountyID = reader.GetInt32(9),
                        TransoprtTypeID = reader.GetInt32(10),
                        HotelID = reader.GetInt32(11),
                        IsConducted = reader.GetBoolean(12)
                    });
                }
            }
            return result;
        }

        #endregion

        #region Adding data into Database

        /// <summary>
        /// Return true if added succesfully, otherwise - false
        /// </summary>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            using var db = new TourAgencyDbContext();
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Добавление страны в базу данных


        //добавить новый отель async
        public async Task AddHotel(Hotel h)
        {
            using (var db = new TourAgencyDbContext())
            {
                if (await db.Hotels.Where(old => old.Name == h.Name &&
                        old.Address == h.Address).AnyAsync())
                {
                    throw new Exception("Отель уже есть в базе");
                }

                await db.Hotels.AddAsync(h);
                await db.SaveChangesAsync();
            }
        }

        //добавить тур
        public Tour AddTour(Tour to_add)
        {
            using (var db = new TourAgencyDbContext())
            {
                db.Tours.Add(to_add);
                db.SaveChanges();
            }
            return to_add;
        }

        //добавить тур async
        public async Task<Tour> AddTourAsync(Tour to_add)
        {
            using (var db = new TourAgencyDbContext())
            {
                await db.Tours.AddAsync(to_add);
                await db.SaveChangesAsync();
            }
            return to_add;
        }

        #endregion

        #region Save Changes to Data (detached mode)
        public async Task SaveCountriesAsync(IEnumerable<Country> countries)
        {
            using (var context = new TourAgencyDbContext())
            {
                foreach (var country in countries)
                {
                    if (country.ID == 0)
                    {
                        // добавление новой записи todo: проверить работает ли без Attach
                        //context.Countries.Attach(country);
                        await context.Countries.AddAsync(country);
                    }
                    else
                    {
                        // существующую - помечаем как измененную
                        context.Countries.Attach(country);
                        context.Entry(country).State = EntityState.Modified;
                    }
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task SaveTransportsAsync(IEnumerable<Transport> transports)
        {
            using (var context = new TourAgencyDbContext())
            {
                foreach (var transport in transports)
                {
                    if (transport.ID == 0)
                    {
                        // добавление новой записи todo: проверить работает ли без Attach
                        //context.Transports.Attach(transport);
                        await context.Transports.AddAsync(transport);
                    }
                    else
                    {
                        // существующую - помечаем как измененную
                        context.Transports.Attach(transport);
                        context.Entry(transport).State = EntityState.Modified;
                    }
                }
                await context.SaveChangesAsync();
            }
        }

        #endregion

        #region Edit data (without detached mode)

        public async Task EditProfileAsync(int userID, Profile changes)
        {
            using var db = new TourAgencyDbContext();
            User u = await db.Users.FirstAsync(f => f.ID == userID);
            u.Email = changes.Email;
            db.Update(u);
            if(u.Role == RoleEnum.Client)
            {
                Tourist c = await db.Clients.Include(c=>c.Photo).FirstAsync(f => f.UserID == userID);
                db.Entry(c).CurrentValues.SetValues(changes);
                if (changes.Photo != null) c.Photo = new Photo() { PhotoValue = changes.Photo };
                else c.PhotoID = null;
            }
            else //employee
            {
                Employee emp = await db.Employees.Include(e => e.Photo).FirstAsync(f => f.UserID == userID);
                db.Entry(emp).CurrentValues.SetValues(changes);
                if(changes.Photo != null) emp.Photo = new Photo() { PhotoValue = changes.Photo };
                else emp.PhotoID = null;
            }
            await db.SaveChangesAsync();
        }

        //внести изменения в данные Отеля async
        public async Task EditHotel(int origId, Hotel changes)
        {
            using (var db = new TourAgencyDbContext())
            {
                var founded = await db.Hotels.Include("Photos").Where(h => h.ID == origId).FirstOrDefaultAsync();
                if (founded == null)
                {
                    await AddHotel(changes);
                }
                else
                {
                    db.Entry(founded).CurrentValues.SetValues(changes);

                    //удаление фото
                    foreach (var photo in founded.Photos)
                    {
                        if (!changes.Photos.Any(p => p.ID == photo.ID))
                        {
                            db.Photos.Remove(photo);
                        }
                    }

                    //  добавление/изменение новых фото
                    foreach (var photo in changes.Photos)
                    {
                        var existingPhoto = founded.Photos
                            .FirstOrDefault(p => p.ID == photo.ID);

                        if (existingPhoto == null)
                        {
                            founded.Photos.Add(photo);
                        }
                        else
                        {
                            db.Entry(existingPhoto).CurrentValues.SetValues(photo);
                        }
                    }
                }
                await db.SaveChangesAsync();
            }
        }

        //Изменение пароля пользователя async
        public async Task ChangePasswordAsync(int user_id, string new_hashed_password)
        {
            using var db = new TourAgencyDbContext();
            User founded = await db.Users.FirstAsync(u=> u.ID == user_id);
            founded.Password = new_hashed_password;
            await db.SaveChangesAsync();
        }

        //внести изменения в данные Тура async
        public async Task EditTourAsync(int origId, Tour changes)
        {
            using (var db = new TourAgencyDbContext())
            {
                var founded = await db.Tours.Include("Photos").Where(t => t.ID == origId).FirstOrDefaultAsync();
                
                if (founded == null)
                {
                    await AddTourAsync(changes);
                }
                else
                {
                    db.Entry(founded).CurrentValues.SetValues(changes);
                    
                    //удаление фото
                    foreach (var photo in founded.Photos)
                    {
                        if (!changes.Photos.Any(p => p.ID == photo.ID))
                        {
                            db.Photos.Remove(photo);
                        }
                    }

                    //  добавление/изменение новых фото
                    foreach (var photo in changes.Photos)
                    {
                        var existingPhoto = founded.Photos
                            .FirstOrDefault(p => p.ID == photo.ID);

                        if (existingPhoto == null)
                        {
                            founded.Photos.Add(photo);
                        }
                        else
                        {
                            db.Entry(existingPhoto).CurrentValues.SetValues(photo);
                        }
                    }
                }
                await db.SaveChangesAsync();
            }
        }
        #endregion

        //удалить тур (поместить в архив) async
        public async Task DeleteTourAsync(int tour_id)
        {
            using (var db = new TourAgencyDbContext())
            {
                Tour? tour_to_del = await db.Tours.FirstOrDefaultAsync(t => t.ID == tour_id);
                if (tour_to_del != null)
                {
                    tour_to_del.IsConducted = true;
                    await db.SaveChangesAsync();
                }
            }
        }

        //записаться на тур (туристом)
        public async Task<bool> SignUpFoTour(int clientId, int tourId)
        {
            try
            {
                using var db = new TourAgencyDbContext();
                var db_client = await db.Clients.Include("ClientTours").FirstAsync(c => c.ID == clientId);
                var db_tour = await db.Tours.Include(t => t.Tourists).FirstAsync(t => t.ID == tourId);

                if( db_tour.HasAvailableSpots && // !db_tour.IsOnTour - проверяется в GetActualTours
                    !db_client.ClientTours.Contains(db_tour, new ToursEqualityComparer()))
                {
                    db_client.ClientTours.Add(db_tour);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        #region Search

        //Поиск тура в диапазоне дат
        public IEnumerable<Tour> GetToursByDate(DateTime date1, DateTime date2)
        {
            List<Tour> tours = new List<Tour>();
            using (var db = new TourAgencyDbContext())
            {
                var result = db.Tours.Where(t => t.StartDate >= date1 && t.StartDate <= date2).ToList();
                tours = result;
            }
            return tours;
        }

        //Поиск тура по странам
        public IEnumerable<Tour> GetToursByCountry(string country)
        {
            List<Tour> tours = null;
            using (var db = new TourAgencyDbContext())
            {
                tours = db.Tours.Include("Country").Where(t => t.Country.Name == country).ToList();
            }
            return tours;
        }

        // Поиск тура по способу передвижения
        public IEnumerable<Tour> GetToursByTransport(string transport)
        {
            List<Tour> tours = null;
            using (var db = new TourAgencyDbContext())
            {
                tours = db.Tours.Include("TransportType").Where(t => t.TransoprtType.Name == transport).ToList();
            }
            return tours;
        }
        #endregion
        //Получение подробных сведений о туре по ID (async)
        public async Task<Tour> GetFullTourInfoByIDAsync(int tour_id)
        {
            Tour result = null!;
            using (var db = new TourAgencyDbContext())
            {
                result = await db.Tours.Include(t => t.Country)
                    .Include(t => t.Photos)
                    .Include(t => t.TransoprtType)
                    .Include(t => t.Hotel)
                    .Where(t => t.ID == tour_id)
                    .FirstAsync();
            }
            return result;
        }
    }
}
