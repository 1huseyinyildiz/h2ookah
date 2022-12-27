using Business.Aspects.Secured;
using Business.Repositories.PriceListDetailRepository;
using Business.Repositories.PriceListRepository.Constants;
using Business.Repositories.PriceListRepository.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.PriceListRepository;
using Entities.Concrete;

namespace Business.Repositories.PriceListRepository
{
    public class PriceListManager : IPriceListService
    {
        private readonly IPriceListDal _priceListDal;
        private readonly IPriceListDetailService _priceListDetailService;

        public PriceListManager(IPriceListDal priceListDal, IPriceListDetailService priceListDetailService)
        {
            _priceListDal = priceListDal;
            _priceListDetailService = priceListDetailService;
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(PriceListValidator))]
        [RemoveCacheAspect("IPriceListService.Get")]

        public async Task<IResult> Add(PriceList priceList)
        {
            await _priceListDal.Add(priceList);
            return new SuccessResult(PriceListMessages.Added);
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(PriceListValidator))]
        [RemoveCacheAspect("IPriceListService.Get")]

        public async Task<IResult> Update(PriceList priceList)
        {
            await _priceListDal.Update(priceList);
            return new SuccessResult(PriceListMessages.Updated);
        }

        //[SecuredAspect()]
        //[RemoveCacheAspect("IPriceListService.Get")]

        public async Task<IResult> Delete(PriceList priceList)
        {

            var priceListDetail = await _priceListDetailService.GetById(priceList.Id);

            IResult result = BusinessRules.Run(
             await CheckIfProductExistToPriceListDetail(priceListDetail.Data.ProductId),
             await CheckIfPriceListExistToPriceListDetail(priceListDetail.Data.PriceListId));

            if (result != null)
            {
                return result;
            }

            await _priceListDal.Delete(priceList);
            return new SuccessResult(PriceListMessages.Deleted);
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<PriceList>>> GetList()
        {
            return new SuccessDataResult<List<PriceList>>(await _priceListDal.GetAll());
        }

        [SecuredAspect()]
        public async Task<IDataResult<PriceList>> GetById(int id)
        {
            return new SuccessDataResult<PriceList>(await _priceListDal.Get(p => p.Id == id));
        }

        public async Task<IResult> CheckIfProductExistToPriceListDetail(int productId)
        {
            var result = await _priceListDetailService.GetListByProductId(productId);

            if (result.Count > 0)
            {
                return new ErrorResult("Silmeye çalýþtýðýnýz ürünü bulunuyor!!");
            }

            return new SuccessResult();
        }

        public async Task<IResult> CheckIfPriceListExistToPriceListDetail(int priceListId)
        {
            var result = await _priceListDetailService.GetListPriceListId(priceListId);

            if (result.Count > 0)
            {
                return new ErrorResult("Silmeye çalýþtýðýnýz ürünün fiyat listesi bulunuyor!!");
            }

            return new SuccessResult();
        }

    }
}
