using Domain.Repositories;
using System.Threading.Tasks;

namespace Domain.Services.GetInvite
{
    internal class GetInviteService : IGetInviteService
    {
        private readonly IPersonRepository _repository;

        public GetInviteService(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetInviteOutput> Run(string id)
        {
            var person = await _repository.GetAsync(id);

            var personOutput = new GetInviteOutput(person);

            return personOutput;
        }
    }
}
