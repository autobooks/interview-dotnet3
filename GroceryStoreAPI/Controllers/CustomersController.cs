using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GroceryStoreAPI.Domain;
using GroceryStoreAPI.DTO;

namespace GroceryStoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {                     
        private readonly ICustomerRepository _repository;        

        public CustomersController(ICustomerRepository repository)
        {            
            _repository = repository;                       
        }

        /// <summary>
        /// Get all customers
        /// </summary>                
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ICustomer>>> GetAsync()
        {                                          
            var customerList =  await _repository.GetList();
            return Ok(customerList);
        }

        /// <summary>
        /// Get customer by id
        /// </summary>         
        /// <param name="id"></param>
        [HttpGet("{id}")]        
        public async Task<ActionResult<ICustomer>> GetAsync(int id)
        {
            var customerEntity = await _repository.GetById(id);
            if (customerEntity == null)
            {
                return NotFound();
            }
            else
            {
                return  Ok(customerEntity);
            }            
        }

        /// <summary>
        /// Add customer
        /// </summary>        
        /// <param name="customer"></param>
        [HttpPost]
        public async Task<ActionResult> PostAsync(CustomerDTO customer)
        {
            try
            {
                var newCustomer = await _repository.Add(customer);            
                var res = CreatedAtAction("get", new {id = newCustomer.Id});
                return res;
            }
            catch(Exception ex)
            {
                return ValidationProblem(ex.Message);
            }            
        }

        /// <summary>
        /// Update customer
        /// </summary>        
		/// <param name="id"></param>
		/// <param name="customer"></param>
        [HttpPatch]          
        public async Task<ActionResult> PatchAsync(int id, CustomerDTO customer)
        {            
            try
            {
                var entity = await _repository.Update(id, customer);
                if (entity == null)
                {
                    return NotFound();               
                }
                else
                { 
                    return Ok();
                }
            }
            catch(Exception ex)
            {
                return ValidationProblem(ex.Message);
            }    
        }

        /// <summary>
        /// Delete customer by id
        /// </summary>        
        /// <param name="id"></param>
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var result = await  _repository.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }            
        }       
    }
}
