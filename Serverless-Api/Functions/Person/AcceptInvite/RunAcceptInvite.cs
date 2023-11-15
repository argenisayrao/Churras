using Domain.Events;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Domain.Services.DeclineInvite;
using System.Net;
using Domain.Services.AcceptInvite;

namespace Serverless_Api
{
    public partial class RunAcceptInvite
    {
        private readonly IAcceptInviteService _acceptInviteService;
        private readonly IPersonRepository _personRepository;
        private readonly Person _user;
        public RunAcceptInvite(IPersonRepository personRepository, Person user, IAcceptInviteService acceptInviteService)
        {
            _personRepository = personRepository;
            _acceptInviteService = acceptInviteService;
            _user = user;
        }

        [Function(nameof(RunAcceptInvite))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "person/invites/{inviteId}/accept")] HttpRequestData req, string inviteId)
        {
            var answer = await req.Body<InviteAnswer>();

            if (string.IsNullOrWhiteSpace(inviteId))
                return await req.CreateResponse(HttpStatusCode.BadRequest, "Barbecue id is required");

            if (answer is null)
                return await req.CreateResponse(HttpStatusCode.BadRequest, "Body is required");

            var response = await _acceptInviteService.Run(new AnswerInviteInput ( _user.Id,  inviteId, answer.IsVeg));
          
            if(response.Person is null)
                return req.CreateResponse(HttpStatusCode.NotFound);

            return await req.CreateResponse(HttpStatusCode.OK, response.Person.TakeSnapshot());
        }
    }
}
