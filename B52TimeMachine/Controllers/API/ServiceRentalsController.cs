using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using B52TimeMachine.Dtos;
using B52TimeMachine.Models;
using System.Data.Entity;
using AutoMapper;
using System.Collections;

namespace B52TimeMachine.Controllers.API
{
    public class ServiceRentalsController : ApiController
    {
        ApplicationDbContext _context;
        public ServiceRentalsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [ActionName("AddServiceRental")]
        [HttpPost]
        public IHttpActionResult AddServiceRental(ServiceRentalDto serviceRentalDto)
        {
            var rentalId = serviceRentalDto.RentalId;
            var rental = _context.Rentals.Where(r => r.RentalId == rentalId).SingleOrDefault();
            var addedServices = _context.ServiceRentals.Where(s => s.RentalId == rentalId).ToList();
            var warehouses = _context.Warehouses.Include(w => w.Service).ToList();

            if (rental == null)
                return BadRequest("Invalid rental");

            if (warehouses.Count <= 0)
                return BadRequest("You need to restock the services before adding");

            foreach (var item in serviceRentalDto.ServiceRentals)
            {
                var addedService = addedServices.Where(s => s.ServiceId == item.ServiceId).SingleOrDefault();
                if (addedService == null)
                {
                    var service = _context.Services.Where(s => s.ServiceId == item.ServiceId).Single();

                    var serviceRental = new ServiceRental
                    {
                        Rental = rental,
                        Service = service,
                        Quantity = item.Quantity
                    };
                    _context.ServiceRentals.Add(serviceRental);
                }
                else
                {
                    addedService.Quantity += item.Quantity;
                }
                warehouses.Single(w => w.Service.ServiceId == item.ServiceId).Quantity -= item.Quantity;
            }
            _context.SaveChanges();
            return Json("Service added");
        }

        //edit function per one add or minus click (obsolete)
        [ActionName("EditServiceRental")]
        [HttpPost]
        public IHttpActionResult EditServiceRental(int Id, int rentalId, int value)
        {
            var serviceRental = _context.ServiceRentals.Where(s => s.ServiceId == Id && s.RentalId == rentalId).SingleOrDefault();

            if (serviceRental == null)
                return BadRequest("Invalid service");

            if (value > 0)
                serviceRental.Quantity += 1;
            else
                if (serviceRental.Quantity == 1)
                    _context.ServiceRentals.Remove(serviceRental);
                else if (serviceRental.Quantity > 1)
                    serviceRental.Quantity -= 1;

            _context.SaveChanges();

            return Json("Edit successfully");
        }

        [HttpPut]
        public IHttpActionResult EditServiceRentals(ServiceRentalDto serviceRentalDto)
        {
            var rental = _context.Rentals.Where(r => r.RentalId == serviceRentalDto.RentalId).SingleOrDefault();

            if (rental == null)
                return BadRequest("Invalid rental");

            var serviceRentalsInDb = _context.ServiceRentals.Where(s => s.RentalId == serviceRentalDto.RentalId).ToList();
            var warehouse = _context.Warehouses.Include(s => s.Service).ToList();

            foreach (var item in serviceRentalDto.ServiceRentals)
            {
                var serviceRental = serviceRentalsInDb.Where(s => s.ServiceId == item.ServiceId).Single();

                if (item.Quantity == 0)
                    _context.ServiceRentals.Remove(serviceRental);

                if (serviceRental.Quantity != item.Quantity)
                { 
                    //edit warehouse quantity
                    warehouse.Single(w => w.Service.ServiceId == item.ServiceId).Quantity += serviceRental.Quantity - item.Quantity;
                    //edit service rental quantity
                    serviceRental.Quantity = item.Quantity;
                }
            }
            _context.SaveChanges();
            return Ok("Saved");
        }

        [HttpGet]
        public IHttpActionResult GetServiceByRentalId(int Id)
        {
            var serviceRental = _context.ServiceRentals.Include(s => s.Service)
                                                       .Where(s => s.RentalId == Id).ToList();

            var serviceRentalDto = new ServiceRentalDto()
            {
                RentalId = Id,
                ServiceRentals = new List<ServiceRentalAddingProps> { }
            };

            foreach (var item in serviceRental)
            {
                serviceRentalDto.ServiceRentals.Add(new ServiceRentalAddingProps() { 
                    ServiceId = item.ServiceId,
                    Name = item.Service.Name,
                    Price = item.Service.Price,
                    SumPrice = item.Service.Price * item.Quantity,
                    Quantity = item.Quantity
                });
            }

            return Ok(serviceRentalDto);
        }
    }
}
