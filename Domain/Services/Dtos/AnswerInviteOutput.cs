using Domain.Entities;

namespace Domain.Services.Dtos
{
    public class AnswerInviteOutput
    {
        public Person? Person { get; set; }
        public Bbq? Bbq { get; set; }
        public bool PersonWasFound { get; set; }
        public bool BbqWasFound { get; set; }
        public AnswerInviteOutput(Person? person, Bbq? bbq)
        {
            if (person == null)
                PersonWasFound = false;
            else
            {
                Person = person;
                PersonWasFound = true;
            }

            if (bbq == null)
                BbqWasFound = false;
            else
            {
                Bbq = bbq;
                BbqWasFound = true;
            }
        }
    }
}
