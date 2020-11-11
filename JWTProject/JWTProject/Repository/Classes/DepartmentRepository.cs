using JWTProject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTProject.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddDepartment(Department department)
        {
            _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartment(int id)
        {
            Department department = await GetDepartment(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }

        public async Task EditDepartment(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task<Department> GetDepartment(int Id)
        {
            return await _context.Departments.FirstOrDefaultAsync(e => e.Id == Id);
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }
    }
}
