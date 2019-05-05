using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MalaKnjizara.Models;
using PagedList;

namespace MalaKnjizara.Controllers
{
    public class AutorstvoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Autorstvo
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "naziv_desc" : "";
            ViewBag.OIBSortParm = sortOrder == "OIB_asc" ? "OIB_desc" : "OIB_asc";
            ViewBag.UdioAutorstvaSortParm = sortOrder == "udioAutorstva_asc" ? "udioAutorstva_desc" : "udioAutorstva_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var autorstva = db.Autorstvo.Include(k => k.Autor).Include(k => k.Knjiga);

            if (!String.IsNullOrEmpty(searchString))
                autorstva = autorstva.Where(s => s.Autor.OIB.Contains(searchString) ||
                                           s.Knjiga.Naziv.Contains(searchString));

            switch (sortOrder)
            {
                case "naziv_desc":
                    autorstva = autorstva.OrderByDescending(s => s.Knjiga.Naziv);
                    break;
                case "OIB_desc":
                    autorstva = autorstva.OrderByDescending(s => s.Autor.OIB);
                    break;
                case "OIB_asc":
                    autorstva = autorstva.OrderBy(s => s.Autor.OIB);
                    break;
                case "udioAutorstva_desc":
                    autorstva = autorstva.OrderByDescending(s => s.UdioAutorstva);
                    break;
                case "udioAutorstva_asc":
                    autorstva = autorstva.OrderBy(s => s.UdioAutorstva);
                    break;
                default:
                    autorstva = autorstva.OrderBy(s => s.Knjiga.Naziv);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(autorstva.ToPagedList(pageNumber, pageSize));
        }

        // GET: Autorstvo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autorstvo autorstvo = db.Autorstvo.Find(id);
            if (autorstvo == null)
            {
                return HttpNotFound();
            }
            return View(autorstvo);
        }

        // GET: Autorstvo/Create
        public ActionResult Create()
        {
            ViewBag.AutorID = new SelectList(db.Autor, "AutorID", "OIB");
            ViewBag.KnjigaID = new SelectList(db.Knjiga, "KnjigaID", "Naziv");
            return View();
        }

        // POST: Autorstvo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AutorstvoID,KnjigaID,AutorID,UdioAutorstva")] Autorstvo autorstvo)
        {
            if (ModelState.IsValid)
            {
                db.Autorstvo.Add(autorstvo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AutorID = new SelectList(db.Autor, "AutorID", "OIB", autorstvo.AutorID);
            ViewBag.KnjigaID = new SelectList(db.Knjiga, "KnjigaID", "Naziv", autorstvo.KnjigaID);
            return View(autorstvo);
        }

        // GET: Autorstvo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autorstvo autorstvo = db.Autorstvo.Find(id);
            if (autorstvo == null)
            {
                return HttpNotFound();
            }
            ViewBag.AutorID = new SelectList(db.Autor, "AutorID", "OIB", autorstvo.AutorID);
            ViewBag.KnjigaID = new SelectList(db.Knjiga, "KnjigaID", "Naziv", autorstvo.KnjigaID);
            return View(autorstvo);
        }

        // POST: Autorstvo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AutorstvoID,KnjigaID,AutorID,UdioAutorstva")] Autorstvo autorstvo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(autorstvo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AutorID = new SelectList(db.Autor, "AutorID", "OIB", autorstvo.AutorID);
            ViewBag.KnjigaID = new SelectList(db.Knjiga, "KnjigaID", "Naziv", autorstvo.KnjigaID);
            return View(autorstvo);
        }

        // GET: Autorstvo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autorstvo autorstvo = db.Autorstvo.Find(id);
            if (autorstvo == null)
            {
                return HttpNotFound();
            }
            return View(autorstvo);
        }

        // POST: Autorstvo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Autorstvo autorstvo = db.Autorstvo.Find(id);
            db.Autorstvo.Remove(autorstvo);
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
