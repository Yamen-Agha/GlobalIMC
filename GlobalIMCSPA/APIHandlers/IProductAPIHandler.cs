using GlobalIMCSPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalIMCSPA.APIHandlers
{
    public interface IProductAPIHandler
    {
        Task<List<Product>> GetAll();
        Task<Product> Get(int Id);
        Task<Product> GetWithIncreaseView(int Id);
        Task<bool> Delete(int Id);
        Task<bool> IsVendorExisits(int id, string VendorId);
        Task<List<Product>> Search(string SearchText);
        Task<int> Create(Product NewProduct);

        Task<bool> Update(Product ProductToEdit);
    }
}
