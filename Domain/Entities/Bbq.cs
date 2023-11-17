using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Events;

namespace Domain.Entities
{
    public class Bbq : AggregateRoot
    {
        public string Reason { get; set; }
        public BbqStatus Status { get; set; }
        public DateTime Date { get; set; }
        public bool IsTrincasPaying { get; set; }
        public ShoppingList ShoppingList { get; set; } = new();
        public List<ConfirmedGuest> ConfirmedGuest { get; set; } = new();
        public void When(ThereIsSomeoneElseInTheMood @event)
        {
            Id = @event.Id.ToString();
            Date = @event.Date;
            Reason = @event.Reason;
            Status = BbqStatus.New;
        }

        internal void When(BbqStatusUpdated @event)
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

        internal void When(InviteWasDeclined @event)
        {
            var person = ConfirmedGuest.Where(guest => guest.Id == @event.PersonId).FirstOrDefault();

            if (person is null)
                return;

            if (person.IsVeg)
                ShoppingList.QuantityVegetablesInKilos -= 0.6;
            else
            {
                ShoppingList.QuantityVegetablesInKilos -= 0.3;
                ShoppingList.QuantityMeatInKilos -= 0.3;
            }

            ConfirmedGuest.Remove(person);
            SetStatus();
        }

        internal void When(InviteWasAccepted @event)
        {
            if (ConfirmedGuest.Select(guest => guest.Id).Contains(@event.PersonId))
            {
                UpdatedGuestAndUpdateShoppingList(@event);
            }
            else
            {
                CreateGuestAndSetShoppingList(@event);
            }
            ShoppingList.QuantityVegetablesInKilos = Math.Round(ShoppingList.QuantityVegetablesInKilos, 2);
            ShoppingList.QuantityMeatInKilos = Math.Round(ShoppingList.QuantityMeatInKilos, 2);
        }

        internal void CreateGuestAndSetShoppingList(InviteWasAccepted @event)
        {
            ConfirmedGuest.Add(new ConfirmedGuest(@event.PersonId, @event.IsVeg));
            SetStatus();

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

        internal void UpdatedGuestAndUpdateShoppingList(InviteWasAccepted @event)
        {
            var person = ConfirmedGuest.Where(guest => guest.Id == @event.PersonId).FirstOrDefault();

            if (person.IsVeg == @event.IsVeg)
                return;

            if (@event.IsVeg)
            {
                ShoppingList.QuantityVegetablesInKilos += 0.3;
                ShoppingList.QuantityMeatInKilos -= 0.3;
            }
            else
            {
                ShoppingList.QuantityVegetablesInKilos -= 0.3;
                ShoppingList.QuantityMeatInKilos += 0.3;
            }

            ConfirmedGuest.Where(guest => guest.Id == @event.PersonId).FirstOrDefault().IsVeg = @event.IsVeg;
        }

        private void SetStatus()
        {
            if (ConfirmedGuest.Count > 6)
                Status = BbqStatus.Confirmed;
            else
                Status = BbqStatus.PendingConfirmations;
        }
        public object TakeSnapshot()
        {
            return new
            {
                Id,
                Date,
                IsTrincasPaying,
                Status = Status.ToString()
            };
        }

        public object TakeSnapshotWithShoppingListAndConfirmedGuest()
        {
            return new
            {
                Id,
                Date,
                IsTrincasPaying,
                Status = Status.ToString(),
                ShoppingList,
                ConfirmedGuest
            };
        }
    }
}
