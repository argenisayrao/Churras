using System;
using System.Collections.Generic;
using Domain.Events;

namespace Domain.Entities
{
    public class Bbq : AggregateRoot
    {
        public string Reason { get; set; }
        public BbqStatus Status { get; set; }
        public DateTime Date { get; set; }
        public bool IsTrincasPaying { get; set; }
        public ShoppingList ShoppingList { get; set; }
        public List<ConfirmedGuest> ConfirmedGuest { get; set; } = new();
        public void When(ThereIsSomeoneElseInTheMood @event)
        {
            Id = @event.Id.ToString();
            Date = @event.Date;
            Reason = @event.Reason;
            Status = BbqStatus.New;
        }

        public void When(BbqStatusUpdated @event)
        {
            if (@event.GonnaHappen)
                Status = BbqStatus.PendingConfirmations;
            else 
                Status = BbqStatus.ItsNotGonnaHappen;

            if (@event.TrincaWillPay)
                IsTrincasPaying = true;
            else
                IsTrincasPaying = false;
        }

        public void When(InviteWasDeclined @event)
        {
            //TODO:Deve ser possível rejeitar um convite já aceito antes.
            //Se este for o caso, a quantidade de comida calculada pelo aceite anterior do convite
            //deve ser retirado da lista de compras do churrasco.
            //Se ao rejeitar, o número de pessoas confirmadas no churrasco for menor que sete,
            //o churrasco deverá ter seu status atualizado para “Pendente de confirmações”. 
        }

        public void When(InviteWasAccepted @event)
        {
            ConfirmedGuest.Add(new ConfirmedGuest(@event.PersonId, @event.IsVeg));

            if (ConfirmedGuest.Count > 6)
                Status = BbqStatus.Confirmed;
            else
                Status = BbqStatus.PendingConfirmations;

            if (@event.IsVeg)
            {               
                ShoppingList.QuantityVegetablesInKilos += 0.6;
            }                
            else
            {
                ShoppingList.QuantityVegetablesInKilos += 0.3;
                ShoppingList.QuantityMeatInKilos += 0.3;
            }
        }

        public object TakeSnapshot()
        {
            return new
            {
                Id,
                Date,
                IsTrincasPaying,
                Status = Status.ToString(),
                ShoppingList
            };
        }
    }
}
