using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MetingApi.DTOs
{
    public class MetingDTO
    {
        [Required]
        public double? MetingResultaat { get; set; }

        public IList<ResultaatDTO> Resultaten { get; set; }
    }
}
