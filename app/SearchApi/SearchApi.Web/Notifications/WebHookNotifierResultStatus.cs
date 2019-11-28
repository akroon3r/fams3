﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SearchApi.Core.Adapters.Contracts;
using SearchApi.Web.Configuration;

namespace SearchApi.Web.Notifications
{
    public class WebHookNotifierResultStatus :ISearchApiNotifier<PersonFound> 
    {

        private readonly HttpClient _httpClient;
        private readonly SearchApiOptions _searchApiOptions;
        private readonly ILogger<WebHookNotifierResultStatus> _logger;

        public WebHookNotifierResultStatus(HttpClient httpClient, IOptions<SearchApiOptions> searchApiOptions,
            ILogger<WebHookNotifierResultStatus> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _searchApiOptions = searchApiOptions.Value;
        }

        public async Task NotifyEventAsync(Guid searchRequestId, PersonFound matchFound,
           CancellationToken cancellationToken)
        {
            foreach (var webHook in _searchApiOptions.WebHooks.FindAll(x => x.EventName.Contains(nameof(PersonFound), StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogDebug(
                   $"The webHook {nameof(PersonFound)} notification is attempting to send event for {webHook.Name} webhook.");

                if (!URLHelper.TryCreateUri(webHook.Uri, $"{searchRequestId}", out var endpoint))
                {
                    _logger.LogWarning(
                        $"The webHook {nameof(PersonFound)} notification uri is not established or is not an absolute Uri for {webHook.Name}. Set the WebHook.Uri value on SearchApi.WebHooks settings.");
                    return;
                }

                using var request = new HttpRequestMessage();

                try
                {
                    var content = new StringContent(JsonConvert.SerializeObject(matchFound));
                    content.Headers.ContentType =
                        System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json");
                    request.Content = content;
                    request.Method = HttpMethod.Post;
                    request.Headers.Accept.Add(
                        System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
                    request.RequestUri = endpoint;
                    var response = await _httpClient.SendAsync(request, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError(
                            $"The webHook {typeof(PersonFound).Name} notification has not executed successfully for {webHook.Name} webHook. The error code is {response.StatusCode.GetHashCode()}.");
                        return;
                    }

                    _logger.LogInformation(
                        $"The webHook {typeof(PersonFound).Name} notification has executed successfully for {webHook.Name} webHook.");

                }
                catch (Exception exception)
                {
                    _logger.LogError($"The failure notification for {webHook.Name} has not executed successfully.",
                        exception);
                }
            }
        }
       


      

    }

}