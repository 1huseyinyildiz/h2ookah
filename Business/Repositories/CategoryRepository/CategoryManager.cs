using Business.Repositories.BasketRepository;
using Business.Repositories.CategoryRepository.Constants;
using Business.Repositories.CategoryRepository.Validation;
using Business.Repositories.OrderDetailRepository;
using Business.Repositories.PriceListDetailRepository;
using Business.Repositories.ProductImageRepository;
using Business.Repositories.ProductRepository;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.CategoryRepository;
using DataAccess.Repositories.ProductRepository;
using Entities.Concrete;

namespace Business.Repositories.CategoryRepository
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly IPriceListDetailService _priceListDetailService;
        private readonly IBasketService _basketService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductDal _productDal;


        public CategoryManager(ICategoryDal categoryDal, IProductService productService, IProductImageService productImageService,
            IPriceListDetailService priceListDetailService, IBasketService basketService, IOrderDetailService orderDetailService,
            IProductDal productDal)
        {
            _categoryDal = categoryDal;
            _basketService = basketService;
            _productService = productService;
            _productImageService = productImageService;
            _orderDetailService = orderDetailService;
            _productDal = productDal;
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(CategoryValidator))]
        [RemoveCacheAspect("ICategoryService.Get")]

        public async Task<IResult> Add(Category category)
        {
            await _categoryDal.Add(category);
            return new SuccessResult(CategoryMessages.Added);
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(CategoryValidator))]
        [RemoveCacheAspect("ICategoryService.Get")]

        public async Task<IResult> Update(Category category)
        {
            await _categoryDal.Update(category);
            return new SuccessResult(CategoryMessages.Updated);
        }

        //[SecuredAspect()]
        [RemoveCacheAspect("ICategoryService.Get")]

        public async Task<IResult> Delete(Category category)
        {
            IResult result = BusinessRules.Run(
              await CheckIfCategoryExistToProduct(category.Id));

            if (result != null)
            {
                return result;
            }

            await _categoryDal.Delete(category);
            return new SuccessResult(CategoryMessages.Deleted);
        }

        //[SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<Category>>> GetList()
        {
            return new SuccessDataResult<List<Category>>(await _categoryDal.GetAll());
        }

        //[SecuredAspect()]
        public async Task<IDataResult<Category>> GetById(int id)
        {
            return new SuccessDataResult<Category>(await _categoryDal.Get(p => p.Id == id));
        }

        public async Task<IResult> CheckIfCategoryExistToProduct(int categoryId)
        {
            var result = await _productDal.GetAll(p => p.CategoryId == categoryId);

            if (result.Count > 0)
            {
                return new ErrorResult("Silmeye çalýþtýðýnýz kategorinin ürünü  bulunuyor!!");
            }

            return new SuccessResult();
        }
    }
}
