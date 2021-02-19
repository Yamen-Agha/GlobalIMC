using GlobalIMCSPA.APIHandlers;
using GlobalIMCSPA.Attributes;
using GlobalIMCSPA.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalIMCSPA.Models
{
    public class Product
    {
        public Product()
        {
            this.Id = 0;
            this.Image = "";
            this.ViewsNumber = 0;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string VendorId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public IFormFile ImageFF { get; set; }

        public string ImageUrl 
        { 
            get
            {
                return ProductAPIHandler.API_ROOT_URL + this.Image;
            }
        }

        [Required]
        public DietaryFlags DietaryFlag { get; set; }

        [Required]
        public int ViewsNumber { get; set; }
    }
}
