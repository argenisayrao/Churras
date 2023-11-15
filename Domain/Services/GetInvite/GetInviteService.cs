using Domain.Repositories;
using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.GetInvite
{
    internal class GetInviteService : IGetInviteService
    {
        private readonly IPersonRepository _personRepository;

        public GetInviteService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonOutput> Run(string id)
        {
            var person = await _personRepository.GetAsync(id);

            var personOutput = new PersonOutput(person);

            return personOutput;
        }
    }
}
