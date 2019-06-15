using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class WalkerSchedule
    {
        public int Id { get; set; }
        public int WalkerId { get; set; }
        public Walker Walker { get; set; }


    }
}
