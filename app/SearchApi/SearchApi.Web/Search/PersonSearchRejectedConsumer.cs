﻿using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using SearchApi.Core.Adapters.Contracts;
using SearchApi.Core.Adapters.Models;
using SearchApi.Web.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SearchApi.Web.Search
{
    public class PersonSearchRejectedConsumer : IConsumer<PersonSearchRejected>
    {

        private readonly ILogger<PersonSearchRejectedConsumer> _logger;

        private readonly ISearchApiNotifier<ProviderSearchEventStatus> _searchApiNotifier;
        private readonly IMapper _mapper;

        public PersonSearchRejectedConsumer(ISearchApiNotifier<ProviderSearchEventStatus> searchApiNotifier, ILogger<PersonSearchRejectedConsumer> logger, IMapper mapper)
        {
            _searchApiNotifier = searchApiNotifier;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<PersonSearchRejected> context)
        {
            var cts = new CancellationTokenSource();
            var message = _mapper.Map<ProviderSearchEventStatus>(context.Message);
            var profile = context.Headers.Get<ProviderProfile>(nameof(ProviderProfile));
            _logger.LogInformation($"received new {nameof(PersonSearchRejected)} event from {profile.Name}");
            await _searchApiNotifier.NotifyEventAsync(context.Message.SearchRequestId, message,
                cts.Token);
        }
    }
}
