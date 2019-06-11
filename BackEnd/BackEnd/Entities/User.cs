using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi.Model;

namespace WebApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Province { get; set; }
        public string Canton { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email2 { get; set; }
        public string Mobile { get; set; }

        [Required]
        public string Role { get; set; }
        public string Token { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Petowner> Petowners { get; set; }
        public virtual ICollection<Walker> Walkers { get; set; }
    }
}