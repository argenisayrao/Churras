using Domain.Entities;
using Domain.Services.GetInvite;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Serverless_Api
{
    public partial class RunGetInvites
    {
        private readonly IGetInviteService _getInviteService;
        private readonly Person _user;

        public RunGetInvites(IGetInviteService getInviteService, Person user)
        {
            _getInviteService = getInviteService;
            _user = user;
        }

        [Function(nameof(RunGetInvites))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "person/invites")] HttpRequestData req)
        {
            if (string.IsNullOrWhiteSpace(_user.Id))
                return await req.CreateResponse(HttpStatusCode.Unauthorized, "User id is required in header");

            var response = await _getInviteService.Run(_user.Id);

            if (response.Person == null)
                return req.CreateResponse(HttpStatusCode.NotFound);

            return await req.CreateResponse(HttpStatusCode.OK, response.Person.TakeSnapshot());
        }
    }
}
