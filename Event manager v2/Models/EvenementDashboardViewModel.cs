using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Event_manager_v2.Models
{
    public class EvenementDashboardViewModel
    {
        public Evenement Evenement { get; set; }
        public IEnumerable<Beheerder> Beheerders {get; set;}
        public IEnumerable<Activiteit> Activiteiten { get; set; }

        public IEnumerable<Deelnemer> Deelnemers { get; set; }
    }
}