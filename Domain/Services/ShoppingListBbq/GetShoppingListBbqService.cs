using Domain.Repositories;
using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.ShoppingListBbq
{
    internal class GetShoppingListBbqService: IGetShoppingListBbqService
    {
        private readonly IBbqRepository _bbqRepository;

        public GetShoppingListBbqService(IBbqRepository bbqRepository)
        {
            _bbqRepository = bbqRepository;
        }

        public async Task<BbqOutput> Run(string id)
        {
            var bbq = await _bbqRepository.GetAsync(id);

            var output = new BbqOutput(bbq);

            return output;
        }
    }
}
