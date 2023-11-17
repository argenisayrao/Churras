using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.DeclineInvite
{
    public interface IDeclineInviteService
    {
        Task<AnswerInviteOutput> Run(AnswerInviteInput input);
    }
}
