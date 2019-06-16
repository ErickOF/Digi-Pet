using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class ReportWalk
    {
        public int Id { get; set; }
        [Required]
        public int WalkId { get; set; }
        public virtual Walk  Walk { get; set; }
        public int Stars { get; set; }
        [Required]
        public string Comments { get; set; }
        [Required]
        public string Photos { get; set; }
        [Required, Column(TypeName = "decimal(4,2)"), Range(0,double.MaxValue)]
        public decimal Distance { get; set; }

        public string[] Necesidades { get; set; }


    }

}
