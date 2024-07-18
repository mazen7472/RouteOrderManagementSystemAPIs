﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Core.DTOs;
using OrderManagementSystem.Core.Entites;
using OrderManagementSystem.Core.Repositories;
using OrderManagementSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepo productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Product> AddProductAsync(ProductCreateDTO productCreateDto)
        {
            if (productCreateDto == null)
            {
                throw new ArgumentException("no Product");
            }

            var product = new Product
            {

                Name = productCreateDto.Name,
                Price = productCreateDto.Price,
                Stock = productCreateDto.Stock,

            };
            await _productRepository.AddProductAsync(product);
            await _productRepository.SaveChangesAsync();
            return product;
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetProductsAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> UpdateProductAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _productRepository.UpdateProductAsync(product);
            await _productRepository.SaveChangesAsync();
            return _mapper.Map<ProductDTO>(product);
        }



    }
}
