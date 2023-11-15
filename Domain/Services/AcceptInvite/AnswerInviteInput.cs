﻿namespace Domain.Services.AcceptInvite
{
    public class AnswerInviteInput
    {
        public AnswerInviteInput(string personId, string invitedId, bool isVeg)
        {
            PersonId = personId;
            InviteId = invitedId;
            IsVeg = isVeg;
        }

        public string PersonId { get; set; }
        public string InviteId { get; set; }
        public bool IsVeg { get; set; }
    }
}
