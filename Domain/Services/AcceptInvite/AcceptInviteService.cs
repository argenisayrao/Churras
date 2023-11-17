using Domain.Events;
using Domain.Repositories;
using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.DeclineInvite
{
    internal class AcceptInviteService: IAcceptInviteService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IBbqRepository _bbqRepository;
        public AcceptInviteService(IPersonRepository personRepository, IBbqRepository bbqRepository)
        {
            _personRepository = personRepository;
            _bbqRepository = bbqRepository;
        }
        public async Task<AnswerInviteOutput> Run(AnswerInviteInput input)
        {
            var person = await _personRepository.GetAsync(input.PersonId);
            var bbq = await _bbqRepository.GetAsync(input.InviteId);

            if (person is null || bbq is null)
                return new AnswerInviteOutput(person, bbq);

            person.Apply(new InviteWasAccepted { InviteId = input.InviteId, IsVeg = input.IsVeg, PersonId = person.Id });
            bbq.Apply(new InviteWasAccepted { InviteId = input.InviteId, IsVeg = input.IsVeg, PersonId = person.Id });

            await _personRepository.SaveAsync(person);
            await _bbqRepository.SaveAsync(bbq);

            return new AnswerInviteOutput(person,bbq);
        }
    }
}
