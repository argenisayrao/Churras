using Domain.Events;
using Domain.Repositories;
using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.DeclineInvite
{
    internal class DeclineInviteService: IDeclineInviteService
    {
        private readonly IBbqRepository _bbqRepository;
        private readonly IPersonRepository _personRepository;
        
        public DeclineInviteService(IBbqRepository bbqRepository, IPersonRepository personRepository)
        {
            _bbqRepository = bbqRepository;
            _personRepository = personRepository;
        }

        public async Task<AnswerInviteOutput> Run(AnswerInviteInput input)
        {
            var person = await _personRepository.GetAsync(input.PersonId);
            var bbq = await _bbqRepository.GetAsync(input.InviteId);

            if (person is null || bbq is null)
                return new AnswerInviteOutput(person, bbq);
 
            person.Apply(new InviteWasDeclined { InviteId = input.InviteId, PersonId = person.Id });
            bbq.Apply(new InviteWasDeclined { InviteId = input.InviteId, PersonId = person.Id });

            await _personRepository.SaveAsync(person);
            await _bbqRepository.SaveAsync(bbq);

            return new AnswerInviteOutput(person,bbq);
        }
    }
}
