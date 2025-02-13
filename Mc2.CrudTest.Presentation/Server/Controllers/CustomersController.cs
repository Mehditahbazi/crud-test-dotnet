using Mc2.CrudTest.Application.DTOs;
using Mc2.CrudTest.Application.Use_Cases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Presentation.Server.Controllers
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
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetCustomerByIdAsync), new { id = result }, result);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerByEmailAsync(string email)
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
        public async Task<IActionResult> GetCustomerByIdAsync(int id)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery() { Id = id });
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomerAsync(UpdateCustomerCommand command)
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
        public async Task<IActionResult> DeleteCustomerAsync(DeleteCustomerCommand command)
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