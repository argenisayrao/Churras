using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.DeclineInvite
{
    public interface IAcceptInviteService
    {
        Task<AnswerInviteOutput> Run(AnswerInviteInput input);
    }
}
