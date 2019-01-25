using B52TimeMachine.Dtos;
using B52TimeMachine.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace B52TimeMachine.Controllers.API
{
    public class RentalsController : ApiController
    {
        ApplicationDbContext _context;
        public RentalsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        [ActionName("GetRental")]
        public IHttpActionResult GetRental(int Id)
        {
            var rental = _context.Rentals.Include(p => p.Ps)
                                         .SingleOrDefault(r => r.RentalId == Id);

            if (rental == null)
                return BadRequest("Invalid rental");

            var serviceFee = (from s in _context.ServiceRentals
                             where s.RentalId == Id
                             group s by 1 into g
                             select new { svFee = g.Sum(x => (x.Service.Price * x.Quantity)) }).SingleOrDefault();

            double rentalFee = 0;
            double rentalTimeSpan = (rental.To == null) ? DateTime.Now.Subtract(rental.From).TotalMinutes : Convert.ToDateTime(rental.To).Subtract(rental.From).TotalMinutes;
            int price = rental.Ps.Price;

            rentalFee = CalculateMethods.RentalFeeCalculate(rentalTimeSpan, price);

            var rentalDto = new RentalDto 
            { 
                RentalId = rental.RentalId,
                From = rental.From.ToString("d MMM H:m"),
                To = (rental.To == null) ? DateTime.Now.ToString("d MMM H:m") : Convert.ToDateTime(rental.To).ToString("d MMM H:m"),
                RentalFee = Convert.ToInt32(rentalFee),
                ServiceFee = serviceFee == null ? 0 : serviceFee.svFee
            };

            rental.RentalFee = Convert.ToInt32(rentalFee);
            rental.ServiceFee = serviceFee == null ? 0 : serviceFee.svFee;

            _context.SaveChanges();

            return Ok(rentalDto);
        }

        [HttpPost]
        [ActionName("CreateRental")]
        public IHttpActionResult CreateRental(int Id)
        {
            if (Id == 0)
                return BadRequest("Invalid ps id");

            var ps = _context.Pss.SingleOrDefault(p => p.PsId == Id);

            if(ps == null)
                return BadRequest("Invalid ps");

            if (!ps.IsAvailable)
                return BadRequest("This ps is not available");

            var rental = new Rental()
            {
                From = DateTime.Now,
                Ps = ps,
                To = null
            };
            ps.IsAvailable = false;

            _context.Rentals.Add(rental);
            _context.SaveChanges();

            return Ok(rental.RentalId);
        }

        [HttpPut]
        [ActionName("CheckOut")]
        public IHttpActionResult CheckOut(int Id)
        {
            var rental = _context.Rentals.Include(p => p.Ps)
                                         .SingleOrDefault(r => r.RentalId == Id);
            var ps = _context.Pss.Single(p => p.PsId == rental.Ps.PsId);

            ps.IsAvailable = true;
            rental.To = DateTime.Now;

            _context.SaveChanges();
            return Ok(rental);
        }

        
        [HttpPut]
        [ActionName("SwitchPs")]
        public IHttpActionResult SwitchPs(SwitchPsDto switchPsDto)
        {
            var oldPs = _context.Pss.SingleOrDefault(p => p.PsId == switchPsDto.OldPsId);
            var newPs = _context.Pss.SingleOrDefault(p => p.PsId == switchPsDto.NewPsId);

            if (oldPs == null)
                return BadRequest("Invalid old ps");
            if (newPs == null)
                return BadRequest("Invalid new ps");

            var oldRental = _context.Rentals.Where(r => r.Ps.PsId == switchPsDto.OldPsId && r.To == null).Single();

            if (switchPsDto.Case.Equals("1"))
            {
                oldPs.IsAvailable = true;
                newPs.IsAvailable = false;
                oldRental.Ps = newPs;
            }
            else if (switchPsDto.Case.Equals("2"))
            {
                var newRental = _context.Rentals.Where(r => r.Ps.PsId == switchPsDto.NewPsId && r.To == null).Single();
                oldRental.Ps = newPs;
                newRental.Ps = oldPs;
            }
            _context.SaveChanges();
            return Ok();
        }
    }
}
