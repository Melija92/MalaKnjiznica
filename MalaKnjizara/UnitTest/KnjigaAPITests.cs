using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using MalaKnjizara;
using MalaKnjizara.Controllers;
using MalaKnjizara.Models;
using MalaKnjizara.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class KnjigaAPITests
    {
        private KnjigaApiController _controller;
        private Mock<IKnjigaRepository> _mockRepository;
        private Knjiga knjiga;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IKnjigaRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.Knjige).Returns(_mockRepository.Object);

            _controller = new KnjigaApiController(mockUoW.Object);

            knjiga = new Knjiga()
            {
                KnjigaID = 99,
                Naziv = "test knjiga",
                Kolicina = 100,
                BrojStranica = 200,
                Cijena = 100,
                JezikPisanja = "hrvatski"
            };
        }

        [TestMethod]
        public
        void TestiranjeDohvataJedneKnjige()
        {
            _mockRepository.Setup(r => r.VratiJednuKnjigu(99)).Returns(knjiga);
            _controller.Request = new HttpRequestMessage() { Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } } };

            var rezultat = _controller.GetKnjiga(99);

            Assert.AreEqual(HttpStatusCode.OK, rezultat.StatusCode);
            Assert.AreEqual(99, rezultat.Content.ReadAsAsync<Knjiga>().Result.KnjigaID);
            Assert.AreEqual("test knjiga", rezultat.Content.ReadAsAsync<Knjiga>().Result.Naziv);
        }
        [TestMethod]
        public void TestiranjeKrivogDohvataKnjige()
        {
            _mockRepository.Setup(r => r.VratiJednuKnjigu(99)).Returns(knjiga);
            _controller.Request = new HttpRequestMessage() { Properties = { { HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration() } } };
            var rezultat = _controller.GetKnjiga(50);

            Assert.AreEqual(HttpStatusCode.NotFound, rezultat.StatusCode, "");
        }

        [TestMethod]
        public void DodavanjeNoveKnjige()
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"], new HttpRouteValueDictionary { { "controller", "KnjigaApi" } });

            _controller.Request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44314/api/PostKnjiga/")
            {
                Properties = { { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration }, { HttpPropertyKeys.HttpRouteDataKey, httpRouteData } }
            };

            var rezultat = _controller.Post(knjiga);

            Assert.AreEqual(HttpStatusCode.Created, rezultat.StatusCode);
            Assert.AreEqual(string.Format("https://localhost:44314/api/KnjigaApi/{0}", knjiga.KnjigaID), rezultat.Headers.Location.ToString());
        }
    }
}
