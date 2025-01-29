using Mc2.CrudTest.Application.Use_Cases;
using Mc2.CrudTest.Shared.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudTest.Presentation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomerAsync(CreateCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCustomerById), new { id = result }, result);
        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerByEmail(string email)
        {
            var query = new GetCustomerByEmailQuery() { email = email };
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery() { Id = id });
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException)
                {
                    return Conflict();
                }
                throw;
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(DeleteCustomerCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception e)
            {

                if (e is DbUpdateConcurrencyException)
                {
                    return Conflict();
                }
                throw; ;
            }

        }

    }
}