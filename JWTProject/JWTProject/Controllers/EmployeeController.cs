
using System.Threading.Tasks;
using JWTProject.Model;
using JWTProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTProject.Controllers
{
    [Authorize(Roles = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController( IEmployeeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetEmployees")]
        public async Task< IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _repository.GetEmployees();
                return Ok(employees);
            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpGet("GetEmployee/{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var employee = await _repository.GetEmployee(id);
                return Ok(employee);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
              await _repository.AddEmployee(employee);
                return Ok(employee);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(Employee employee)
        {
            try
            {
                await _repository.EditEmployee(employee);
                return Ok(employee);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
              await  _repository.DeleteEmployee(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}