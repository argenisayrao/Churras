using Domain.Entities;
using Domain.Events;
using System;
using Eveneum;
using System.Linq;
using System.Threading.Tasks;
using CrossCutting;

namespace Domain.Services.CreateNewBbq
{
    internal class CreateNewBbqService : ICreateNewBbqService
    {
        private readonly IEventStore<Bbq> _bbqsStore;
        private readonly SnapshotStore _snapshots;
        private readonly IEventStore<Person> _peopleStore;
        private readonly Person _user;

        public CreateNewBbqService(IEventStore<Bbq> bbqsStore, Person user, SnapshotStore snapshots, IEventStore<Person> peopleStore)
        {
            _bbqsStore = bbqsStore;
            _snapshots = snapshots;
            _peopleStore = peopleStore;
            _user = user;
        }

        public async Task<object> Run(CreateNewBbqServiceInput input)
        {
            var churras = new Bbq();
            churras.Apply(new ThereIsSomeoneElseInTheMood(Guid.NewGuid(), input.Date, input.Reason, input.IsTrincasPaying));

            await _bbqsStore.WriteToStream(churras.Id, churras.Changes.Select(evento => new EventData(churras.Id, evento, new { CreatedBy = _user.Id },
                churras.Version, DateTime.Now.ToString())).ToArray(), expectedVersion: churras.Version == 0 ? null : churras.Version);

            var churrasSnapshot = churras.TakeSnapshot();

            var Lookups = await _snapshots.AsQueryable<Lookups>("Lookups").SingleOrDefaultAsync();

            foreach (var personId in Lookups.ModeratorIds)
            {
                var header = await _peopleStore.ReadHeader(personId);
                var @event = new PersonHasBeenInvitedToBbq(churras.Id, churras.Date, churras.Reason);
                await _peopleStore.WriteToStream(personId, new[] { new EventData(personId, @event, new { CreatedBy = _user.Id }, header.StreamHeader.Version, DateTime.Now.ToString()) }, expectedVersion: header.StreamHeader.Version == 0 ? null : header.StreamHeader.Version);
            }

            return churrasSnapshot;
        }
    }
}
