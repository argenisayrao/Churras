using Domain;
using Eveneum;
using CrossCutting;
using Domain.Events;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using static Domain.ServiceCollectionExtensions;
using Domain.Services.DeclineInvite;
using Domain.Services.Dtos;
using System.Net;

namespace Serverless_Api
{
    public partial class RunDeclineInvite
    {
        private readonly Person _user;
        private readonly IDeclineInviteService _declineInviteService;

        public RunDeclineInvite(Person user, IDeclineInviteService declineInviteService)
        {
            _user = user;
            _declineInviteService = declineInviteService;
        }

        [Function(nameof(RunDeclineInvite))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "person/invites/{inviteId}/decline")] HttpRequestData req, string inviteId)
        {
            if (string.IsNullOrWhiteSpace(inviteId))
                return await req.CreateResponse(HttpStatusCode.BadRequest, "Barbecue id is required");

            if (string.IsNullOrWhiteSpace(_user.Id))
                return await req.CreateResponse(HttpStatusCode.Unauthorized, "User id is required in header");

            var response = await _declineInviteService.Run(new AnswerInviteInput(_user.Id, inviteId));

            if (response.PersonWasFound is false)
                return await req.CreateResponse(HttpStatusCode.NotFound, "User not found");

            if (response.BbqWasFound is false)
                return await req.CreateResponse(HttpStatusCode.NotFound, "Barbecue not found");

            return await req.CreateResponse(HttpStatusCode.OK, response.Person.TakeSnapshot());
        }
    }
}
