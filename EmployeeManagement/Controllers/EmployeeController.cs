using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Model;
using System;
using System.Collections.Generic;
using EmployeeManagement.Repository;

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
    public IActionResult GetEmployees()
    {
        var employees = _employeeRepository.GetAllEmployees();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployee(int id)
    {
        var employee = _employeeRepository.GetEmployeeById(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployee(EmployeeDto employee)
    {
        if (employee == null)
        {
            return BadRequest();
        }

        _employeeRepository.AddEmployee(employee);
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEmployee(int id, EmployeeDto employee)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }

        var existingEmployee = _employeeRepository.GetEmployeeById(id);

        if (existingEmployee == null)
        {
            return NotFound();
        }

        _employeeRepository.UpdateEmployee(employee);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        var employee = _employeeRepository.GetEmployeeById(id);

        if (employee == null)
        {
            return NotFound();
        }

        _employeeRepository.DeleteEmployee(id);
        return NoContent();
    }
}
