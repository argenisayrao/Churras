using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.ShoppingListBbq
{
    public interface IGetShoppingListBbqService
    {
        Task<BbqOutput> Run(string id);
    }
}
