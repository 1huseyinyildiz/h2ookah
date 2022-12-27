using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using Core.Utilities.Result.Abstract;

namespace Business.Repositories.CategoryRepository
{
    public interface ICategoryService
    {
        Task<IResult> Add(Category category);
        Task<IResult> Update(Category category);
        Task<IResult> Delete(Category category);
        Task<IDataResult<List<Category>>> GetList();
        Task<IDataResult<Category>> GetById(int id);
    }
}
