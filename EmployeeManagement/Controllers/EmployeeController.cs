using EmployeeManagement.Model;
using EmployeeManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/employees")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
    {
        var employees = await _employeeRepository.GetEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var employee = await _employeeRepository.GetEmployeeAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeDto employee)
    {
        if (await _employeeRepository.EmployeeExistsAsync(employee.FirstName, employee.LastName, employee.Email))
        {
            return BadRequest("Employee with the same name and email already exists.");
        }

        var createdEmployee = await _employeeRepository.CreateEmployeeAsync(employee);

        return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.Id }, createdEmployee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto employee)
    {
        if (id != employee.Id)
        {
            return BadRequest("ID mismatch");
        }

        try
        {
            await _employeeRepository.UpdateEmployeeAsync(employee);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _employeeRepository.EmployeeExistsAsync(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<EmployeeDto>> DeleteEmployee(int id)
    {
        var employee = await _employeeRepository.GetEmployeeAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        await _employeeRepository.DeleteEmployeeAsync(id);

        return Ok(employee);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchEmployees([FromQuery] string name)
    {
        var employees = await _employeeRepository.SearchEmployees(name);

        if (employees == null || employees.Count() == 0)
        {
            return NotFound("No employees found matching the search criteria.");
        }

        return Ok(employees);
    }
}
