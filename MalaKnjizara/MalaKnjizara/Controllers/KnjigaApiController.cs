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
using MalaKnjizara.Repositories;

namespace MalaKnjizara.Controllers
{
    public class KnjigaApiController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public KnjigaApiController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public KnjigaApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET SVI
        [HttpGet]
        [Route("api/GetKnjige")]
        public HttpResponseMessage GetKnjige()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _unitOfWork.Knjige.VratiKnjige());
        }

        //GET JEDAN
        [HttpGet]
        [Route("api/GetKnjiga/{id}")]
        public HttpResponseMessage GetKnjiga(int id)
        {
            var knjiga = _unitOfWork.Knjige.VratiJednuKnjigu(id);

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
        public HttpResponseMessage Post(Knjiga knjiga)
        {
            try
            {
                _unitOfWork.Knjige.SpremiKnjigu(knjiga);
                _unitOfWork.Complete();

                //da dobijemo status kod 201 za Rest kovenciju
                var message = Request.CreateResponse(HttpStatusCode.Created, knjiga);
                //message.Headers.Location = new Uri(Request.RequestUri + knjiga.KnjigaID.ToString());
                var uriString = Url.Link("DefaultApi", new { id = knjiga.KnjigaID });
                message.Headers.Location = new Uri(uriString);


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
                var knjiga = _unitOfWork.Knjige.VratiJednuKnjigu(id);
                if (knjiga == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Knjiga s ID-om " + id.ToString() + " nije pronađena");
                }

                _unitOfWork.Knjige.IzbrisiKnjigu(knjiga);
                _unitOfWork.Complete();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


        ////postavljanje PUT
        ////trebamo staviti status code 200
        ////ako nema knjige ne smije biti status 500 internal server error nego 404
        //[Route("api/PutKnjiga/{id}")]
        //public HttpResponseMessage Put(int id, [FromBody]Knjiga knjiga)
        //{
        //    try
        //    {
        //        var dbKnjiga = _unitOfWork.Knjiga.FirstOrDefault(e => e.KnjigaID == id);

        //        if (knjiga == null)
        //            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Knjiga s ID-om " + id.ToString() + " nije pronađena");
        //        else
        //        {
        //            dbKnjiga.NakladnikID = knjiga.NakladnikID;
        //            dbKnjiga.BrojStranica = knjiga.BrojStranica;
        //            dbKnjiga.JezikPisanja = knjiga.JezikPisanja;
        //            dbKnjiga.Naziv = knjiga.Naziv;
        //            dbKnjiga.Kolicina = knjiga.Kolicina;
        //            dbKnjiga.Cijena = knjiga.Cijena;
        //            dbKnjiga.PolicaID = knjiga.PolicaID;

        //            _unitOfWork.SaveChanges();

        //            return Request.CreateResponse(HttpStatusCode.OK, knjiga);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        //    }
        //}
    }
}
