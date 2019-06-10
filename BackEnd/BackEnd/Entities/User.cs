using System.Collections;
using System.Collections.Generic;
using WebApi.Model;

namespace WebApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public virtual ICollection<Petowner> Petowners { get; set; }
        public virtual ICollection<Walker> Walkers { get; set; }
    }
}