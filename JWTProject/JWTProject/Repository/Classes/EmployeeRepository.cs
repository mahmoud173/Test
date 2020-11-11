using JWTProject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTProject.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddEmployee(Employee employee)
        {
             _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployee(int Id)
        {
            Employee employee = await GetEmployee(Id);
            _context.Employees.Remove(employee);
            await  _context.SaveChangesAsync();
        }

        public async Task EditEmployee(Employee employee)
        {

           _context.Employees.Update(employee);
           await  _context.SaveChangesAsync();

        }

        public async Task< Employee> GetEmployee(int id)
        {
            return await _context.Employees.Include(d=>d.Department).FirstOrDefaultAsync(e=>e.Id==id);
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
          return await  _context.Employees.Include(d => d.Department).ToListAsync();
        }
    }
}
