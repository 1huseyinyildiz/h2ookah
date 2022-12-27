using Business.Aspects.Secured;
using Business.Repositories.CustomerRealitonShipRepository.Constants;
using Business.Repositories.CustomerRealitonShipRepository.Validation;
using Business.Repositories.CustomerRelationshipRepository;
using Core.Aspects.Caching;
using Core.Aspects.Validation;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CustomerRelationshipRepository;
using Entities.Concrete;

namespace Business.Repositories.CustomerRealitonshipRepository
{
    public class CustomerRelationshipManager : ICustomerRelationshipService
    {
        private readonly ICustomerRelationshipDal _customerRelationshipDal;

        public CustomerRelationshipManager(ICustomerRelationshipDal customerRelationshipDal)
        {
            _customerRelationshipDal = customerRelationshipDal;
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(CustomerRealitonShipValidator))]
        [RemoveCacheAspect("ICustomerRealitonShipService.Get")]

        public async Task<IResult> Add(CustomerRelationship customerRelationsShip)
        {
            await _customerRelationshipDal.Add(customerRelationsShip);
            return new SuccessResult(CustomerRealitonShipMessages.Added);
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(CustomerRealitonShipValidator))]
        [RemoveCacheAspect("ICustomerRealitonShipService.Get")]

        public async Task<IResult> Update(CustomerRelationship customerRealitonShip)
        {
            await _customerRelationshipDal.Update(customerRealitonShip);
            return new SuccessResult(CustomerRealitonShipMessages.Updated);
        }

        [SecuredAspect()]
        [RemoveCacheAspect("ICustomerRealitonShipService.Get")]

        public async Task<IResult> Delete(CustomerRelationship customerRealitonShip)
        {
            await _customerRelationshipDal.Delete(customerRealitonShip);
            return new SuccessResult(CustomerRealitonShipMessages.Deleted);
        }

        //[SecuredAspect()]
        //[CacheAspect()]
        //[PerformanceAspect()]
        public async Task<IDataResult<List<CustomerRelationship>>> GetList()
        {
            return new SuccessDataResult<List<CustomerRelationship>>(await _customerRelationshipDal.GetAll());
        }


        [SecuredAspect()]
        public async Task<IDataResult<CustomerRelationship>> GetById(int id)
        {
            return new SuccessDataResult<CustomerRelationship>(await _customerRelationshipDal.Get(p => p.Id == id));
        }

        [SecuredAspect()]
        public async Task<IDataResult<CustomerRelationship>> GetByCustomerId(int customerId)
        {
            return new SuccessDataResult<CustomerRelationship>(await _customerRelationshipDal.Get(p => p.CustomerId == customerId));
        }
    }
}
