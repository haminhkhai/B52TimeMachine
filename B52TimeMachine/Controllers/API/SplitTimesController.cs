using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using B52TimeMachine.Models;
using B52TimeMachine.Dtos;

namespace B52TimeMachine.Controllers.API
{
    public class SplitTimesController : ApiController
    {
        ApplicationDbContext _context;
        public SplitTimesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //public static int RentalFeeCalculate(double numberOfMinutes, int price)
        //{
        //    double rentalFee = price / 60 * numberOfMinutes;
        //    var rentalFeeTemp = rentalFee / 1000;
        //    rentalFee = Math.Round(rentalFeeTemp, MidpointRounding.AwayFromZero) * 1000;

        //    return Convert.ToInt32(rentalFee);
        //}

        [HttpGet]
        public IHttpActionResult GetSplitTimes(int Id)
        {
            var splitTimes = _context.SplitTimes.Where(p => p.Rental.RentalId == Id)
                                               .Include(p => p.Rental)
                                               .Include(p => p.Rental.Ps)
                                               .ToList();

            var SplitTimeDtos = new List<SplitTimeDto>();
            double rentalFee = 0;
            var rental = _context.Rentals.Include(r => r.Ps).Single(r => r.RentalId == Id);
            //whole rental time, and check if this rental is checkout or not to show modal as view mode or checkout mode
            TimeSpan rentalSpan = (rental.To == null) ? DateTime.Now.Subtract(rental.From) : Convert.ToDateTime(rental.To).Subtract(rental.From); ;
            TimeSpan splitSpan;

            // if there is no splitTimes
            if (splitTimes.Count() == 0)
            {
                rentalFee = CalculateMethods.RentalFeeCalculate(rentalSpan.TotalMinutes, rental.Ps.Price);

                SplitTimeDtos.Add(new SplitTimeDto
                {
                    RentalId = Id,
                    //SplitSpans in this case is whole RentalSpan
                    SplitSpan = string.Format("{0}:{1}", decimal.Truncate(Convert.ToDecimal(rentalSpan.TotalHours)), rentalSpan.Minutes),
                    SplitFee = Convert.ToInt32(rentalFee),
                    SplitSpanRatio = 100
                });
                //return whole rental span and rental fee
                return Ok(SplitTimeDtos);
            }

            for (int i = 0; i <= splitTimes.Count(); i++)
            {
                double splitSpanRatio = 0;
                if (i < splitTimes.Count())
                {
                    var from = (i == 0) ? rental.From : splitTimes[i - 1].TimeSplit;
                    splitSpan = splitTimes[i].TimeSplit.Subtract(from);
                }
                else
                {
                    splitSpan = (rental.To == null) ? DateTime.Now.Subtract(splitTimes[i - 1].TimeSplit) : Convert.ToDateTime(rental.To).Subtract(splitTimes[i - 1].TimeSplit);
                }

                rentalFee = CalculateMethods.RentalFeeCalculate(splitSpan.TotalMinutes, rental.Ps.Price);
                //calculate split span ratio to show on view
                splitSpanRatio = splitSpan.TotalMinutes * 100 / rentalSpan.TotalMinutes;
                
                SplitTimeDtos.Add(new SplitTimeDto {
                    RentalId = Id,
                    SplitSpan = string.Format("{0}:{1}", decimal.Truncate(Convert.ToDecimal(splitSpan.TotalHours)), splitSpan.Minutes),
                    SplitFee = Convert.ToInt32(rentalFee),
                    SplitSpanRatio = splitSpanRatio
                });
            }
            return Ok(SplitTimeDtos);
        }

        [HttpPost]
        public IHttpActionResult CreateTimeSplit(int Id)
        {
            var rental = _context.Rentals.SingleOrDefault(r => r.RentalId == Id);
            if (rental == null)
                return BadRequest("Invalid Rental");

            var lastSplit = _context.SplitTimes.OrderByDescending(s => s.SplitTimeId)
                                              .Where(s => s.Rental.RentalId == Id)
                                              .Take(1)
                                              .SingleOrDefault();
            //prevent spamming and split time when rental time under 15 minutes
            var splitSpan = (lastSplit != null) ? DateTime.Now.Subtract(lastSplit.TimeSplit) : DateTime.Now.Subtract(rental.From);

            if(splitSpan.TotalMinutes < 15)
                return BadRequest("Can't split time when rental time under 15 minutes");

            var splitTime = new SplitTime 
            { 
                Rental = rental,
                TimeSplit = DateTime.Now
            };

            _context.SplitTimes.Add(splitTime);
            _context.SaveChanges();

            return Json(splitTime);
        }
    }
}
