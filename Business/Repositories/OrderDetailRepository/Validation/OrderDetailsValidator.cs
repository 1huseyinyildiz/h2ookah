using System;
using System.Collections.Generic;
using FluentValidation;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace Business.Repositories.OrderDetailsRepository.Validation
{
    public class OrderDetailsValidator : AbstractValidator<OrderDetail>
    {
        public OrderDetailsValidator()
        {
        }
    }
}
