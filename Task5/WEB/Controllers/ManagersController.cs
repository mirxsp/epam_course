using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WEB.DAL.Contexts;
using WEB.DAL.Units;
using WEB.Models;

namespace WEB.Controllers
{
    public class ManagersController : Controller
    {
        private UnitOfWork unit = new UnitOfWork();

        // GET: Managers
	[Authorize]
        public ActionResult Index()
        {

            return View();
            //return View(managers.ToList());
        }

        [HttpPost]
	[Authorize]
        public ActionResult GetManagers(string filterType = "all", string filterValue = "")
        {
            object resObj = null;
            if (filterType == "all")
            {
                resObj = unit.ManagerRepository.Get().ToList<Manager>();
            }
            else if (filterType == "id")
            {
                resObj = unit.ManagerRepository.Get(x => x.Id.ToString().Equals(filterValue)).ToList<Manager>();
            }
            else if (filterType == "name")
            {
                resObj = unit.ManagerRepository.Get(x => x.Name.Contains(filterValue)).ToList<Manager>();
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
            return Content(result, "application/json");
        }

	[Authorize]
        public ActionResult Search()
        {
            return View();
        }

        // GET: Managers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                unit.ManagerRepository.Add(manager);
                unit.Save();
                return RedirectToAction("Index");
            }
            return View(manager);
        }

        // GET: Managers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = unit.ManagerRepository.Get(x => x.Id == id).FirstOrDefault();
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,RowVersion")] Manager manager)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unit.ManagerRepository.Update(manager);
                    unit.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Manager)entry.Entity;
                var databaseValues = (Manager)entry.GetDatabaseValues().ToObject();
                if (databaseValues.Name != clientValues.Name)
                {
                    ModelState.AddModelError("Name", "Current value: "
                        + databaseValues.Name);
                }
                ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, click "
                    + "the Save button again. Otherwise click the Back to List hyperlink.");
                manager.RowVersion = databaseValues.RowVersion;
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(manager);
        }

        // GET: Managers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, bool? concurrencyError)
        {
            Manager manager = unit.ManagerRepository.Get(x => x.Id == id).FirstOrDefault();
            if (concurrencyError.GetValueOrDefault())
            {
                if (manager == null)
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
            return View(manager);
        }

        // POST: Managers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Manager manager)
        {
            try
            {
                //sale = unit.SaleRepository.Get(x => x.Id == id).FirstOrDefault();
                unit.ManagerRepository.Remove(manager);
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
                return View(manager);
            }
        }

	[Authorize]
        public ActionResult GetChart()
        {
            var managers = unit.ManagerRepository.Get().ToList<Manager>();
            List<int> salesCount = managers.Select(x => x.Sales.Count).ToList();
            List<string> managersNames = managers.Select(x => x.Name).ToList();
            byte[] chart = new Chart(600, 300,T5ChartTheme.Vanilla)
                .AddSeries(
                    name: "Manager",
                    xValue: managersNames,
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
