using System.ComponentModel.DataAnnotations;

namespace MetingApi.DTOs
{
    public class ResultaatDTO
    {
        [Required]
        public string Vraag { get; set; }

        public double? Amount { get; set; }
    }
}
