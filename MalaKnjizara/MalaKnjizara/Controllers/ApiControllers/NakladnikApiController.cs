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

namespace MalaKnjizara.Controllers.ApiControllers
{
    public class NakladnikApiController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public NakladnikApiController ()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public NakladnikApiController (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET SVI
        [HttpGet]
        [Route("api/GetNakladnike")]
        public HttpResponseMessage GetNakladnike()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _unitOfWork.Nakladnici.VratiNakladnike());
        }

        //GET JEDAN
        [HttpGet]
        [Route("api/GetNakladnika/{id}")]
        public HttpResponseMessage GetNakladnik(int id)
        {
            var nakladnik = _unitOfWork.Nakladnici.VratiJednogNakladnika(id);

            if (nakladnik != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, nakladnik);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Nakladnik s ID-om = " + id.ToString() + " nije pronaden");
            }
        }

        //POST
        [Route("api/PostNakladnik")]
        public HttpResponseMessage Post(Nakladnik nakladnik)
        {
            try
            {
                _unitOfWork.Nakladnici.SpremiNakladnika(nakladnik);
                _unitOfWork.Complete();

                //da dobijemo status kod 201 za Rest kovenciju
                var message = Request.CreateResponse(HttpStatusCode.Created, nakladnik);
                var uriString = Request.RequestUri.ToString().Replace("PostNakladnik", "GetNakladnik/") + nakladnik.NakladnikID.ToString();
                message.Headers.Location = new Uri(uriString);

                return message;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //DELETE
        [Route("api/DeleteNakladnik/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var nakladnik = _unitOfWork.Nakladnici.VratiJednogNakladnika(id);
                if (nakladnik == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Nakladnik s ID-om " + id.ToString() + " nije pronaden");
                }

                _unitOfWork.Nakladnici.IzbrisiNakladnika(nakladnik);
                _unitOfWork.Complete();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}