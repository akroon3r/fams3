﻿using System;
using System.Threading.Tasks;
using DynamicsAdapter.Web.SearchApi;
using Moq;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using DynamicsAdapter.Web.SearchRequest;
using Quartz;

namespace DynamicsAdapter.Web.Test.SearchRequest
{
    public class SearchRequestJobTest
    {
     
        private readonly Mock<ILogger<SearchRequestJob>> _loggerMock = new Mock<ILogger<SearchRequestJob>>();
        private readonly Mock<IJobExecutionContext> _jobExecutionContextMock = new Mock<IJobExecutionContext>();
        private readonly Mock<IPeopleClient> _peopleClientMock = new Mock<IPeopleClient>();

        private SearchRequestJob _sut;

        [SetUp]
        public void Setup()
        {

            _peopleClientMock.Setup(x => x.SearchAsync(It.IsAny<PersonSearchRequest>())).Returns(Task.FromResult(
                new PersonSearchResponse()
                {
                    Id = Guid.NewGuid()
                }));

            _sut = new SearchRequestJob(_peopleClientMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task It_should_execute_the_job()
        {
            await _sut.Execute(_jobExecutionContextMock.Object);
            _peopleClientMock.Verify(x => x.SearchAsync(It.IsAny<PersonSearchRequest>()), Times.Once);

        }


    }
}