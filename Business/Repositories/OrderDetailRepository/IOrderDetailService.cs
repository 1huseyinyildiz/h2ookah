using Core.Utilities.Result.Abstract;
using Entities.Concrete;

namespace Business.Repositories.OrderDetailRepository
{
    public interface IOrderDetailService
    {
        Task<IResult> Add(OrderDetail orderDetails);
        Task<IResult> Update(OrderDetail orderDetails);
        Task<IResult> Delete(OrderDetail orderDetails);
        Task<IDataResult<List<OrderDetail>>> GetList(int orderId);
        Task<List<OrderDetail>> GetListByProductId(int productId);
        Task<List<OrderDetail>> GetListByCategoryId(int categoryId);
        Task<IDataResult<OrderDetail>> GetById(int id);
    }
}
