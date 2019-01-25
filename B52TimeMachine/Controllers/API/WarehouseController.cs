using B52TimeMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using B52TimeMachine.Dtos;

namespace B52TimeMachine.Controllers.API
{
    public class WarehouseController : ApiController
    {
        ApplicationDbContext _context;
        public WarehouseController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        public IHttpActionResult GetWarehouse()
        {
            var warehouses = _context.Warehouses.Include(s => s.Service).Where(s => s.Service.IsVisible).ToList();

            return Ok(warehouses);
        }

        [HttpGet]
        public IHttpActionResult GetRestockServiceWithRestockId(int Id)
        {
            var restockServices = _context.RestockServices.Include(r => r.Restock).Where(r => r.Restock.RestockId == Id).ToList();

            if (restockServices.Count < 1)
                return BadRequest("Restock not found");

            var restockServiceDtos = new List<RestockServiceDto>();
            foreach (var item in restockServices)
            {
                restockServiceDtos.Add(new RestockServiceDto {
                    RestockServiceId = item.RestockServiceId,
                    RestockDate = item.Restock.RestockDate.ToString("dd/MM/yyyy"),
                    ServiceId = item.ServiceId,
                    Quantity = item.Quantity,
                    Total = item.Total
                });
            }

            return Ok(restockServiceDtos);
        }

        [HttpGet]
        [Route("Api/Warehouse/GetRestock/{month?}")]
        public IHttpActionResult GetRestock(int? month)
        {
            month = (month == null) ? DateTime.Now.Month : month;
            var restocks = _context.Restocks.Where(r => r.RestockDate.Month == month).ToList();

            if (restocks.Count < 1)
                return BadRequest("No restock in this month");

            List<RestockDto> restockDtos = new List<RestockDto>();
            foreach (var item in restocks)
            {
                restockDtos.Add(new RestockDto {
                    RestockId = item.RestockId,
                    RestockDate = item.RestockDate.ToString("dd MMM yyyy HH:mm"),
                    Total = item.Total.ToString("n0")
                });
            }
            return Ok(restockDtos);
        }

        [HttpPost]
        public IHttpActionResult CreateRestock(List<RestockServiceDto> restockServiceDtos)
        {
            var restock = new Restock {
                RestockDate = DateTime.Now,
                Total = restockServiceDtos.Where(r => r.Quantity > 0).Sum(r => r.Total)
            };

            _context.Restocks.Add(restock);

            foreach (var item in restockServiceDtos)
            {
                if (item.Quantity > 0)
                {
                    var restockService = new RestockService
                    {
                        Restock = restock,
                        ServiceId = item.ServiceId,
                        Quantity = item.Quantity,
                        Total = item.Total
                    };
                    _context.RestockServices.Add(restockService);

                    var warehouseInDb = _context.Warehouses.SingleOrDefault(w => w.Service.ServiceId == item.ServiceId);
                    if (warehouseInDb == null)
                    {
                        var warehouse = new Warehouse {
                            Service = _context.Services.Single(s => s.ServiceId == item.ServiceId),
                            Quantity = item.Quantity
                        };
                        _context.Warehouses.Add(warehouse);
                    }
                    else
                    {
                        warehouseInDb.Quantity += item.Quantity;
                    }
                }
            }

            _context.SaveChanges();

            var restockDto = new RestockDto {
                RestockId = restock.RestockId,
                RestockDate = restock.RestockDate.ToString("dd MMM yyyy HH:mm"),
                Total = restock.Total.ToString("n0")
            };
            return Json(restockDto);
        }

        [HttpDelete]
        public IHttpActionResult DeleteRestock(int Id)
        {
            var restock = _context.Restocks.SingleOrDefault(r => r.RestockId == Id);

            if (restock == null)
                return BadRequest("Invalid restock");

            var restockServices = _context.RestockServices.Where(r => r.Restock.RestockId == Id).ToList();
            var warehouses = _context.Warehouses.Include(w => w.Service).ToList();

            bool flag = true;
            foreach (var item in restockServices)
            {
                if (item.Quantity > warehouses.Single(s => s.Service.ServiceId == item.ServiceId).Quantity)
                    flag = false;
            }

            if (flag)
            {
                foreach (var item in restockServices)
                {
                    //minus warehouse quantity
                    warehouses.Single(w => w.Service.ServiceId == item.ServiceId).Quantity -= item.Quantity;
                    //remove restock services
                    _context.RestockServices.Remove(item);
                }
                _context.Restocks.Remove(restock);
            }
            else
            {
                return BadRequest("Not enough quantity in warehouse to delete this restock form");
            }
            _context.SaveChanges();
            return Ok();
        }
    }
}
