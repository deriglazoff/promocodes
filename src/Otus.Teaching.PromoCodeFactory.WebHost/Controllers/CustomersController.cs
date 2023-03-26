using System;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {

        private readonly IRepository<Customer> _repository;

        public CustomersController(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var entity = request.Adapt<Customer>();
            var result = await _repository.Create(entity);
            return Ok(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var entity = request.Adapt<Customer>();
            var result = await _repository.UpdateByIdAsync(entity);
            return Ok(result);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            await _repository.RemoveByIdAsync(id);
            return Ok();
        }
    }
}