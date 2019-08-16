using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MockServer.Entities;
using MockServer.MongoStorage;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Niusys.Extensions.ResponseEnvelopes;
using Niusys.Utils;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MockServer.Middlewares
{
    public class ApiRequestProcessor
    {
        public RequestDelegate Next { get; }
        public ILogger<ApiRequestProcessor> Logger { get; }

        private readonly IApplicationLifetime _applicationLifetime;

        public ApiRequestProcessor(RequestDelegate next,
            ILogger<ApiRequestProcessor> logger,
            IApplicationLifetime applicationLifetime)
        {
            this.Next = next;
            this.Logger = logger;
            this._applicationLifetime = applicationLifetime;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.HasValue || !context.Request.Path.Value.Contains("/api/", StringComparison.OrdinalIgnoreCase))
            {
                await Next(context);
                return;
            }

            var requestPath = context.Request.Path.Value;
            var pathArray = requestPath.Trim('/').Split('/');
            var category = pathArray[0];
            var apiSection = pathArray[1];
            var realApiPath = requestPath.Substring(category.Length + 1);
            if (!apiSection.Equals("api", StringComparison.OrdinalIgnoreCase))
            {
                await Next(context);
                return;
            }

            var apiInterfaceRepository = context.RequestServices.GetService<MockServerNoSqlRepository<ApiInterface>>();
            var entity = await apiInterfaceRepository.SearchOneAsync(Builders<ApiInterface>.Filter.Where(x => x.Category == category && x.RequestPath == realApiPath));
            if (entity != null)
            {
                var envelop = new EnvelopMessage<object>()
                {
                    Code = 200,
                    Tid = GuidGenerator.GenerateDigitalUUID(),
                    FriendlyMessage = "返回成功",
                    ErrorMessage = string.Empty
                };
                envelop.Data = JToken.Parse(entity.ResponseResult);
                await HandleStatus(context, envelop);
            }
            else
            {
                await InterfaceNotFound(context);
            }
        }

        private async Task HandleStatus(HttpContext context, object statsContent)
        {
            context.Response.OnStarting(async state =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "application/json; charset=utf-8";
                await Task.CompletedTask;
            }, context);
            await context.Response.Body.WriteContent(statsContent);
        }

        private async Task InterfaceNotFound(HttpContext context)
        {
            context.Response.OnStarting(async state =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json; charset=utf-8";
                await Task.CompletedTask;
            }, context);
            await context.Response.Body.WriteContent(string.Empty);
        }
    }
}
