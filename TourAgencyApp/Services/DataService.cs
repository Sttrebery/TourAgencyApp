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
            List<Tour> actualTours = null!;
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    actualTours = db.Tours.Include("Photos").ToList();
                }
            }
            catch (Exception ex)
            {
                actualTours = new();
            }
            return actualTours;
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

        //Туры пользователя :todo
        public IEnumerable<Tour> GetClientTours(string name, string surname, string patronimyc)
        {
            List<Tour> tours = new List<Tour>();
            using (var db = new TourAgencyDbContext())
            {
                //fOrDef + try-catch or if
                var clients = db.Clients.Include("ClientTours").First(c => c.ID == 1); //брать айди из User.ID, который будет хранится при входе в программу
                                                                                       //(отношение таблиц 1-1)
                tours = clients.ClientTours.ToList();
            }
            return tours;
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
        // todo: Получение популярной страны
        //public void GetTopCountry(out string name, out int count)
        //{
        //    using (var db = new TourAgencyDbContext())
        //    {
        //        var res = db.Populist_Country();
        //        name = res.Select(r => r.NameCountry).First();
        //        count = res.Select(r => r.AllTours).First() ?? 0;
        //    }
        //}

        // todo:  Получение популярного актуального тура
        //public IEnumerable<Tour> GetTopActualTour()
        //{
        //    List<Tour> topTour = new List<Tour>();
        //    try
        //    {
        //        using (var db = new TourAgencyDbContext())
        //        {
        //            var res = db.PopularTour;
        //            Tour tmp = db.Tours.First(t => t.NameTour == res.Select(r => r.NameTour).FirstOrDefault());
        //            topTour.Add(tmp);
        //        }
        //    }
        //    catch { }
        //    return topTour;
        //}

        // todo: Получение популярного отеля
        //
        //public IEnumerable<Hotel> GetTopHotel()
        //{
        //    List<Hotel> topHotel = new List<Hotel>();
        //    try
        //    {
        //        using (var db = new TourAgencyDbContext())
        //        {
        //            var res = db.Populist_Hotel();
        //            string name = res.Select(r => r.NameHotel).First();
        //            Hotel tmp = db.Hotels.First(h => h.Name_ == name);
        //            topHotel.Add(tmp);
        //        }
        //    }
        //    catch { }
        //    return topHotel;
        //}

        // todo: Получение непопулярного тура - также как и популярные, просто last, а не first
        //public IEnumerable<Tour> GetUnpopularTour()
        //{
        //    List<Tour> antiTour = new List<Tour>();
        //    try
        //    {
        //        using (var db = new TourAgencyDbContext())
        //        {
        //            var res = db.AntiPopularTour;
        //            Tour tmp = db.Tours.First(t => t.NameTour == res.Select(r => r.NameTour).FirstOrDefault());
        //            antiTour.Add(tmp);
        //        }
        //    }
        //    catch {}
        //    return antiTour;
        //}

        // todo: Получение активного пользователя
        //public string GetActiveTourist()
        //{
        //    string tourist = string.Empty;
        //    try
        //    {
        //        using (var db = new TourAgencyDbContext())
        //        {
        //            var res = db.ActiveTourist().First();
        //            tourist = $"{res.Surname} {res.FirstName} {res.Patronymic}";
        //        }
        //    }
        //    catch { }
        //    return tourist;
        //}


        //todo: check записаться на тур( туристом)
        public async Task<bool> SignUpFoTour(Tourist client, Tour tour)
        {
            try
            {
                //sp_проверка не записан ли турист(если не получится, то c#)
                using var db = new TourAgencyDbContext();
                var db_client = db.Clients.Include("ClientTours").FirstOrDefault(c => c.ID == client.ID);
                if(!db_client.ClientTours.Contains(tour, new ToursEqualityComparer()))
                {
                    db_client.ClientTours.Add(tour);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //удалить тур (поместить в архив) async
        public async Task DeleteTourAsync(int tour_id)
        {
            using (var db = new TourAgencyDbContext())
            {
                Tour? tour_to_del = await db.Tours.FirstOrDefaultAsync(t => t.ID == tour_id);
                if(tour_to_del != null)
                {
                    tour_to_del.IsConducted = true;
                    await db.SaveChangesAsync();
                }
            }
        }


        //удалить отель из базы 
        public void DeleteHotel(string hotelName)
        {
            using (var db = new TourAgencyDbContext())
            {
                var founded = db.Hotels.Where(h => h.Name == hotelName).ToList();
                if (founded != null)
                {
                    foreach (var f in founded)
                    {
                        db.Hotels.Remove(f);
                    }
                    db.SaveChanges();
                }
            }
        }

        //todo: проверка клиента в туре ли он
        //public string CheckClient(string surname, string name, string patronimyc)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        using(var db = new TourAgencyDbContext())
        //        {
        //            var temp = db.stp_GetLocation2(name,patronimyc,surname).First();

        //            if (temp.IsOnTour.Value)
        //            {
        //                result = $"{temp.Surname} {temp.FirstName} {temp.Patronymic} в туре {temp.NameTour} с {temp.StartDate} по {temp.EndDate}";
        //            }
        //            else
        //            {
        //                result = $"{temp.Surname} {temp.FirstName} {temp.Patronymic} сейчас не в туре";
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        result = e.Message;
        //    }
        //    return result;
        //}

        // ======Поиск=====

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

        //todo: check Поиск тура по странам
        public IEnumerable<Tour> GetToursByCountry(string country)
        {
            List<Tour> tours = null;
            using (var db = new TourAgencyDbContext())
            {
                tours = db.Tours.Include("Country").Where(t => t.Country.Name == country).ToList();
            }
            return tours;
        }

        //todo: check Поиск тура по способу передвижения
        public IEnumerable<Tour> GetToursByTransport(string transport)
        {
            List<Tour> tours = null;
            using (var db = new TourAgencyDbContext())
            {
                tours = db.Tours.Include("TransportType").Where(t => t.TransoprtType.Name == transport).ToList();
            }
            return tours;
        }

    }
}
