using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WEB.DAL.Contexts;
using WEB.DAL.Units;
using WEB.Models;

namespace WEB.Controllers
{
    public class SalesController : Controller
    {
        private UnitOfWork unit = new UnitOfWork();

        // GET: Sales
        [Authorize]
        public ActionResult Index()
        {

            return View();
            //return View(sales.ToList());
        }

        [HttpPost]
        public ActionResult GetSales(string filterType = "all",string filterValue = "")
        {
            object resObj = null;
            if(filterType=="all")
            {
                resObj = unit.SaleRepository.Get().ToList<Sale>();
            }
            else if (filterType == "client_name")
            {
                resObj = unit.SaleRepository.Get(x => x.Client.Name.Contains(filterValue)).ToList<Sale>();
            }
            else if (filterType == "manager_name")
            {
                resObj = unit.SaleRepository.Get(x => x.Manager.Name.Contains(filterValue)).ToList<Sale>();
            }
            else if (filterType == "item_name")
            {
                resObj = unit.SaleRepository.Get(x => x.Item.Name.Contains(filterValue)).ToList<Sale>();
            }
            string result = "No elements";
            if (resObj != null)
            {
                result = JsonConvert.SerializeObject(resObj,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
            }
            return Content(result,"application/json");
        }

        [Authorize]
        public ActionResult Search()
        {
            return View();
        }

        // GET: Sales/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            ViewBag.Client_Id = new SelectList(unit.ClientRepository.Get(), "Id", "Name");
            ViewBag.Item_Id = new SelectList(unit.ItemRepository.Get(), "Id", "Name");
            ViewBag.Manager_Id = new SelectList(unit.ManagerRepository.Get(), "Id", "Name");
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Date,Client_Id,Item_Id,Manager_Id")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                unit.SaleRepository.Add(sale);
                unit.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Client_Id = new SelectList(unit.ClientRepository.Get(), "Id", "Name");
            ViewBag.Item_Id = new SelectList(unit.ItemRepository.Get(), "Id", "Name");
            ViewBag.Manager_Id = new SelectList(unit.ManagerRepository.Get(), "Id", "Name");
            return View(sale);
        }

        // GET: Sales/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = unit.SaleRepository.Get(x=>x.Id==id).FirstOrDefault();
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.Client_Id = new SelectList(unit.ClientRepository.Get(), "Id", "Name");
            ViewBag.Item_Id = new SelectList(unit.ItemRepository.Get(), "Id", "Name");
            ViewBag.Manager_Id = new SelectList(unit.ManagerRepository.Get(), "Id", "Name");
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Date,Client_Id,Item_Id,Manager_Id,RowVersion")] Sale sale)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unit.SaleRepository.Update(sale);
                    unit.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Sale)entry.Entity;
                var databaseValues = (Sale)entry.GetDatabaseValues().ToObject();
                if (databaseValues.Client_Id != clientValues.Client_Id)
                {
                    ModelState.AddModelError("Client_Id", "Current value: "
                        + unit.ClientRepository.Get(x => x.Id == databaseValues.Client_Id).FirstOrDefault().Name);
                }
                if (databaseValues.Manager_Id != clientValues.Manager_Id)
                {
                    ModelState.AddModelError("Manager_Id", "Current value: "
                        + unit.ManagerRepository.Get(x => x.Id == databaseValues.Manager_Id).FirstOrDefault().Name);
                }
                if (databaseValues.Date != clientValues.Date)
                {
                    ModelState.AddModelError("Date", "Current value: "
                        + String.Format("{0:d}", databaseValues.Date));
                }
                if (databaseValues.Item_Id != clientValues.Item_Id)
                {
                    ModelState.AddModelError("Item_Id", "Current value: "
                        + unit.ItemRepository.Get(x => x.Id == databaseValues.Item_Id).FirstOrDefault().Name);
                }
                ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, click "
                    + "the Save button again. Otherwise click the Back to List hyperlink.");
                sale.RowVersion = databaseValues.RowVersion;
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            ViewBag.Client_Id = new SelectList(unit.ClientRepository.Get(), "Id", "Name");
            ViewBag.Item_Id = new SelectList(unit.ItemRepository.Get(), "Id", "Name");
            ViewBag.Manager_Id = new SelectList(unit.ManagerRepository.Get(), "Id", "Name");
            return View(sale);
        }

        // GET: Sales/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            Sale sale = unit.SaleRepository.Get(x=>x.Id==id).FirstOrDefault();
            if (concurrencyError.GetValueOrDefault())
            {
                if (sale == null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                        + "was deleted by another user after you got the original values. "
                        + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                        + "was modified by another user after you got the original values. "
                        + "The delete operation was canceled and the current values in the "
                        + "database have been displayed. If you still want to delete this "
                        + "record, click the Delete button again. Otherwise "
                        + "click the Back to List hyperlink.";
                }
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Sale sale)
        {
            try
            {
                //sale = unit.SaleRepository.Get(x => x.Id == id).FirstOrDefault();
                unit.SaleRepository.Remove(sale);
                unit.Save();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true });
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(sale);
            }
        }

        public ActionResult GetChart()
        {
            var sales = unit.SaleRepository.Get().ToList<Sale>();
            List<int> salesYears = sales.Select(x => x.Date.Year).ToList();
            List<int> years = new List<int>();
            List<int> salesCount = new List<int>();
            for(int i = salesYears.Min();i<=salesYears.Max();i++)
            {
                years.Add(i);
                salesCount.Add(sales.Where(x => x.Date.Year == i).Count());
            }
            byte[] chart = new Chart(600, 300,T5ChartTheme.Vanilla3D)
                .AddSeries(
                    name: "Year",
                    xValue: years,
                    yValues: salesCount)
                .GetBytes();
            return File(chart, "image/png");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unit.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
