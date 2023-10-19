using EmployeeManagement.Model;

namespace EmployeeManagement.Repository
{
    public interface IEmployeeRepository
    {
        EmployeeDto GetEmployeeById(int id);
        IEnumerable<EmployeeDto> GetAllEmployees();
        void AddEmployee(EmployeeDto employee);
        void UpdateEmployee(EmployeeDto employee);
        void DeleteEmployee(int id);
    }
}
