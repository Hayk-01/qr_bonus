using FluentValidation;
using QRBonus.DTO.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRBonus.BLL.Validators.ProductValidators
{
    public class AddOrUpdateProductValidator : AbstractValidator<ProductDto>
    {
        public AddOrUpdateProductValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(256);
        }
    }
}
