using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.CustomAttributes;

namespace WebApi.Model
{
    public class Walk
    {
        public int Id { get; set; }
        [Required]
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }
        [Required]
        public int WalkerId { get; set; }
        public virtual Walker Walker { get; set; }

        [Required,DataType(DataType.DateTime),ValidateDate]
        public DateTime Begin { get; set; }

        [Required, Column(TypeName = "decimal(2,1)"), Range(0.5, 8.0)]
        public decimal Duration { get; set; }
        public virtual DateTime End { get
            {
                return Begin.AddHours((double)Duration);
            }
        }
        [Required]
        public string Province { get; set; }
        [Required]
        public string Canton { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ExactAddress { get; set; }

        public virtual ICollection<ReportWalk> ReportWalks { get; set; }
    }
}
