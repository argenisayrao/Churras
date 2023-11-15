using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.RunModerateBbq
{
    public class ModerateBbqOutput
    {
        public Bbq? Barbecue { get; set; }

        public ModerateBbqOutput(Bbq? barbecue)
        {
            Barbecue = barbecue;
        }

        public ModerateBbqOutput()
        {
        }
    }
}
