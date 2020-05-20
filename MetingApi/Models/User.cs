using MetingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class User
    {
        #region Properties
        //add extra properties if needed
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public ICollection<Meting> Metingen { get; set; }
        #endregion

        #region Constructors
        public User()
        {
            Metingen = new List<Meting>();
        }
        #endregion

        #region Methods
        public void AddMeting(Meting meting)
        {
            Metingen.Add(new Meting() { Id = meting.Id, Created = DateTime.Now, User = this, MetingResultaat = meting.MetingResultaat});
        }
        #endregion
    }
}

