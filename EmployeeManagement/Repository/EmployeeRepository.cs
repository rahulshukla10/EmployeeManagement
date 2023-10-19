using EmployeeManagement.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public EmployeeDto GetEmployeeById(int id)
        {
            return _context.Employees.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<EmployeeDto> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public void AddEmployee(EmployeeDto employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            // Check for uniqueness of first name, last name, and email address
            if (_context.Employees.Any(e =>
                e.FirstName == employee.FirstName &&
                e.LastName == employee.LastName &&
                e.Email == employee.Email))
            {
                throw new InvalidOperationException("Employee with the same first name, last name, and email address already exists.");
            }

            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void UpdateEmployee(EmployeeDto employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        public void DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }
    }
}
