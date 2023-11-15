using Domain.Entities;
using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.GetInvite
{
    public interface IGetInviteService
    {
        Task<PersonOutput> Run(string id);
    }
}
