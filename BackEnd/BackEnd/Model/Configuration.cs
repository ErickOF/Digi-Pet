using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class AppConfiguration
    {
        public int Id { get; set; }
        [Required]
        public string Property { get; set; }
        [Required]
        public string Value { get; set; }

    }
}
