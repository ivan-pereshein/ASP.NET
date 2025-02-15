using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository) 
        { 
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            //TODO: Добавить получение списка клиентов

            var customers = await _customerRepository.GetAllAsync();            
            var response = customers.Select(c => new CustomerShortResponse { Email = c.Email, FirstName = c.FirstName, LastName = c.LastName, Id = c.Id });
            return Ok(response);
        }

        /// <summary>
        /// Gets Preferences of the customer.
        /// </summary>
        /// <param name="id">Customer Id.</param>
        /// <returns>List of preferences.</returns>
        [HttpGet("Preferences/{id}")]
        public async Task<ActionResult<PreferenceResponse>> GetPreferenceCustomersAsync(Guid id)
        {
            Customer customer = await _customerRepository.GetByIdIncludePreferencesAsync(id);
            IEnumerable<PreferenceResponse> response = customer.CustomerPreferences.Select(cp => new PreferenceResponse { Id = cp.PreferenceId, Name = cp.Preference.Name });
            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            CustomerResponse response = new CustomerResponse { Id = customer.Id, Email = customer.Email, FirstName = customer.FirstName, LastName = customer.LastName };
            return Ok(response);
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="request">Attributes of a new customer.</param>
        /// <returns>Id of a new customer</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            // В идеале маппинг ДТО в entity наверное надо делать где-то в Application layer.
            // Контроллер должен иметь ссылку на какой-нибудь IServive или IRequest/MediatR
            
            Customer customer = new Customer 
            {   
                Id = Guid.NewGuid(),
                Email = request.Email, 
                FirstName = request.FirstName, 
                LastName = request.LastName
            };

            customer.CustomerPreferences = request.PreferenceIds
                .Select(pid => new CustomerPreference { Customer = customer, PreferenceId = pid })
                .ToList();
           
            await _customerRepository.AddAsync(customer);

            return Ok(customer.Id);

        }

        /// <summary>
        /// Updates customer with the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="request">New customer's attributes</param>
        /// <returns>OK or NotFound</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            if (!await _customerRepository.Exists(id))
            {
                return NotFound();
            }

            // Это тоже типа Application Logic.
            var customer = await _customerRepository.GetByIdIncludePreferencesAsync(id);

            customer.Email = request.Email;
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;

            foreach (var preferenceId in request.PreferenceIds)
            {
                // Добавляем если нет такого preference у customer.
                if (customer.CustomerPreferences.All(cp => cp.PreferenceId != preferenceId))
                {
                    customer.CustomerPreferences.Add(
                        new CustomerPreference { Customer = customer, PreferenceId = preferenceId });
                }
            }
 
            await _customerRepository.UpdateAsync(customer);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            if (!await _customerRepository.Exists(id))
            {
                return NoContent();
            }

            await _customerRepository.DeleteAsync(id);
            return Ok();
        }
    }
}