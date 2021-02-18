using AutoMapper;
using GlobalIMCSPA.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GlobalIMCSPA.APIHandlers
{
    public class ProductAPIHandler : IProductAPIHandler
    {
        private const string PRODUCT_API_ROOT_URL = "https://localhost:44337/Product";

        private const string GET_ALL_URL = PRODUCT_API_ROOT_URL + "/GetAll";
        private const string IS_VENDOR_EXISITS_URL = PRODUCT_API_ROOT_URL + "/IsVendorExisits";
        private const string SEARCH_URL = PRODUCT_API_ROOT_URL + "/Search";
        private const string DELETE_URL = PRODUCT_API_ROOT_URL + "/Delete";
        private const string GET_URL = PRODUCT_API_ROOT_URL + "/Get";
        private const string GET_WITH_INCREASE_URL = PRODUCT_API_ROOT_URL + "/GetIncrease";
        private const string UPDATE_URL = PRODUCT_API_ROOT_URL + "/Update";

        private IMapper _Mapper;

        public ProductAPIHandler(IMapper Mapper)
        {
            this._Mapper = Mapper;
        }

        public async Task<List<Product>> GetAll()
        {
            try
            {
                using(HttpClient Client = new HttpClient())
                {
                    using(HttpResponseMessage Res = await Client.GetAsync(GET_ALL_URL))
                    {
                        if (Res.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            using (HttpContent Content = Res.Content)
                            {
                                string Data = await Content.ReadAsStringAsync();
                                ServiceResponse<List<ProductDTO>> APIResponse =
                                    JsonConvert.DeserializeObject<ServiceResponse<List<ProductDTO>>>(Data);

                                if (!APIResponse.Success) return null;
                                return APIResponse.Data.Select(P => this._Mapper.Map<Product>(P)).ToList();
                            }
                        }
                        else
                        {
                            return null;
                        }

                        
                    }
                }

            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Product>> Search(string SearchText)
        {
            try
            {
                using (HttpClient Client = new HttpClient())
                {
                    Uri URL = new Uri($"{SEARCH_URL}/{SearchText}");
                    using (HttpResponseMessage Res = await Client.GetAsync(URL))
                    {
                        if (Res.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            using (HttpContent Content = Res.Content)
                            {
                                string Data = await Content.ReadAsStringAsync();
                                ServiceResponse<List<ProductDTO>> APIResponse =
                                    JsonConvert.DeserializeObject<ServiceResponse<List<ProductDTO>>>(Data);

                                if (!APIResponse.Success) return null;
                                return APIResponse.Data.Select(P => this._Mapper.Map<Product>(P)).ToList();
                            }
                        }
                        else
                        {
                            return null;
                        }


                    }
                }

            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> IsVendorExisits(int Id, string VendorId)
        {
            using (HttpClient Client = new HttpClient())
            {
                Uri URL = new Uri($"{IS_VENDOR_EXISITS_URL}/{Id}/{VendorId}");
                using (HttpResponseMessage Res = await Client.GetAsync(URL))
                {
                    if (Res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        using (HttpContent Content = Res.Content)
                        {
                            string Data = await Content.ReadAsStringAsync();
                            ServiceResponse<bool> APIResponse =
                                JsonConvert.DeserializeObject<ServiceResponse<bool>>(Data);

                            if (!APIResponse.Success) throw new Exception();
                            return APIResponse.Data;
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }


                }
            }
        }

        public async Task<Product> Get(int Id)
        {
            using (HttpClient Client = new HttpClient())
            {

                Uri URL = new Uri($"{GET_URL}/{Id}");
                using (HttpResponseMessage Res = await Client.GetAsync(URL))
                {
                    if (Res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        using (HttpContent Content = Res.Content)
                        {
                            string Data = await Content.ReadAsStringAsync();
                            ServiceResponse<ProductDTO> APIResponse =
                                JsonConvert.DeserializeObject<ServiceResponse<ProductDTO>>(Data);

                            if (!APIResponse.Success) return null;
                            
                            return this._Mapper.Map<Product>(APIResponse.Data);
                        }
                    }
                    else
                    {
                        return null;
                    }


                }
            }
        }

        public async Task<Product> GetWithIncreaseView(int Id)
        {
            using (HttpClient Client = new HttpClient())
            {
                Uri URL = new Uri($"{GET_WITH_INCREASE_URL}/{Id}");
                using (HttpResponseMessage Res = await Client.GetAsync(URL))
                {
                    if (Res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        using (HttpContent Content = Res.Content)
                        {
                            string Data = await Content.ReadAsStringAsync();
                            ServiceResponse<ProductDTO> APIResponse =
                                JsonConvert.DeserializeObject<ServiceResponse<ProductDTO>>(Data);

                            if (!APIResponse.Success) return null;

                            return this._Mapper.Map<Product>(APIResponse.Data);
                        }
                    }
                    else
                    {
                        return null;
                    }


                }
            }
        }

        public async Task<bool> Delete(int Id)
        {
            using (HttpClient Client = new HttpClient())
            {
                Uri URL = new Uri($"{DELETE_URL}/{Id}");
                using (HttpResponseMessage Res = await Client.DeleteAsync(URL))
                {
                    if (Res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        using (HttpContent Content = Res.Content)
                        {
                            string Data = await Content.ReadAsStringAsync();
                            ServiceResponse<bool> APIResponse =
                                JsonConvert.DeserializeObject<ServiceResponse<bool>>(Data);

                            if (!APIResponse.Success) return false;

                            return APIResponse.Data;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public async Task<int> Create(Product product)
        {
            ProductDTO NewProduct = this._Mapper.Map<ProductDTO>(product);
            using (HttpClient Client = new HttpClient())
            {
                Uri URL = new Uri($"{PRODUCT_API_ROOT_URL}");


                var multipartContent = new MultipartFormDataContent();

                multipartContent.Add(new StringContent(NewProduct.Id.ToString()), "Id");
                multipartContent.Add(new StringContent(NewProduct.VendorId), "VendorId");
                multipartContent.Add(new StringContent(NewProduct.Title), "Title");
                multipartContent.Add(new StringContent(NewProduct.Description), "Description");
                multipartContent.Add(new StringContent(NewProduct.Price.ToString()), "Price");
                multipartContent.Add(new StringContent(NewProduct.Image), "Image");
                multipartContent.Add(new StringContent(NewProduct.DietaryFlag.ToString()), "DietaryFlag");
                multipartContent.Add(new StringContent(NewProduct.ViewsNumber.ToString()), "ViewsNumber");

                byte[] data;
                using (var br = new BinaryReader(NewProduct.ImageFF.OpenReadStream()))
                {
                    data = br.ReadBytes((int)NewProduct.ImageFF.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                multipartContent.Add(bytes, "ImageFF", NewProduct.ImageFF.FileName);

                using (HttpResponseMessage Res = await Client.PostAsync(URL, multipartContent))
                {
                    if (Res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        using (HttpContent Content = Res.Content)
                        {
                            string Data = await Content.ReadAsStringAsync();
                            ServiceResponse<int> APIResponse =
                                JsonConvert.DeserializeObject<ServiceResponse<int>>(Data);

                            if (!APIResponse.Success) return -1;

                            return APIResponse.Data;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

        public async Task<bool> Update(Product product)
        {
            ProductDTO ProductToEdit = this._Mapper.Map<ProductDTO>(product);
            using (HttpClient Client = new HttpClient())
            {
                Uri URL = new Uri($"{PRODUCT_API_ROOT_URL}");

                var multipartContent = new MultipartFormDataContent();

                multipartContent.Add(new StringContent(ProductToEdit.Id.ToString()), "Id");
                multipartContent.Add(new StringContent(ProductToEdit.VendorId), "VendorId");
                multipartContent.Add(new StringContent(ProductToEdit.Title), "Title");
                multipartContent.Add(new StringContent(ProductToEdit.Description), "Description");
                multipartContent.Add(new StringContent(ProductToEdit.Price.ToString()), "Price");
                multipartContent.Add(new StringContent(ProductToEdit.Image), "Image");
                multipartContent.Add(new StringContent(ProductToEdit.DietaryFlag.ToString()), "DietaryFlag");
                multipartContent.Add(new StringContent(ProductToEdit.ViewsNumber.ToString()), "ViewsNumber");

                byte[] data;
                using (var br = new BinaryReader(ProductToEdit.ImageFF.OpenReadStream()))
                {
                    data = br.ReadBytes((int)ProductToEdit.ImageFF.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                multipartContent.Add(bytes, "ImageFF", ProductToEdit.ImageFF.FileName);

                using (HttpResponseMessage Res = await Client.PutAsync(URL, multipartContent))
                {
                    if (Res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        using (HttpContent Content = Res.Content)
                        {
                            string Data = await Content.ReadAsStringAsync();
                            ServiceResponse<bool> APIResponse =
                                JsonConvert.DeserializeObject<ServiceResponse<bool>>(Data);

                            if (!APIResponse.Success) return false;

                            return APIResponse.Data;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
