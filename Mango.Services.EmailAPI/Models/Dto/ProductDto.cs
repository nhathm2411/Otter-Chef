﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Services.EmailAPI.Models.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
        [Range(1,100)]
        public int Count { get; set; } = 1;
    }
}
