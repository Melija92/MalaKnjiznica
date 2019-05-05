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
    public class KnjigaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "naziv_desc" : "";
            ViewBag.NakladnikSortParm = sortOrder == "nakladnik_asc" ? "nakladnik_desc" : "nakladnik_asc";
            ViewBag.OznakaSortParm = sortOrder == "oznaka_asc" ? "oznaka_desc" : "oznaka_asc";
            ViewBag.KolicinaSortParm = sortOrder == "kolicina_asc" ? "kolicina_desc" : "kolicina_asc";
            ViewBag.BrojStranicaSortParm = sortOrder == "brojStranica_asc" ? "brojStranica_desc" : "brojStranica_asc";
            ViewBag.JezikPisanjaSortParm = sortOrder == "jezikPisanja_asc" ? "jezikPisanja_desc" : "jezikPisanja_asc";
            
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var knjige = db.Knjiga.Include(k => k.Nakladnik).Include(k => k.Polica);

            if (!String.IsNullOrEmpty(searchString))
                knjige = knjige.Where(s => s.Naziv.Contains(searchString) || 
                                           s.Polica.Oznaka.Contains(searchString));

            switch (sortOrder)
            {
                case "naziv_desc":
                    knjige = knjige.OrderByDescending(s => s.Naziv);
                    break;
                case "nakladnik_desc":
                    knjige = knjige.OrderByDescending(s => s.Nakladnik.Naziv);
                    break;
                case "nakladnik_asc":
                    knjige = knjige.OrderBy(s => s.Nakladnik.Naziv);
                    break;
                case "kolicina_desc":
                    knjige = knjige.OrderByDescending(s => s.Kolicina);
                    break;
                case "kolicina_asc":
                    knjige = knjige.OrderBy(s => s.Kolicina);
                    break;
                case "brojStranica_desc":
                    knjige = knjige.OrderByDescending(s => s.BrojStranica);
                    break;
                case "brojStranica_asc":
                    knjige = knjige.OrderBy(s => s.BrojStranica);
                    break;
                case "jezikPisanja_desc":
                    knjige = knjige.OrderByDescending(s => s.JezikPisanja);
                    break;
                case "jezikPisanja_asc":
                    knjige = knjige.OrderBy(s => s.JezikPisanja);
                    break;
                case "oznaka_desc":
                    knjige = knjige.OrderByDescending(s => s.Polica.Oznaka);
                    break;
                case "oznaka_asc":
                    knjige = knjige.OrderBy(s => s.Polica.Oznaka);
                    break;
                default:
                    knjige = knjige.OrderBy(s => s.Naziv);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(knjige.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knjiga knjiga = db.Knjiga.Find(id);
            if (knjiga == null)
            {
                return HttpNotFound();
            }
            return View(knjiga);
        }
        public ActionResult Create()
        {
            ViewBag.NakladnikID = new SelectList(db.Nakladnik, "NakladnikID", "Naziv");
            ViewBag.PolicaID = new SelectList(db.Polica, "PolicaID", "Oznaka");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KnjigaID,Naziv,Kolicina,BrojStranica,JezikPisanja,NakladnikID,PolicaID")] Knjiga knjiga)
        {
            if (ModelState.IsValid)
            {
                db.Knjiga.Add(knjiga);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NakladnikID = new SelectList(db.Nakladnik, "NakladnikID", "Naziv", knjiga.NakladnikID);
            ViewBag.PolicaID = new SelectList(db.Polica, "PolicaID", "Oznaka", knjiga.PolicaID);
            return View(knjiga);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knjiga knjiga = db.Knjiga.Find(id);
            if (knjiga == null)
            {
                return HttpNotFound();
            }
            ViewBag.NakladnikID = new SelectList(db.Nakladnik, "NakladnikID", "Naziv", knjiga.NakladnikID);
            ViewBag.PolicaID = new SelectList(db.Polica, "PolicaID", "Oznaka", knjiga.PolicaID);
            return View(knjiga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KnjigaID,Naziv,Kolicina,BrojStranica,JezikPisanja,NakladnikID,PolicaID")] Knjiga knjiga)
        {
            if (ModelState.IsValid)
            {
                db.Entry(knjiga).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NakladnikID = new SelectList(db.Nakladnik, "NakladnikID", "OIB", knjiga.NakladnikID);
            ViewBag.PolicaID = new SelectList(db.Polica, "PolicaID", "Oznaka", knjiga.PolicaID);
            return View(knjiga);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knjiga knjiga = db.Knjiga.Find(id);
            if (knjiga == null)
            {
                return HttpNotFound();
            }
            return View(knjiga);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Knjiga knjiga = db.Knjiga.Find(id);
            db.Knjiga.Remove(knjiga);
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
