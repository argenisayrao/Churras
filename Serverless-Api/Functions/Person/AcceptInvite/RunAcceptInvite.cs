using Domain.Events;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Serverless_Api
{
    public partial class RunAcceptInvite
    {
        private readonly Person _user;
        private readonly IPersonRepository _personRepository;
        public RunAcceptInvite(IPersonRepository personRepository, Person user)
        {
            _user = user;
           _personRepository = personRepository;
        }

        [Function(nameof(RunAcceptInvite))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "person/invites/{inviteId}/accept")] HttpRequestData req, string inviteId)
        {
            var answer = await req.Body<InviteAnswer>();

            var person = await _personRepository.GetAsync(_user.Id);
           
            person.Apply(new InviteWasAccepted { InviteId = inviteId, IsVeg = answer.IsVeg, PersonId = person.Id });

            await _personRepository.SaveAsync(person);

            //implementar efeito do aceite do convite no churrasco
            //quando tiver 7 pessoas ele está confirmado

            return await req.CreateResponse(System.Net.HttpStatusCode.OK, person.TakeSnapshot());
        }
    }
}
