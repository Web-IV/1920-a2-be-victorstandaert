using MetingApi.Models;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.DTOs
{
    public class UserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public IEnumerable<Meting> Metingen { get; set; }

        public UserDTO() { }

        public UserDTO(User user) : this()
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Metingen = user.Metingen;
        }
    }
}
