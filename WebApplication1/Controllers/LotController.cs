using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Infrastructure;

namespace WebApplication1.Controllers
{
    public class LotController : Controller
    {

        private readonly AppDbContext _context;

        public LotController(AppDbContext context)
        {
            _context = context;
        }

        private DbSet<Lot> Lots => _context.Lots;
        private DbSet<Bid> Bids => _context.Bids;
        private DbSet<Comment> Comments => _context.Comments;
        public IActionResult Index(string searchString, string sortOrder, int? minPrice, int? maxPrice)
        {
            var lots = Lots.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                lots = lots.Where(s => s.Description.Contains(searchString));
            }

            if (minPrice.HasValue)
            {
                lots = lots.Where(l => l.MinPrice >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                lots = lots.Where(l => l.MinPrice <= maxPrice);
            }

            switch (sortOrder)
            {
                case "price_asc":
                    lots = lots.OrderBy(l => l.MinPrice);
                    break;
                case "price_desc":
                    lots = lots.OrderByDescending(l => l.MinPrice);
                    break;
                case "date_asc":
                    lots = lots.OrderBy(l => l.CreatedDate);
                    break;
                case "date_desc":
                    lots = lots.OrderByDescending(l => l.CreatedDate);
                    break;
                default:
                    lots = lots.OrderBy(l => l.Id);
                    break;
            }

            return View(lots.ToList());
        }
        public IActionResult AddLot()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddLot(Lot lot)
        {
            if (ModelState.IsValid)
            {
                lot.CreatedDate = DateTime.Now;
                Lots.Add(lot);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(lot);
            }
        }
        public IActionResult LotDetails(int id)
        {
            var lot = Lots.FirstOrDefault(l => l.Id == id);
            if (lot == null)
            {
                return NotFound();
            }
            var lotBids = Bids.Where(b => b.LotId == id).ToList();
            var lotComments = Comments.Where(c => c.LotId == id).ToList();
            ViewBag.LotBids = lotBids;
            ViewBag.LotComments = lotComments;
            return View(lot);
        }
        [HttpPost]
        public IActionResult PlaceBid(int lotId, decimal amount)
        {
            var lot = Lots.FirstOrDefault(l => l.Id == lotId);
            if (lot == null || amount <= lot.CurrentBid)
            {
                return RedirectToAction("LotDetails", new { id = lotId });
            }
            var bid = new Bid
            {
                LotId = lotId,
                UserId = 4,
                Amount = amount,
                BidTime = DateTime.Now
            };

            Bids.Add(bid);
            lot.CurrentBid = amount;
            _context.SaveChanges();

            return RedirectToAction("LotDetails", new { id = lotId });
        }
        [HttpPost]
        public IActionResult AddComment(int lotId, string content)
        {
            var lot = Lots.FirstOrDefault(l => l.Id == lotId);
            if (lot == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                LotId = lotId,
                UserId = 4,
                Content = content,
                CreatedDate = DateTime.Now
            };

            Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("LotDetails", new { id = lotId });
        }
        public IActionResult ExportToExcel()
        {
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Лоти");
                worksheet.Cells.LoadFromCollection(Lots.Select(x => new {x.Id, x.Description, x.MinPrice, x.BuyoutPrice, x.CurrentBid, Date=x.CreatedDate.ToString("yyyy_MM_dd") }), true, TableStyles.Medium9);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Lots-{DateTime.Now.ToString("yyyyMMddHH")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
