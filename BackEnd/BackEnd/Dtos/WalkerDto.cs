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
        public string SchoolId { get; set; } //número de carné
        [Required,MinLength(8),DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.EmailAddress)]
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
        [Required]
        public string Description { get; set; }
    }
}
