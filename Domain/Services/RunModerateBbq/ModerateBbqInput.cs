using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.RunModerateBbq
{
    public class ModerateBbqInput
    {
        public ModerateBbqInput(bool gonnaHappen, bool trincaWillPay, string barbecueId)
        {
            GonnaHappen = gonnaHappen;
            TrincaWillPay = trincaWillPay;
            BarbecueId = barbecueId;
        }

        public bool GonnaHappen { get; set; }
        public bool TrincaWillPay { get; set; }
        public string BarbecueId { get; set; }

    }
}
