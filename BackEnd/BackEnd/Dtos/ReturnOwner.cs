using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Dtos
{
    public class ReturnOwner
    {
        public ReturnOwner(Petowner owner)
        {

            Id = owner.Id;
            Name = owner.User.FirstName;
            LastName = owner.User.LastName;
            Email = owner.User.Email;
            Email2 = owner.User.Email2;
            Mobile = owner.User.Mobile;
            Province = owner.User.Province;
            Canton = owner.User.Canton;
            Description = owner.User.Description;
            DateCreated = owner.User.DateCreated;
            Pets = owner.Pets.Select(p => new PetDto
            {
                Id = p.Id,
                Name = p.Name,
                Race = p.Race,
                Age = p.Age,
                Size = p.Size,
                Description = p.Description,
                Photos = p.Photos,
                DateCreated = p.DateCreated,
                Trips=p.Walks.Count
            }
            ).ToList();
            Photo = owner.User.Photo;
        }

        public int Id { get; }
        public string Name { get;  set; }
        public string LastName { get;  set; }
        public string Email { get;  set; }
        public string Email2 { get;  set; }
        public string Mobile { get;  set; }
        public string Province { get;  set; }
        public string Canton { get;  set; }
        public string Description { get;  set; }
        public DateTime DateCreated { get;  set; }
        public List<PetDto> Pets { get;  set; }
        public string Photo { get;  set; }
    }
}
