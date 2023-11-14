using CrossCutting;
using Domain.Entities;
using Domain.Events;
using Domain.Repositories;
using Domain.Repositories.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services.RunModerateBbq
{
    internal class ModerateBbqService : IModerateBbqService
    {
        private readonly SnapshotStore _snapshots;
        private readonly IPersonRepository _persons;
        private readonly IBbqRepository _repository;

        public ModerateBbqService(IBbqRepository repository, SnapshotStore snapshots, IPersonRepository persons)
        {
            _persons = persons;
            _repository = repository;
            _snapshots = snapshots;
        }

        public async Task<ModerateBbqOutput> Run(ModerateBbqInput input)
        {
            var bbq = await _repository.GetAsync(input.BarbecueId);

            if (bbq == null)
                return new ModerateBbqOutput();

            if (input.GonnaHappen is false)
            {
                var query = _snapshots.AsQueryable<PersonsHasBeenInvitedToBbqDto>("People").Where(person => person.Body.Id == input.BarbecueId);
                var personsDto = await SnapshotStoreExtensions.ToListAsync<PersonsHasBeenInvitedToBbqDto>(query);

                var persons = new List<Person>();

                foreach (var personDto in personsDto)
                {
                    var person = await _persons.GetAsync(personDto.StreamId);

                    if (person == null)
                        continue;

                    person.Apply(new InviteWasDeclined() { PersonId = personDto.StreamId, InviteId = personDto.Body.Id });

                    await _persons.SaveAsync(person);
                }
            }
            else
            {
                var lookups = await _snapshots.AsQueryable<Lookups>("Lookups").SingleOrDefaultAsync();

                foreach (var personId in lookups.PeopleIds)
                {
                    var person = await _persons.GetAsync(personId);
                    var @event = new PersonHasBeenInvitedToBbq(bbq.Id, bbq.Date, bbq.Reason);

                    if (person == null)
                        continue;

                    person.Apply(@event);
                    await _persons.SaveAsync(person);
                }
            }
            
            bbq.Apply(new BbqStatusUpdated(input.GonnaHappen, input.TrincaWillPay));
            
            await _repository.SaveAsync(bbq);

            return new ModerateBbqOutput(bbq);
        }
    }
}
