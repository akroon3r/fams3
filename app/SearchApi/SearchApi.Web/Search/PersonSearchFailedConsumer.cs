using AutoMapper;
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
    public class PersonSearchFailedConsumer : IConsumer<PersonSearchFailed>
    {

        private readonly ILogger<PersonSearchFailedConsumer> _logger;

        private readonly ISearchApiNotifier<ProviderSearchEventStatus> _searchApiNotifier;
        private readonly IMapper _mapper;

        public PersonSearchFailedConsumer(ISearchApiNotifier<ProviderSearchEventStatus> searchApiNotifier, ILogger<PersonSearchFailedConsumer> logger, IMapper mapper)
        {
            _searchApiNotifier = searchApiNotifier;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<PersonSearchFailed> context)
        {
            var cts = new CancellationTokenSource();
            var message = _mapper.Map<ProviderSearchEventStatus>(context.Message);
            var profile = context.Headers.Get<ProviderProfile>(nameof(ProviderProfile));
            _logger.LogInformation($"received new {nameof(PersonSearchFailed)} event from {profile.Name}");
            await _searchApiNotifier.NotifyEventAsync(context.Message.SearchRequestId, message,
                cts.Token);
        }
    }
}