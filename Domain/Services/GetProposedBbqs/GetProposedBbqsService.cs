using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services.GetProposedBbqs
{
    public class GetProposedBbqsService : IGetProposedBbqsService
    {
        private readonly Person _user;
        private readonly IBbqRepository _bbqs;
        private readonly IPersonRepository _repository;

        public GetProposedBbqsService(IPersonRepository repository, IBbqRepository bbqs, Person user)
        {
            _user = user;
            _bbqs = bbqs;
            _repository = repository;
        }

        public async Task<List<object>> Run()
        {
            var bbqs = await GetBbqsWithDateGreatherThanNow();

            var bbqsWithLatestVersion = GetBbqsWithLatestVersionAndStatusEqualPendingConfirmationsOrConfirmed(bbqs);

            var snapshots = new List<object>();

            bbqsWithLatestVersion.ForEach(bbq =>
            {
                snapshots.Add(bbq.TakeSnapshot());
            });

            return snapshots;
        }

        private async Task<List<Bbq>> GetBbqsWithDateGreatherThanNow()
        {
            var moderator = await _repository.GetAsync(_user.Id);

            if (moderator is null)
                return new List<Bbq>();

            var bbqs = new List<Bbq>();

            foreach (var bbqId in moderator.Invites.Where(i => i.Date > DateTime.Now).Select(o => o.Id).ToList())
            {
                var bbq = await _bbqs.GetAsync(bbqId);

                if (bbq is null)
                    continue;

                bbqs.Add(bbq);
            }

            return bbqs;
        }

        private List<Bbq> GetBbqsWithLatestVersionAndStatusEqualPendingConfirmationsOrConfirmed(List<Bbq> bbqs)
        {
            var bbqsWithLatestVersion = bbqs
                .Where(bbq => bbq.Status == BbqStatus.PendingConfirmations || bbq.Status == BbqStatus.Confirmed)
                .GroupBy(bbq => bbq.Id)
                .Select(group => group.OrderByDescending(bbq => bbq.Version)
                .FirstOrDefault())
                .ToList();

            return bbqs;
        }
    }
}
