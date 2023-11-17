using Domain.Entities;
using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.RunModerateBbq
{
    public interface IModerateBbqService
    {
        Task<BbqOutput> Run(ModerateBbqInput input);
    }
}
