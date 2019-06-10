using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    public class WalkerDto
    {
        [Required]
        public string Id { get; set; } //número de carné
        [Required,MinLength(8)]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Email2 { get; set; }
        [Required]
        public string University { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string Canton { get; set; }
        [Required]
        public bool DoesOtherProvinces { get; set; }//permite otras provincias
        public string[] OtherProvinces { get; set; }

        [Required]
        public string Mobile { get; set; }
    }
}
