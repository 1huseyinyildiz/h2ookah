using Business.Aspects.Secured;
using Business.Repositories.OrderDetailsRepository.Constants;
using Business.Repositories.OrderDetailsRepository.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Aspects.Validation;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.OrderDetailRepository;
using Entities.Concrete;

namespace Business.Repositories.OrderDetailRepository
{
    public class OrderDetailManager : IOrderDetailService
    {
        private readonly IOrderDetailDal _orderDetailsDal;

        public OrderDetailManager(IOrderDetailDal orderDetailsDal)
        {
            _orderDetailsDal = orderDetailsDal;
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(OrderDetailsValidator))]
        [RemoveCacheAspect("IOrderDetailsService.Get")]

        public async Task<IResult> Add(OrderDetail orderDetails)
        {
            await _orderDetailsDal.Add(orderDetails);
            return new SuccessResult(OrderDetailsMessages.Added);
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(OrderDetailsValidator))]
        [RemoveCacheAspect("IOrderDetailsService.Get")]

        public async Task<IResult> Update(OrderDetail orderDetails)
        {
            await _orderDetailsDal.Update(orderDetails);
            return new SuccessResult(OrderDetailsMessages.Updated);
        }

        //[SecuredAspect()]
        [RemoveCacheAspect("IOrderDetailsService.Get")]

        public async Task<IResult> Delete(OrderDetail orderDetails)
        {
            await _orderDetailsDal.Delete(orderDetails);
            return new SuccessResult(OrderDetailsMessages.Deleted);
        }

        //[SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<OrderDetail>>> GetList(int orderId)
        {
            return new SuccessDataResult<List<OrderDetail>>(await _orderDetailsDal.GetAll(p => p.OrderId == orderId));
        }

        [SecuredAspect()]
        public async Task<IDataResult<OrderDetail>> GetById(int id)
        {
            return new SuccessDataResult<OrderDetail>(await _orderDetailsDal.Get(p => p.Id == id));
        }

        public async Task<List<OrderDetail>> GetListByProductId(int productId)
        {
            return await _orderDetailsDal.GetAll(p => p.ProductId == productId);
        }

        public async Task<List<OrderDetail>> GetListByCategoryId(int categoryId)
        {
            return await _orderDetailsDal.GetAll(p => p.CategoryId == categoryId);
        }
    }
}
