namespace Domain.Services.Dtos
{
    public class AnswerInviteInput
    {
        public AnswerInviteInput(string personId, string invitedId, bool isVeg)
        {
            PersonId = personId;
            InviteId = invitedId;
            IsVeg = isVeg;
        }

        public AnswerInviteInput(string personId, string invitedId)
        {
            PersonId = personId;
            InviteId = invitedId;
        }

        public string PersonId { get; set; }
        public string InviteId { get; set; }
        public bool IsVeg { get; set; }
    }
}
