using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;

namespace JobPortal.Controllers
{
    public class JobNatureTablesController : Controller
    {
        private JobshuntDbEntities db = new JobshuntDbEntities();

        // GET: JobNatureTables
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            return View(db.JobNatureTables.ToList());
        }

        // GET: JobNatureTables/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            return View(new JobNatureTable());
        }

        // POST: JobNatureTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobNatureID,JobNature")] JobNatureTable jobNatureTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                db.JobNatureTables.Add(jobNatureTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobNatureTable);
        }

        // GET: JobNatureTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobNatureTable jobNatureTable = db.JobNatureTables.Find(id);
            if (jobNatureTable == null)
            {
                return HttpNotFound();
            }
            return View(jobNatureTable);
        }

        // POST: JobNatureTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobNatureID,JobNature")] JobNatureTable jobNatureTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                db.Entry(jobNatureTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobNatureTable);
        }

      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
