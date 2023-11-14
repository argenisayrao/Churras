using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Services.RunModerateBbq
{
    public interface IModerateBbqService
    {
        Task<ModerateBbqOutput> Run(ModerateBbqInput input);
    }
}
