﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    public class DenunciaDto
    {
 
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }

    }
}
