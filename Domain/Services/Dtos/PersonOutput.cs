using Domain.Entities;

namespace Domain.Services.Dtos
{
    public class PersonOutput
    {
        public Person? Person { get; set; }
        public bool WasFound { get; set; }
        public PersonOutput(Person person)
        {
            if (person == null)
                WasFound = false;
            else
            {
                Person = person;
                WasFound = true;
            }

        }
    }
}
