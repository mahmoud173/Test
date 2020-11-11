
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
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentController(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                var departments = await _repository.GetDepartments();
                return Ok(departments);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetDepartment/{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            try
            {
                var department = await _repository.GetDepartment(id);
                return Ok(department);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Department department)
        {
            try
            {
                await _repository.AddDepartment(department);
                return Ok(department);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(Department department)
        {
            try
            {
                await _repository.EditDepartment(department);
                return Ok(department);
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
                await _repository.DeleteDepartment(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}