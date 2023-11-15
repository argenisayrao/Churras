using Domain.Services.GetInvite;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Serverless_Api
{
    public partial class RunGetInvites
    {
        private readonly IGetInviteService _getInviteService;

        public RunGetInvites(IGetInviteService getInviteService)
        {
            _getInviteService = getInviteService;
        }

        [Function(nameof(RunGetInvites))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "person/{personId}/invites")] HttpRequestData req, string personId)
        {
            if(string.IsNullOrWhiteSpace(personId))
                return await req.CreateResponse(HttpStatusCode.BadRequest, "person id is required");

            var response = await _getInviteService.Run(personId);

            if (response.Person == null)
                return req.CreateResponse(HttpStatusCode.NotFound);

            return await req.CreateResponse(HttpStatusCode.OK, response.Person.TakeSnapshot());
        }
    }
}
