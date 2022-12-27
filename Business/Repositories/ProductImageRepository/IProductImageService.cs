using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Repositories.ProductImageRepository
{
    public interface IProductImageService
    {
        Task<IResult> Add(ProductImageAddDto productImageAddDto);
        Task<IResult> Update(ProductImageUpdateDto productImageUpdateDto);
        Task<IResult> SetMainImage(int Id);
        Task<IResult> Delete(ProductImage productImage);
        Task<IDataResult<List<ProductImage>>> GetList();
        Task<IDataResult<ProductImage>> GetById(int id);
        Task<IDataResult<List<ProductImage>>> GetListByProductId(int productId);
        Task<IDataResult<List<ProductImage>>> GetListProductId(int productId);
    }
}
