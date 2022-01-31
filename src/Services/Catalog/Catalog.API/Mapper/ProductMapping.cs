using AutoMapper;
using Catalog.API.Dto.Property;
using Catalog.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Mapper
{
    public class ProductMapping:Profile
    {

        public ProductMapping()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
           

        }
    }
}
