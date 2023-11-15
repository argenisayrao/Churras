using Domain.Services.AcceptInvite;
using Domain.Services.Dtos;
using System.Threading.Tasks;

namespace Domain.Services.DeclineInvite
{
    public interface IAcceptInviteService
    {
        Task<PersonOutput> Run(AnswerInviteInput input);
    }
}
