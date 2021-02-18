using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedEntities
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string VendorId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Image { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile ImageFF { get; set; }

        public byte DietaryFlag { get; set; }

        public int ViewsNumber { get; set; }
    }
}
