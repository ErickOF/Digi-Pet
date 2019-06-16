using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    public class Rate
    {
        [Required]
        public int walkId { get; set; }
        [Required]
        public int stars { get; set; }

        public DenunciaDto Denuncia { get; set; }
    }
}
