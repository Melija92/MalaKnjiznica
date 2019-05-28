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
    public class AutorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "User, Admin, SuperAdmin")]
        // GET: Autor
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ImeSortParm = String.IsNullOrEmpty(sortOrder) ? "ime_desc" : "";
            ViewBag.PrezimeSortParm = sortOrder == "prezime_asc" ? "prezime_desc" : "prezime_asc";
            ViewBag.OIBSortParm =  sortOrder == "oib_asc" ? "oib_desc" : "oib_asc";
            
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;
            var autori = db.Autor.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
                autori = autori.Where(s => s.Ime.Contains(searchString) || s.Prezime.Contains(searchString));

            switch (sortOrder)
            {
                case "ime_desc":
                    autori = autori.OrderByDescending(s => s.Ime);
                    break;
                case "prezime_desc":
                    autori = autori.OrderByDescending(s => s.Prezime);
                    break;
                case "prezime_asc":
                    autori = autori.OrderBy(s => s.Prezime);
                    break;
                case "oib_desc":
                    autori = autori.OrderByDescending(s => s.OIB);
                    break;
                case "oib_asc":
                    autori = autori.OrderBy(s => s.OIB);
                    break;
                default:
                    autori = autori.OrderBy(s => s.Ime);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(autori.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var knjigeID = db.Autorstvo
                .Where(a => a.AutorID == id)
                .Select(a => a.KnjigaID)
                .ToList();

            List<KnjigaViewModel> knjige = new List<KnjigaViewModel>();
            foreach (var knjigaID in knjigeID)
            {
                var knjiga = db.Knjiga.Find(knjigaID);

                KnjigaViewModel knjigaViewModel = new KnjigaViewModel();
                knjigaViewModel.Naziv = knjiga.Naziv;
                knjigaViewModel.NazivNakladnika = knjiga.Nakladnik.Naziv;
                knjigaViewModel.BrojStranica = knjiga.BrojStranica;
                knjigaViewModel.Kolicina = knjiga.Kolicina;
                knjigaViewModel.JezikPisanja = knjiga.JezikPisanja;

                knjige.Add(knjigaViewModel);
            }
            ViewBag.NazivAutora = db.Autor.FirstOrDefault(a => a.AutorID == id).Ime
                + " " + db.Autor.FirstOrDefault(a => a.AutorID == id).Prezime;
            return View(knjige);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AutorID,OIB,Ime,Prezime")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                db.Autor.Add(autor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(autor);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autor autor = db.Autor.Find(id);
            if (autor == null)
            {
                return HttpNotFound();
            }
            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AutorID,OIB,Ime,Prezime")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(autor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(autor);
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autor autor = db.Autor.Find(id);
            if (autor == null)
            {
                return HttpNotFound();
            }
            return View(autor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Autor autor = db.Autor.Find(id);
            db.Autor.Remove(autor);
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
