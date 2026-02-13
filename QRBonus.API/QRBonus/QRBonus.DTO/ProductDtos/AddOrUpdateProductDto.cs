using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.DTO.ProductDtos
{
    public class AddOrUpdateProductDto : BaseWithRegionDto
    {
        public FileDto? AddPhoto { get; set; }
        public string? Link { get; set; }   
        public List<TranslationDto> Translations {  get; set; } 
    }
}
