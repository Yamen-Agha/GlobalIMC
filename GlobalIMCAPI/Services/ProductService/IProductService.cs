using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalIMCAPI.Data;
using SharedEntities;

namespace GlobalIMCAPI.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<IEnumerable<ProductDTO>>> GetAllProducts();

        Task<ServiceResponse<IEnumerable<ProductDTO>>> Search(string SearchText);

        Task<ServiceResponse<ProductDTO>> Get(int Id, bool IncreaseViews);

        Task<ServiceResponse<int>> Create(ProductDTO NewProduct);

        Task<ServiceResponse<bool>> Update(ProductDTO EditedProduct);

        Task<ServiceResponse<bool>> Delete(int Id);

        Task<ServiceResponse<bool>> IsVendorExisits(int Id, string VendorId);
    }
}
