using System.Collections.Generic;

namespace MetingApi.Models
{
    public interface IMetingRepository
    {
        Meting GetBy(int id);
        bool TryGetMeting(int id, out Meting meting);
        IEnumerable<Meting> GetAll();
        IEnumerable<Meting> GetBy(string resultaatVraag = null);
        void Add(Meting meting);
        void Delete(Meting meting);
        void Update(Meting meting);
        void SaveChanges();
    }
}

