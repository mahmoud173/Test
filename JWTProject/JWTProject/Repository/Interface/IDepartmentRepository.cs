using JWTProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTProject.Repository
{
   public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartments();
        Task<Department> GetDepartment(int Id);
        Task AddDepartment(Department department);
        Task EditDepartment(Department department);
        Task DeleteDepartment(int id);
    }
}
