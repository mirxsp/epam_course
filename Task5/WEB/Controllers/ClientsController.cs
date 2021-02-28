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
    public class ClientsController : Controller
    {
        private UnitOfWork unit = new UnitOfWork();

        // GET: Clients
        [Authorize]
        public ActionResult Index()
        {

            return View();
            //return View(clients.ToList());
        }

        [HttpPost]
        public ActionResult GetClients(string filterType = "all", string filterValue = "")
        {
            object resObj = null;
            if (filterType == "all")
            {
                resObj = unit.ClientRepository.Get().ToList<Client>();
            }
            else if (filterType == "id")
            {
                resObj = unit.ClientRepository.Get(x => x.Id.ToString().Equals(filterValue)).ToList<Client>();
            }
            else if (filterType == "name")
            {
                resObj = unit.ClientRepository.Get(x => x.Name.Contains(filterValue)).ToList<Client>();
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

        // GET: Clients/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name")] Client client)
        {
            if (ModelState.IsValid)
            {
                unit.ClientRepository.Add(client);
                unit.Save();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = unit.ClientRepository.Get(x => x.Id == id).FirstOrDefault();
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,RowVersion")] Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unit.ClientRepository.Update(client);
                    unit.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Client)entry.Entity;
                var databaseValues = (Client)entry.GetDatabaseValues().ToObject();
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
                client.RowVersion = databaseValues.RowVersion;
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, bool? concurrencyError)
        {
            Client client = unit.ClientRepository.Get(x => x.Id == id).FirstOrDefault();
            if (concurrencyError.GetValueOrDefault())
            {
                if (client == null)
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
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(Client client)
        {
            try
            {
                //sale = unit.SaleRepository.Get(x => x.Id == id).FirstOrDefault();
                unit.ClientRepository.Remove(client);
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
                return View(client);
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
