﻿using DynamicsAdapter.Web.Services.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DynamicsAdapter.Web.Configuration;
using DynamicsAdapter.Web.Services.Dynamics.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using EndPoint = DynamicsAdapter.Web.Configuration.EndPoint;

namespace DynamicsAdapter.Web.Test.Services.Dynamics
{
    public class DynamicsServiceTest
    {
        private Mock<IDynamicService<SSG_SearchRequests>> sut;
        private AppSettings settings;
        private string entity = "Search";
        private string filter = "$filter=statuscode eq 867670000";
        [SetUp]
        public void Setup ()
        {
            settings = new AppSettings
            {
                DynamicsAPI = new DynamicsAPIConfig
                {
                    EndPoints = new List<EndPoint>
                    {
                        new EndPoint { Entity = "Search", URL = "http-search"}
                    }
                }
            };
            sut = new Mock<IDynamicService<SSG_SearchRequests>>();
            sut.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new SSG_SearchRequests()));
            sut.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<string>(),It.IsAny<SSG_SearchRequests>())).Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
            sut.Setup(x => x.SaveBatch(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MultipartContent>())).Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

        }
        [Test]
        public async Task it_should_get_entity_search_request()
        {
            var response =  await sut.Object.Get(filter, entity);
            Assert.IsInstanceOf<SSG_SearchRequests>(response);

        }

        [Test]
        public async Task it_should_save_entity()
        {
            var response =
                await sut.Object.Save(filter, entity, new SSG_SearchRequests {SSG_PersonGivenName = "Ranti"});
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        [Test]
        public async Task it_should_save_batch_entity()
        {
            var response = await sut.Object.SaveBatch(filter, entity, new MultipartContent());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
