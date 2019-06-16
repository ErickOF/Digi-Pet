using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Dtos
{
    public class ReturnWalker
    {
        public ReturnWalker()
        {

        }
        public ReturnWalker(Walker u)
        {
            Id = u.Id;
            UserName = u.User.Username;
            FirstName = u.User.FirstName;
            LastName = u.User.LastName;
            Email = u.User.Email;
            Email2  = u.User.Email2;
            University = u.University;
            Province = u.User.Province;
            Canton = u.User.Canton;
            DoesOtherProvinces = u.DoesOtherProvinces;
            OtherProvinces = u.OtherProvinces;
            Mobile = u.User.Mobile;
            Description = u.User.Description;
            DateCreated = u.User.DateCreated;
            Rating = u.Score;
            Trips = u.Walks.Count;
            Photo = u.User.Photo;
        }

        public int Id { get;  set; }
        public string UserName { get;  set; }
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public string Email { get;  set; }
        public string Email2 { get;  set; }
        public string University { get;  set; }
        public string Province { get;  set; }
        public string Canton { get;  set; }
        public bool DoesOtherProvinces { get;  set; }
        public string[] OtherProvinces { get;  set; }
        public string Mobile { get;  set; }
        public string Description { get;  set; }
        public DateTime DateCreated { get;  set; }
        public decimal Rating { get;  set; }
        public int Trips { get;  set; }
        public string Photo { get;  set; }
    }
}

