using GlobalIMCAPI.Services.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalIMCAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _ProductService;

        public ProductController(IProductService productService)
        {
            this._ProductService = productService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            ServiceResponse<IEnumerable<ProductDTO>> Result = await this._ProductService.GetAllProducts();
            return ValidateAction(Result);
            
        }

        [HttpGet("IsVendorExisits/{id}/{vendorId}")]
        public async Task<IActionResult> IsVendorExisits(int Id,string VendorId)
        {
            ServiceResponse<bool> Result = await this._ProductService.IsVendorExisits(Id, VendorId);
            return ValidateAction(Result);
        }

        [HttpGet("Search/{searchText}")]
        public async Task<IActionResult> Search(string SearchText)
        {
            ServiceResponse<IEnumerable<ProductDTO>> Result = await this._ProductService.Search(SearchText);
            return ValidateAction(Result);

        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] ProductDTO NewProduct)
        {
            var FilesPath = Directory.GetCurrentDirectory() + "/images";
            string FileName = DateTime.Now.Ticks + Path.GetExtension(NewProduct.ImageFF.FileName);
            var FilePath = Path.Combine(FilesPath, FileName);
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                await NewProduct.ImageFF.CopyToAsync(stream);
            }
            NewProduct.Image = Path.GetFullPath(FilePath);
            
            ServiceResponse<int> Result = await this._ProductService.Create(NewProduct);
            return ValidateAction(Result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            string OldImagePath = (await this._ProductService.Get(Id, false)).Data.Image;

            ServiceResponse<bool> Result = await this._ProductService.Delete(Id);

            if (Result.Success)
                System.IO.File.Delete(OldImagePath);

            return ValidateAction(Result);
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult> Get(int Id)
        {
            ServiceResponse<ProductDTO> Result = await this._ProductService.Get(Id, false);
            return ValidateAction(Result);
        }

        [HttpGet("GetIncrease/{id}")]
        public async Task<ActionResult> GetWithIncreseView(int Id)
        {
            ServiceResponse<ProductDTO> Result = await this._ProductService.Get(Id, true);
            return ValidateAction(Result);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromForm] ProductDTO ProductToEdit)
        {
            var FilesPath = Directory.GetCurrentDirectory() + "/images";
            string FileName = DateTime.Now.Ticks + Path.GetExtension(ProductToEdit.ImageFF.FileName);
            var FilePath = Path.Combine(FilesPath, FileName);
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                await ProductToEdit.ImageFF.CopyToAsync(stream);
            }
            ProductToEdit.Image = Path.GetFullPath(FilePath);

            string OldImagePath = (await this._ProductService.Get(ProductToEdit.Id, false)).Data.Image;

            ServiceResponse<bool> Result = await this._ProductService.Update(ProductToEdit);

            if (Result.Success)
                System.IO.File.Delete(OldImagePath);

            return ValidateAction(Result);
        }

        private ActionResult ValidateAction<T>(ServiceResponse<T> Response)
        {
            if (Response.Success)
                return Ok(Response);
            else
                return BadRequest(Response);
        }

    }
}
