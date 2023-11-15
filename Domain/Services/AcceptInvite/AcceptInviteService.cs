using Domain.Entities;
using Domain.Events;
using Domain.Repositories;
using Domain.Services.AcceptInvite;
using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.DeclineInvite
{
    internal class AcceptInviteService: IAcceptInviteService
    {
        private readonly IPersonRepository _personRepository;
        public AcceptInviteService(IPersonRepository personRepository, Person user)
        {
            _personRepository = personRepository;
        }
        public async Task<PersonOutput> Run(AnswerInviteInput input)
        {
            var person = await _personRepository.GetAsync(input.PersonId);

            if (person is null)
                return new PersonOutput(person);

            person.Apply(new InviteWasAccepted { InviteId = input.InviteId, IsVeg = input.IsVeg, PersonId = person.Id });

            await _personRepository.SaveAsync(person);

            return new PersonOutput(person);

            //implementar efeito do aceite do convite no churrasco
            //quando tiver 7 pessoas ele está confirmado

 

        }
    }
}
