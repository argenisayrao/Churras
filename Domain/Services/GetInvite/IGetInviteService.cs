using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Services.GetInvite
{
    public interface IGetInviteService
    {
        Task<GetInviteOutput> Run(string id);
    }
}
