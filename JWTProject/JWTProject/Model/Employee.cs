using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWTProject.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int Salary { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
