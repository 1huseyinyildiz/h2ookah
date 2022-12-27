using Entities.Concrete;
using FluentValidation;

namespace Business.Repositories.CustomerRealitonShipRepository.Validation
{
    public class CustomerRealitonShipValidator : AbstractValidator<CustomerRelationship>
    {
        public CustomerRealitonShipValidator()
        {
        }
    }
}
