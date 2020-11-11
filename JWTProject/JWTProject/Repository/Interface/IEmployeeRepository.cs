using JWTProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTProject.Repository
{
   public interface IEmployeeRepository
    {
       Task<IEnumerable<Employee>> GetEmployees();
       Task< Employee> GetEmployee(int Id);
        Task AddEmployee(Employee employee);
        Task EditEmployee(Employee employee);
        Task DeleteEmployee(int id);
    }
}
