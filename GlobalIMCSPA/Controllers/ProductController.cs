using GlobalIMCSPA.APIHandlers;
using GlobalIMCSPA.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GlobalIMCSPA.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductAPIHandler _ProductHandler;
        

        public ProductController(IProductAPIHandler ProductHandler)
        {
            this._ProductHandler = ProductHandler;
            
        }

        // GET: ProductController
        public async Task<ActionResult> Index(string SearchText)
        {
            List<Product> Products = null;

            if (SearchText == null)
                Products = await this._ProductHandler.GetAll();
            else
                Products = await this._ProductHandler.Search(SearchText);

            if (Products == null)
                return BadRequest();

            return View(Products);
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if(id == null)
                return BadRequest();

            Product product = await this._ProductHandler.GetWithIncreaseView((int)id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View(new Product());
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product collection)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    int i = await this._ProductHandler.Create(collection);

                    //collection
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            Product product = await this._ProductHandler.Get((int)id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Product collection)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await this._ProductHandler.Update(collection);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }

            
        }

        // GET: ProductController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            Product product = await this._ProductHandler.Get((int)id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                bool IsDeleted = await this._ProductHandler.Delete(id);
                if (IsDeleted)
                    return RedirectToAction(nameof(Index));
                else
                    return BadRequest();
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> IsVendorExisits(int id,string vendorId)
        {
            return Json(await this._ProductHandler.IsVendorExisits(id, vendorId));
        }
    }
}
