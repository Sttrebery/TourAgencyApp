using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;
using TourAgencyApp.Views.Client.Pages;
using TourAgencyApp.Models;
using Microsoft.EntityFrameworkCore;

namespace TourAgencyApp.Services
{
    public class DataService
    {
        //todo
        // получение актуальных туров
        public IEnumerable<Tour> GetTours()
        {
            using (var db = new TourAgencyDbContext())
            {
                List<Tour> actualTours = new List<Tour>();
                var res = db.Tours.ToList();
                foreach (var tour in res)
                {
                    Tour tmp = new Tour();
                    tmp.ID = tour.ID;
                    tmp.Name = tour.Name;
                    tmp.Cost = tour.Cost;
                    tmp.StartDate = tour.StartDate;
                    tmp.EndDate = tour.EndDate;
                    tmp.Description = tour.Description;
                    tmp.MaxTouristCount = tour.MaxTouristCount;
                    tmp.ResponsibleEmployeeID = tour.ResponsibleEmployeeID;
                    tmp.CountyID = tour.CountyID;
                    tmp.TransoprtTypeID = tour.TransoprtTypeID;
                    tmp.HotelID = tour.HotelID;
                    tmp.IsOnTour = tour.IsOnTour;
                    tmp.IsConducted = tour.IsConducted;

                    actualTours.Add(tmp);
                }
                return actualTours;
            }
        }

        //получение архивных туров
        public IEnumerable<Tour> GetArchiveTours()
        {
            List<Tour> archives = new List<Tour>();
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    archives = db.Tours.Where(t => t.IsConducted == true).ToList();
                }
            }
            catch
            { }
            return archives;
        }

        //получение всех сотрудников
        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> emps = new List<Employee>();
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    emps = db.Employees.ToList();
                }
            }
            catch
            { }
            return emps;
        }

        //получение всех отелей
        public IEnumerable<Hotel> GetHotels()
        {
            List<Hotel> hotels = new List<Hotel>();
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    hotels = db.Hotels.ToList();
                }
            }
            catch
            { }
            return hotels;
        }

        //получение всех клиентов
        public IEnumerable<Tourist> GetAllClients()
        {
            List<Tourist> clients = new List<Tourist>();
            try
            {
                using (var db = new TourAgencyDbContext())
                {
                    clients = db.Clients.ToList();
                }
            }
            catch
            { }
            return clients;
        }

        //получение всех стран
        public IEnumerable<Country> GetAllCountries()
        {
            List<Country> countries;
            using (var db = new TourAgencyDbContext())
            {
               countries = db.Countries.ToList();
            }
            return countries;
        }

        //получение всех способов передвижения
        public IEnumerable<Transport> GetAllTransports()
        {
            List<Transport> tr;
            using (var db = new TourAgencyDbContext())
            {
                tr = db.Transports.ToList();
            }
            return tr;
        }

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

        //Туры пользователя
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

        //добавить тур
        public Tour AddTour(Tour to_add)
        {
            using (var db = new TourAgencyDbContext())
            {
                if( db.Tours.Where(t => t.Name ==  to_add.Name).Any()) //todo: сделать проверку по ВСЕМ полям
                {
                    throw new Exception("Такой тур уже существует");
                }

                db.Tours.Attach(to_add);
                db.Tours.Add(to_add);
                db.SaveChanges();
            }
            return to_add;
        }

        //удалить тур (поместить в архив)
        public void DeleteActualTour(Tour del)
        {
            using (var db = new TourAgencyDbContext())
            {
                Tour tour_to_del = db.Tours.FirstOrDefault(t => t.ID == del.ID);
                if( tour_to_del != null )
                {
                    tour_to_del.IsOnTour = false;
                    tour_to_del.IsConducted = false;
                    db.SaveChanges();
                }
            }
        }

        //добавить новый отель
        public void AddNewHotel(Hotel h)
        {
            using (var db = new TourAgencyDbContext())
            {
                if (db.Hotels.Where(h2 => h2.Name == h.Name).Any()) //todo: сделать проверку по ВСЕМ полям
                {
                    throw new Exception("Отель уже есть в базе");
                }
                db.Hotels.Attach(h);
                db.Hotels.Add(h);
                db.SaveChanges();
            }
        }

        //внести изменения в данные Отеля
        public void EditHotel(string orig_name, Hotel changes)
        {
            using (var db = new TourAgencyDbContext())
            {
                var founded = db.Hotels.Include("Photos").Where(h => h.Name == orig_name).ToList();
                if (founded == null)
                {
                    AddNewHotel(changes);
                }
                else
                {
                    foreach(var f in founded)
                    {
                        f.Name = changes.Name;
                        f.Address = changes.Address;
                        f.Description = changes.Description;
                        f.Photos = changes.Photos;
                    }
                    db.SaveChanges();
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

        //todo: получение клиента по ФИО
        //public AgencyClients GetClientByFIO(string surname, string name, string patronimyc)
        //{
        //    AgencyClients client;
        //    try
        //    {
        //        using (var db = new TourAgencyDbContext())
        //        {
        //            client = db.AgencyClients.Where(c => c.Name_ == name && c.Surname == surname && c.Patronymic == patronimyc).FirstOrDefault();
        //        }
        //    }
        //    catch
        //    {
        //        client = null;
        //    }
        //    return client;
        //}

        //todo: получени сотрудника по логину
        //public Employee GetEmployeeByLogin(string login)
        //{
        //    Employee emp;
        //    try
        //    {
        //        using (var db = new TourAgencyDbContext())
        //        {
        //            emp = db.Employees.Where(e => e.Login == login).FirstOrDefault();
        //        }
        //    }
        //    catch
        //    {
        //        emp = null;
        //    }
        //    return emp;
        //}

        //todo: добавление нового сотрудника
        public bool AddNewEmployee(Employee e)
        {
            bool result;
            try
            {
                using(var db = new TourAgencyDbContext())
                {
                    db.Employees.Add(e);
                    db.SaveChanges();
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        //todo: добавление нового клиента
        //public bool AddNewClient(AgencyClients ac)
        //{
        //    bool result;
        //    try
        //    {
        //        using (var db = new TourAgencyDbContext())
        //        {
        //            db.AgencyClients.Add(ac);
        //            db.SaveChanges();
        //        }
        //        result = true;
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    return result;
        //}
    }
}
