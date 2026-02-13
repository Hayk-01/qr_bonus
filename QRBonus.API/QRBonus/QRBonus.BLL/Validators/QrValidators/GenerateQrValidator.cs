using FluentValidation;
using QRBonus.DTO.QrCodeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Validators.QrValidators
{
    public class GenerateQrValidator: AbstractValidator <AddQrCodeDto>
    {
        public GenerateQrValidator()
        {
            RuleFor(x => x.QrCodeCounts.Sum(y => y.Count))
                .LessThanOrEqualTo(100000);
        }
    }
}
