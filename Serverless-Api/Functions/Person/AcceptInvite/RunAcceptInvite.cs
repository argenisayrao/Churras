using Domain.Events;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Domain.Services.DeclineInvite;
using System.Net;

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
            if(string.IsNullOrWhiteSpace(inviteId))
                return await req.CreateResponse(HttpStatusCode.BadRequest, "Barbecue id is required");

            var answer = await req.Body<InviteAnswer>();

            //if (string.IsNullOrWhiteSpace(answer.))
            //    return await req.CreateResponse(HttpStatusCode.BadRequest, "Barbecue id is required");

            var person = await _personRepository.GetAsync(_user.Id);
           
            person.Apply(new InviteWasAccepted { InviteId = inviteId, IsVeg = answer.IsVeg, PersonId = person.Id });

            await _personRepository.SaveAsync(person);

            //implementar efeito do aceite do convite no churrasco
            //quando tiver 7 pessoas ele está confirmado

            return await req.CreateResponse(System.Net.HttpStatusCode.OK, person.TakeSnapshot());
        }
    }
}
