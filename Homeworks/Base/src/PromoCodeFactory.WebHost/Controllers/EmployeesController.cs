using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeAsync(UpdateEmployeeRequest employeeRequest)
        {
            Employee employee = new Employee
            {
                Id = Guid.NewGuid(),
                Email = employeeRequest.Email,
                AppliedPromocodesCount = employeeRequest.AppliedPromocodesCount,
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName
            };

            
            await _employeeRepository.AddAsync(employee);


            return Ok(employee);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeeAsync(Guid guid)
        {
            try
            {
                await _employeeRepository.DeleteAsync(guid);
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }

            return Ok();
        }


        [HttpPatch]
        public async Task<IActionResult> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequest employeeRequest)
        {
            Employee employee = new Employee
            {
                Id = id,
                Email = employeeRequest.Email,
                AppliedPromocodesCount = employeeRequest.AppliedPromocodesCount,
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName
            };

            try
            {
                await _employeeRepository.UpdateAsync(employee);
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }
    }
}