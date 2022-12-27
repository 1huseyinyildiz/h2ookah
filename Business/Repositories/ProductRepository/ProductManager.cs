using Business.Aspects.Secured;
using Business.Repositories.BasketRepository;
using Business.Repositories.OrderDetailRepository;
using Business.Repositories.PriceListDetailRepository;
using Business.Repositories.ProductImageRepository;
using Business.Repositories.ProductRepository.Constants;
using Business.Repositories.ProductRepository.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductRepository;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Repositories.ProductRepository
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IProductImageService _productImageService;
        private readonly IPriceListDetailService _priceListDetailService;
        private readonly IBasketService _basketService;
        private readonly IOrderDetailService _orderDetailService;

        public ProductManager(IProductDal productDal, IProductImageService productImageService, IPriceListDetailService priceListDetailService, IBasketService basketService, IOrderDetailService orderDetailService = null)
        {
            _productDal = productDal;
            _productImageService = productImageService;
            _priceListDetailService = priceListDetailService;
            _basketService = basketService;
            _orderDetailService = orderDetailService;
        }

        //[SecuredAspect("admin,product.add")]
        [ValidationAspect(typeof(ProductValidator))]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> Add(Product product)
        {
            await _productDal.Add(product);
            return new SuccessResult(ProductMessages.Added);
        }

        [SecuredAspect("Admin,product.update")]
        [ValidationAspect(typeof(ProductValidator))]
        [RemoveCacheAspect("IProductService.Get")]

        public async Task<IResult> Update(Product product)
        {
            await _productDal.Update(product);
            return new SuccessResult(ProductMessages.Updated);
        }

        //[SecuredAspect("admin,product.delete")]
        [RemoveCacheAspect("IProductService.Get")]

        public async Task<IResult> Delete(int productId)
        {
            IResult result = BusinessRules.Run(
                await CheckIfProductExistToBaskets(productId),
                await CheckIfProductExistToOrderDetail(productId));

            if (result != null)
            {
                return result;
            }

            Product _product = await _productDal.Get(p => p.Id == productId);

            if (_product != null)
            {
                var images = await _productImageService.GetListProductId(productId);
                foreach (var item in images.Data)
                {
                    await _productImageService.Delete(item);
                }

                var priceListProduct = await _priceListDetailService.GetListByProductId(productId);

                foreach (var priceList in priceListProduct)
                {
                    await _priceListDetailService.Delete(priceList);
                }

                await _productDal.Delete(_product);
                return new SuccessResult(ProductMessages.Deleted);
            }

            return new ErrorResult(ProductMessages.Error);
        }

        //[SecuredAspect("admin,product.get")]
        //[CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductListDto>>> GetList()
        {
            return new SuccessDataResult<List<ProductListDto>>(await _productDal.GetList());
        }

        [SecuredAspect("Admin,product.getlistbyid")]
        public async Task<IDataResult<Product>> GetById(int id)
        {
            return new SuccessDataResult<Product>(await _productDal.Get(p => p.Id == id));
        }

        public async Task<IDataResult<List<ProductListDto>>> GetProductList(int customerId)
        {
            return new SuccessDataResult<List<ProductListDto>>(await _productDal.GetProductList(customerId));
        }

        public async Task<IResult> CheckIfProductExistToBaskets(int productId)
        {
            var result = await _basketService.GetListByProductId(productId);

            if (result.Count > 0)
            {
                return new ErrorResult("Silmeye çalýþtýðýnýz ürün sepette bulunuyor!!");
            }

            return new SuccessResult();
        }

        public async Task<IResult> CheckIfProductExistToOrderDetail(int productId)
        {
            var result = await _orderDetailService.GetListByProductId(productId);

            if (result.Count > 0)
            {
                return new ErrorResult("Silmeye çalýþtýðýnýz ürün sipariþi bulunuyor!!");
            }

            return new SuccessResult();
        }


        public async Task<IDataResult<Product>> GetCategoryIncludeProductList(int categoryId)
        {
            return new SuccessDataResult<Product>(await _productDal.Get(x => x.CategoryId == categoryId));
        }
    }
}
