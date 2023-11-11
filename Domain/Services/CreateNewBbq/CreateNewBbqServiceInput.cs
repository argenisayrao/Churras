using System;

namespace Domain.Services.CreateNewBbq
{
    public class CreateNewBbqServiceInput
    {
        public CreateNewBbqServiceInput(DateTime date, string reason, bool isTrincaPaying)
        {
            Date = date;
            Reason = reason;
            IsTrincasPaying = isTrincaPaying;
        }

        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public bool IsTrincasPaying { get; set; }
    }
}
