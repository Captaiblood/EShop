using Catalog.API.Dto.Property;
using Catalog.API.ServiceResponder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Catalog.API.Services.Contract
{
   public interface IProductService
    {
        /// <summary>
        /// Return list of products 
        /// </summary>
        /// <returns>List Of ProductDto</returns>
        Task<ServiceResponse<List<ProductDto>>> GetProductsAsync();
        /// <summary>
        /// Return Product record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>ProductDto</returns>
        Task<ServiceResponse<ProductDto>> GetProductByIdAsync(string Id);
       /// <summary>
       /// Return product by catogery
       /// </summary>
       /// <param name="catogery"></param>
       /// <returns></returns>
        Task<ServiceResponse<ProductDto>> GetProductByCategoryAsync(string catogery);

        /// <summary>
        /// Add new product record in db
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns>ProductDto</returns>
        Task<ServiceResponse<ProductDto>> CreateProductAsync(CreateProductDto createProductDto);
        /// <summary>
        /// Update product record
        /// </summary>
        /// <param name="updateProductDto"></param>
        /// <returns>ProductDto</returns>
        Task<ServiceResponse<ProductDto>> UpdatePrpductAsync(UpdateProductDto updateProductDto);


        Task<ServiceResponse<string>> DeleteProductAsync(string Id);
    }
}
