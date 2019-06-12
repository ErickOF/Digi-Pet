using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class ReportWalk
    {
        public int Id { get; set; }
        public int WalkId { get; set; }
        public virtual Walk  Walk { get; set; }
        public int Stars { get; set; }

        public string Comments { get; set; }
    }
}
