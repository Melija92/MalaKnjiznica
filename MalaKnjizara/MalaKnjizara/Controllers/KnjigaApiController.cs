using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using MalaKnjizara.Models;

namespace MalaKnjizara.Controllers
{
    //[EnableCorsAttribute("http://localhost:58090", "*", "*")]
    public class KnjigaApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET SVI
        [HttpGet]
        [Route("api/GetKnjige")]
        public HttpResponseMessage GetKnjige()
        {
            return Request.CreateResponse(HttpStatusCode.OK, db.Knjiga.ToList());
        }

        //GET JEDAN
        [HttpGet]
        [Route("api/GetKnjiga/{id}")]
        public HttpResponseMessage GetKnjiga(int id)
        {
            var knjiga = db.Knjiga.FirstOrDefault(e => e.KnjigaID == id);

            if (knjiga != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, knjiga);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Knjiga s ID-om = " + id.ToString() + " nije pronadena");
            }
        }

        //POST
        [Route("api/PostKnjiga")]
        public HttpResponseMessage Post([FromBody]Knjiga knjiga)
        {
            try
            {
                db.Knjiga.Add(knjiga);
                db.SaveChanges();

                //da dobijemo status kod 201 za Rest kovenciju
                var message = Request.CreateResponse(HttpStatusCode.Created, knjiga);
                message.Headers.Location = new Uri(Request.RequestUri + knjiga.KnjigaID.ToString());

                return message;
                
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //DELETE
        [Route("api/DeleteKnjiga/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var knjiga = db.Knjiga.FirstOrDefault(e => e.KnjigaID == id);
                if (knjiga == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Employee with Id = " + id.ToString() + " not found to delete");
                }

                db.Knjiga.Remove(knjiga);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


        //postavljanje PUT
        //trebamo staviti status code 200
        //ako nema knjige ne smije biti status 500 internal server error nego 404
        [Route("api/PutKnjiga/{id}")]
        public HttpResponseMessage Put(int id, [FromBody]Knjiga knjiga)
        {
            try
            {
                var dbKnjiga = db.Knjiga.FirstOrDefault(e => e.KnjigaID == id);

                if (knjiga == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Knjiga s ID-om " + id.ToString() + " nije pronađena");
                else
                {
                    dbKnjiga.NakladnikID = knjiga.NakladnikID;
                    dbKnjiga.BrojStranica = knjiga.BrojStranica;
                    dbKnjiga.JezikPisanja = knjiga.JezikPisanja;
                    dbKnjiga.Naziv = knjiga.Naziv;
                    dbKnjiga.Kolicina = knjiga.Kolicina;
                    dbKnjiga.PolicaID = knjiga.PolicaID;

                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, knjiga);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
