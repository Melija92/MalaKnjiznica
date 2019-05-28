using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MalaKnjizara.Models;
using MalaKnjizara.ViewModel;
using PagedList;

namespace MalaKnjizara.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class NakladnikController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NazivSortParm = String.IsNullOrEmpty(sortOrder) ? "naziv_desc" : "";
            ViewBag.OIBSortParm = sortOrder == "OIB_asc" ? "OIB_desc" : "OIB_asc";
            ViewBag.AdresaSortParm = sortOrder == "adresa_asc" ? "adresa_desc" : "adresa_asc";
            ViewBag.EmailSortParm = sortOrder == "email_asc" ? "email_desc" : "email_asc";
            ViewBag.TelefonSortParm = sortOrder == "telefon_asc" ? "telefon_desc" : "telefon_asc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var nakladnici = db.Nakladnik.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
                nakladnici = nakladnici.Where(s => s.Naziv.Contains(searchString));

            switch (sortOrder)
            {
                case "naziv_desc":
                    nakladnici = nakladnici.OrderByDescending(s => s.Naziv);
                    break;
                case "OIB_desc":
                    nakladnici = nakladnici.OrderByDescending(s => s.OIB);
                    break;
                case "OIB_asc":
                    nakladnici = nakladnici.OrderBy(s => s.OIB);
                    break;
                case "adresa_desc":
                    nakladnici = nakladnici.OrderByDescending(s => s.Adresa);
                    break;
                case "adresa_asc":
                    nakladnici = nakladnici.OrderBy(s => s.Adresa);
                    break;
                case "email_desc":
                    nakladnici = nakladnici.OrderByDescending(s => s.Email);
                    break;
                case "email_asc":
                    nakladnici = nakladnici.OrderBy(s => s.Email);
                    break;
                case "telefon_desc":
                    nakladnici = nakladnici.OrderByDescending(s => s.Telefon);
                    break;
                case "telefon_asc":
                    nakladnici = nakladnici.OrderBy(s => s.Telefon);
                    break;
                default:
                    nakladnici = nakladnici.OrderBy(s => s.Naziv);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(nakladnici.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var detalji = db.Knjiga.Where(a => a.NakladnikID == id)
                                    .Select(a => new KnjigaViewModel
                                    {
                                        Naziv = a.Naziv,
                                        NazivNakladnika = a.Nakladnik.Naziv,
                                        Kolicina = a.Kolicina,
                                        BrojStranica = a.BrojStranica,
                                        JezikPisanja = a.JezikPisanja,
                                        NazivPolice = a.Polica.Oznaka
                                    });

            ViewBag.NazivNakladnika = db.Nakladnik.FirstOrDefault(a => a.NakladnikID == id).Naziv;
            return View(detalji);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NakladnikID,OIB,Naziv,Adresa,Email,Telefon")] Nakladnik nakladnik)
        {
            if (ModelState.IsValid)
            {
                db.Nakladnik.Add(nakladnik);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nakladnik);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nakladnik nakladnik = db.Nakladnik.Find(id);
            if (nakladnik == null)
            {
                return HttpNotFound();
            }
            return View(nakladnik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NakladnikID,OIB,Naziv,Adresa,Email,Telefon")] Nakladnik nakladnik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nakladnik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nakladnik);
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nakladnik nakladnik = db.Nakladnik.Find(id);
            if (nakladnik == null)
            {
                return HttpNotFound();
            }
            return View(nakladnik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nakladnik nakladnik = db.Nakladnik.Find(id);
            db.Nakladnik.Remove(nakladnik);
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
