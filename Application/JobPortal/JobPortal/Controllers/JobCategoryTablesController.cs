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
    public class JobCategoryTablesController : Controller
    {
        private JobshuntDbEntities db = new JobshuntDbEntities();

        // GET: JobCategoryTables
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            return View(db.JobCategoryTables.ToList());
        }

        // GET: JobCategoryTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCategoryTable jobCategoryTable = db.JobCategoryTables.Find(id);
            if (jobCategoryTable == null)
            {
                return HttpNotFound();
            }
            return View(jobCategoryTable);
        }

        // GET: JobCategoryTables/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        // POST: JobCategoryTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobCategoryID,JobCategory,Description")] JobCategoryTable jobCategoryTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                db.JobCategoryTables.Add(jobCategoryTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobCategoryTable);
        }

        // GET: JobCategoryTables/Edit/5
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
            JobCategoryTable jobCategoryTable = db.JobCategoryTables.Find(id);
            if (jobCategoryTable == null)
            {
                return HttpNotFound();
            }
            return View(jobCategoryTable);
        }

        // POST: JobCategoryTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobCategoryID,JobCategory,Description")] JobCategoryTable jobCategoryTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                db.Entry(jobCategoryTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobCategoryTable);
        }

        // GET: JobCategoryTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCategoryTable jobCategoryTable = db.JobCategoryTables.Find(id);
            if (jobCategoryTable == null)
            {
                return HttpNotFound();
            }
            return View(jobCategoryTable);
        }

        // POST: JobCategoryTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobCategoryTable jobCategoryTable = db.JobCategoryTables.Find(id);
            db.JobCategoryTables.Remove(jobCategoryTable);
            db.SaveChanges();
            return RedirectToAction("Index");
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
