using AutoMapper;
using Catalog.API.Dto.Property;
using Catalog.API.Repository;
using Catalog.API.Repository.Contract;
using Catalog.API.Services.Contract;
using Catalog.API.ServiceResponder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Entities;
using System.Xml.Linq;

namespace Catalog.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        //private readonly IMessageBus _messageBus;

        //public PropertyService(IPropertyRespository compRepo, IMapper mapper, IMessageBus messageBus)
        //{
        //    _propertyRepo = compRepo;
        //    _mapper = mapper;
        //    _messageBus = messageBus;
        //}

        public ProductService(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo?? throw new ArgumentNullException(nameof(productRepo));
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));

        }

        /// <summary>
        /// Add new Property record in db
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns>ProductDto</returns>
        public async Task<ServiceResponse<ProductDto>> CreateProductAsync(CreateProductDto createProductDto)
        {

            ServiceResponse<ProductDto> _response = new();

            try
            {
                var _propertyExist = await _productRepo.GetProductByName(createProductDto.Name);

                if (_propertyExist.Any())
                {

                    _response.Success = false;
                    _response.Message = "Exist";
                    _response.Data = null;
                    return _response;
                }

                var _product = new Product
                {

                    Name = createProductDto.Name,
                    Description = createProductDto.Description,
                    Category = createProductDto.Category,
                    Summary = createProductDto.Summary,
                    ImageFile = createProductDto.ImageFile,
                    Price = createProductDto.Price,

                };


                if (!await _productRepo.CreateProduct(_product))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<ProductDto>(_product);
                _response.Message = "Created";

                ////new property created send a message to bus.
                //MessageProductDto message = new();
                //message = _mapper.Map<MessageProductDto>(_Property);                
                //await _messageBus.PublishMessage(message, "PropertyAddedTopic"); // Shuld be moved to appsettings

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Error = ex.Message.ToString();
            }
            return _response;

        }

        /// <summary>
        /// Return list of Properties which are not marked as deleted.
        /// </summary>
        /// <returns>List Of ProductDto</returns>
        public async Task<ServiceResponse<List<ProductDto>>> GetProductsAsync()
        {

            ServiceResponse<List<ProductDto>> _response = new();

            try
            {

                var productList = await _productRepo.GetProducts();

                var productListDto = new List<ProductDto>();

                foreach (var item in productList)
                {
                    productListDto.Add(_mapper.Map<ProductDto>(item));
                }

                //OR 
                //PropertyListDto.AddRange(from item in PropertiesList select _mapper.Map<ProductDto>(item));
                _response.Success = true;
                _response.Message = "ok";
                _response.Data = productListDto;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Error = ex.Message.ToString();
            }

            return _response;


        }





        /// <summary>
        /// Return Property record.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>ProductDto</returns>
        public async Task<ServiceResponse<ProductDto>> GetProductByIdAsync(string Id)
        {

            ServiceResponse<ProductDto> _response = new();

            try
            {


                var product = await _productRepo.GetProduct(Id);

                if (product == null)
                {

                    _response.Success = false;
                    _response.Message = "Not Found";
                    return _response;
                }

                var _productDto = _mapper.Map<ProductDto>(product);

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = _productDto;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Error = ex.Message.ToString();
            }

            return _response;

        }

        public async Task<ServiceResponse<ProductDto>> GetProductByCategoryAsync(string catogery)
        {
            ServiceResponse<ProductDto> _response = new();

            try
            {


                var product = await _productRepo.GetProductByCategory(catogery);

                if (product == null)
                {

                    _response.Success = false;
                    _response.Message = "Not Found";
                    return _response;
                }

                var _productDto = _mapper.Map<ProductDto>(product);

                _response.Success = true;
                _response.Message = "ok";
                _response.Data = _productDto;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Error = ex.Message.ToString();
            }

            return _response;

        }

        /// <summary>
        /// Permanently delete Property from DB
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Bool</returns>
        public async Task<ServiceResponse<string>> DeleteProductAsync(string Id)
        {

            ServiceResponse<string> _response = new();
            try
            {
                var _existingProduct = await _productRepo.GetProduct(Id);

                if (_existingProduct == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }

                // var _existingProperty = _mapper.Map<Entites.Property>(ProductDto);

                //we can use check other future proffing if Property can be permapenetly removed.

                if (!await _productRepo.DeleteProduct(_existingProduct.Id))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    return _response;
                }

                _response.Success = true;
                _response.Message = "Deleted";
            }
            catch (Exception ex)
            {

                _response.Success = false;
                _response.Message = "Error";
                _response.Error = ex.Message.ToString();
            }
            return _response;
        }





        /// <summary>
        /// Update Property record
        /// </summary>
        /// <param name="updateProductDto"></param>
        /// <returns>ProductDto</returns>
        public async Task<ServiceResponse<ProductDto>> UpdatePrpductAsync(UpdateProductDto updateProductDto)
        {


            ServiceResponse<ProductDto> _response = new();
            try
            {
                var _existingProduct = await _productRepo.GetProduct(updateProductDto.Id);

                if (_existingProduct == null)
                {

                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;

                }


                // var _Property = _mapper.Map<Entites.Property>(updateProductDto);


                _existingProduct.Name = updateProductDto.Name;
                _existingProduct.Description = updateProductDto.Description;
                _existingProduct.Category = updateProductDto.Category;
                _existingProduct.Summary = updateProductDto.Summary;
                _existingProduct.ImageFile = updateProductDto.ImageFile;
                _existingProduct.Price = updateProductDto.Price;




                if (!await _productRepo.UpdateProduct(_existingProduct))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _ProductDto = _mapper.Map<ProductDto>(_existingProduct);
                _response.Success = true;
                _response.Message = "Updated";
                _response.Data = _ProductDto;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Error = ex.Message.ToString();
            }

            return _response;


        }




        

    } 

}
