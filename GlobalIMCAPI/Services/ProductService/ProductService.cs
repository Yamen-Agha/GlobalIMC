using AutoMapper;
using GlobalIMCAPI.Data;
using GlobalIMCAPI.Enums;
using Microsoft.EntityFrameworkCore;
using SharedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalIMCAPI.Extensions;

namespace GlobalIMCAPI.Services.ProductService
{
    public class ProductService : IProductService
    {
        private DataContext _DB;
        private IMapper _Mapper;

        public ProductService(DataContext DB, IMapper Mapper)
        {
            this._DB = DB;
            this._Mapper = Mapper;
        }

        public async Task<ServiceResponse<int>> Create(ProductDTO NewProduct)
        {
            string ErrorMessage = "";
            try
            {
                if((await this.IsVendorExisits(-1,NewProduct.VendorId)).Data)
                {
                    ErrorMessage = "Vendor already Exisits .";
                }
                else
                {
                    Product ProductModel = this._Mapper.Map<Product>(NewProduct);
                    await this._DB.Products.AddAsync(ProductModel);
                    await this._DB.SaveChangesAsync();
                    return new ServiceResponse<int>(ProductModel.Id);
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return new ServiceResponse<int>(-1)
            {
                Success = false,
                Message = ErrorMessage
            };
        }

        public async Task<ServiceResponse<bool>> Delete(int Id)
        {
            string ErrorMessage = "";
            try
            {
                Product ProductToDelete = await this._DB.Products.FindAsync(Id);
                if(ProductToDelete != null)
                {
                    this._DB.Products.Remove(ProductToDelete);
                    await this._DB.SaveChangesAsync();
                    return new ServiceResponse<bool>(true);
                }
                else
                {
                    ErrorMessage = "Invalid Id .";
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return new ServiceResponse<bool>(false)
            {
                Success = false,
                Message = ErrorMessage
            };
        }

        public async Task<ServiceResponse<ProductDTO>> Get(int Id, bool IncreaseViews)
        {
            string ErrorMessage = "";
            try
            {
                Product ProductToGet = await this._DB.Products.FindAsync(Id);
                if (ProductToGet != null)
                {
                    if(IncreaseViews)
                    {
                        ProductToGet.ViewsNumber += 1;
                        this._DB.Products.Update(ProductToGet);
                        await this._DB.SaveChangesAsync();
                    }
                    return new ServiceResponse<ProductDTO>(this._Mapper.Map<ProductDTO>(ProductToGet));
                }
                else
                {
                    ErrorMessage = "Invalid Id .";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return new ServiceResponse<ProductDTO>(null)
            {
                Success = false,
                Message = ErrorMessage
            };
        }

        public async Task<ServiceResponse<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            try
            {
                List<ProductDTO> Result = await this._DB.Products.Select(P => this._Mapper.Map<ProductDTO>(P)).ToListAsync();
                return new ServiceResponse<IEnumerable<ProductDTO>>(Result);
            }
            catch(Exception ex)
            {
                return new ServiceResponse<IEnumerable<ProductDTO>>(null)
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> IsVendorExisits(int Id, string VendorId)
        {
            string ErrorMessage = "";
            try
            {
                if(string.IsNullOrWhiteSpace(VendorId))
                {
                    ErrorMessage = "Invalid Vendor .";
                }
                else
                {
                    string TrimedVendor = VendorId.Trim();
                    return new ServiceResponse<bool>(await this._DB.Products.AnyAsync(P => P.VendorId.Equals(TrimedVendor) && P.Id != Id));
                }
                
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return new ServiceResponse<bool>(false)
            {
                Success = false,
                Message = ErrorMessage
            };

        }

        public async Task<ServiceResponse<IEnumerable<ProductDTO>>> Search(string SearchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText)) 
                    return await GetAllProducts();

                var Words = SearchText.Split();

                List<Product> Result = this._DB.Products.AsEnumerable().Where(P =>
                                        Words.ContainsOrStartsWithAny( P.Title) ||
                                        Words.ContainsOrStartsWithAny( P.Description)).ToList();

                return new ServiceResponse<IEnumerable<ProductDTO>>(Result.Select(PS => this._Mapper.Map<ProductDTO>(PS)));
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<ProductDTO>>(null)
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> Update(ProductDTO EditedProduct)
        {
            string ErrorMessage = "";
            try
            {
                Product ProductToGet = await this._DB.Products.FindAsync(EditedProduct.Id);
                if (ProductToGet != null)
                {
                    //Validate Unique Vendor double check
                    if ((await this.IsVendorExisits(EditedProduct.Id, EditedProduct.VendorId)).Data)
                    {
                        ErrorMessage = "Vendor already Exisits .";
                    }
                    else
                    {
                        ProductToGet.Description = EditedProduct.Description;
                        ProductToGet.DietaryFlag = (DietaryFlags)EditedProduct.DietaryFlag;
                        ProductToGet.Image = EditedProduct.Image;
                        ProductToGet.Price = EditedProduct.Price;
                        ProductToGet.Title = EditedProduct.Title;
                        ProductToGet.VendorId = EditedProduct.VendorId;

                        this._DB.Products.Update(ProductToGet);
                        await this._DB.SaveChangesAsync();

                        return new ServiceResponse<bool>(true);
                    }
                }
                else
                {
                    ErrorMessage = "Invalid Id .";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return new ServiceResponse<bool>(false)
            {
                Success = false,
                Message = ErrorMessage
            };
        }
    }
}