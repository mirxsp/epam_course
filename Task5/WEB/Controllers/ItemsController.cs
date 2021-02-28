using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WEB.DAL.Contexts;
using WEB.DAL.Units;
using WEB.Models;

namespace WEB.Controllers
{
    public class ItemsController : Controller
    {
        private UnitOfWork unit = new UnitOfWork();

        // GET: Items
        [Authorize]
        public ActionResult Index()
        {

            return View();
            //return View(items.ToList());
        }

        [HttpPost]
        public ActionResult GetItems(string filterType = "all", string filterValue = "")
        {
            object resObj = null;
            if (filterType == "all")
            {
                resObj = unit.ItemRepository.Get().ToList<Item>();
            }
            else if (filterType == "name")
            {
                resObj = unit.ItemRepository.Get(x => x.Name.Contains(filterValue)).ToList<Item>();
            }
            else if (filterType == "id")
            {
                resObj = unit.ItemRepository.Get(x => x.Id.ToString().Equals(filterValue)).ToList<Item>();
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

        // GET: Items/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                unit.ItemRepository.Add(item);
                unit.Save();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = unit.ItemRepository.Get(x => x.Id == id).FirstOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,RowVersion")] Item item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unit.ItemRepository.Update(item);
                    unit.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Item)entry.Entity;
                var databaseValues = (Item)entry.GetDatabaseValues().ToObject();
                if (databaseValues.Name != clientValues.Name)
                {
                    ModelState.AddModelError("Name", "Current value: "
                        + databaseValues.Name);
                }
                if (databaseValues.Price != clientValues.Price)
                {
                    ModelState.AddModelError("Price", "Current value: "
                        + databaseValues.Price);
                }
                ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                    + "was modified by another user after you got the original value. The "
                    + "edit operation was canceled and the current values in the database "
                    + "have been displayed. If you still want to edit this record, click "
                    + "the Save button again. Otherwise click the Back to List hyperlink.");
                item.RowVersion = databaseValues.RowVersion;
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, bool? concurrencyError)
        {
            Item item = unit.ItemRepository.Get(x => x.Id == id).FirstOrDefault();
            if (concurrencyError.GetValueOrDefault())
            {
                if (item == null)
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
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Item item)
        {
            try
            {
                //sale = unit.SaleRepository.Get(x => x.Id == id).FirstOrDefault();
                unit.ItemRepository.Remove(item);
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
                return View(item);
            }
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
