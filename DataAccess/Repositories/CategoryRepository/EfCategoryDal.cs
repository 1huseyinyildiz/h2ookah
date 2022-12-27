using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Repositories.CategoryRepository;
using DataAccess.Context.EntityFramework;

namespace DataAccess.Repositories.CategoryRepository
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category, SimpleContextDb>, ICategoryDal
    {
    }
}
