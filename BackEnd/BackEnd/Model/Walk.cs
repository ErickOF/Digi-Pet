using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class Walk
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public int WalkerId { get; set; }
        public virtual Walker Walker { get; set; }
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ReportWalk> ReportWalks { get; set; }

    }
}
