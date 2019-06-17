using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class Denuncia
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int WalkId { get; set; }
        public Walk Walk { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
    }
}
