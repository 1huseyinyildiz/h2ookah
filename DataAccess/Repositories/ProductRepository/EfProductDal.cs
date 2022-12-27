using Core.DataAccess.EntityFramework;
using DataAccess.Context.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.ProductRepository
{
    public class EfProductDal : EfEntityRepositoryBase<Product, SimpleContextDb>, IProductDal
    {

        public async Task<List<ProductListDto>> GetList()
        {
            using (var context = new SimpleContextDb())
            {
                var result = from product in context.Products
                             select new ProductListDto
                             {
                                 Id = product.Id,
                                 Name = product.Name,
                                 Gram = product.Stock,
                                 MainImageUrl = (context.ProductImages.Where(p => p.ProductId == product.Id && p.IsMainImage == true).Count() > 0
                                                ? context.ProductImages.Where(p => p.ProductId == product.Id && p.IsMainImage == true).Select(s => s.ImageUrl).FirstOrDefault()
                                                : ""),
                                 Stock = product.Stock,
                                 CategoryId = product.CategoryId,
                                 UpdateDate = product.UpdateDate,
                                 UpdateUser = product.UpdateUser,
                                 IsActive = product.IsActive,
                             };

                return await result.OrderBy(p => p.Name).ToListAsync();
            }
        }

        public async Task<List<ProductListDto>> GetProductList(int CustomerId)
        {
            using (var context = new SimpleContextDb())
            {
                var customerRelationsShip = context.CustomerRelationships.Where(x => x.CustomerId == CustomerId).SingleOrDefault();
                var result = from product in context.Products
                             select new ProductListDto
                             {
                                 Id = product.Id,
                                 Name = product.Name,
                                 Discount = customerRelationsShip.Discount,
                                 Price = context.PriceListDetails.Where(x => x.PriceListId == customerRelationsShip.PriceListId && x.ProductId == product.Id).Count() > 0
                                 ? context.PriceListDetails.Where(x => x.PriceListId == customerRelationsShip.PriceListId && x.ProductId == product.Id).Select(s => s.Price).SingleOrDefault() : 0,
                                 CategoryId = product.CategoryId,
                                 Stock = product.Stock,
                                 Gram = product.Stock,
                                 IsActive = product.IsActive,
                                 UpdateDate = product.UpdateDate,
                                 UpdateUser = product.UpdateUser
                             };



                return await result.OrderBy(p => p.Name).ToListAsync();
            }
        }
    }
}
