using EmployeeManagement.Model;
using EmployeeManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;

[TestFixture]
public class EmployeeControllerTests
{  
    private EmployeeController _employeeController;
    private Mock<IEmployeeRepository> _employeeRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _employeeController = new EmployeeController(_employeeRepositoryMock.Object);
    }

    [Test]
    public async Task GetEmployees_ReturnsListOfEmployees()
    {
        var employees = new List<EmployeeDto>
        {
            new EmployeeDto { Id = 1, FirstName = "Raul", LastName = "stark", Email = "raul@gmail.com.com" },
            new EmployeeDto { Id = 2, FirstName = "rahul", LastName = "shukla", Email = "rs@gmail.com" },
        };
        _employeeRepositoryMock.Setup(repo => repo.GetEmployeesAsync()).ReturnsAsync(employees);

        var result = await _employeeController.GetEmployees();

        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsInstanceOf(typeof(List<EmployeeDto>), okResult.Value);
        Assert.AreEqual(employees, okResult.Value);
    }

    [Test]
    public async Task GetEmployee_ExistingEmployee_ReturnsEmployee()
    {
        var existingEmployee = new EmployeeDto { Id = 1, FirstName = "Rahul", LastName = "Shukla", Email = "rs@gmail.com" };
        _employeeRepositoryMock.Setup(repo => repo.GetEmployeeAsync(1)).ReturnsAsync(existingEmployee);

        var result = await _employeeController.GetEmployee(1);

        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsInstanceOf(typeof(EmployeeDto), okResult.Value);
        Assert.AreEqual(existingEmployee, okResult.Value);
    }

    [Test]
    public async Task CreateEmployee_ValidInput_CreatesEmployee()
    {
        var newEmployee = new EmployeeDto { FirstName = "Raul", LastName = "stark", Email = "raul@gmail.com" };
        _employeeRepositoryMock.Setup(repo => repo.EmployeeExistsAsync("Rahul", "shukla", "rs@gmail.com")).ReturnsAsync(false);
        _employeeRepositoryMock.Setup(repo => repo.CreateEmployeeAsync(newEmployee)).ReturnsAsync(newEmployee);

        var result = await _employeeController.CreateEmployee(newEmployee);

        Assert.IsInstanceOf(typeof(CreatedAtActionResult), result.Result);
        var createdResult = result.Result as CreatedAtActionResult;
        Assert.AreEqual("GetEmployee", createdResult.ActionName);
        Assert.AreEqual(newEmployee, createdResult.Value);
    }

    [Test]
    public async Task CreateEmployee_DuplicateEmployee_ReturnsBadRequest()
    {
        var existingEmployee = new EmployeeDto { FirstName = "Rahul", LastName = "Shukla", Email = "rahul@gmail.com" };
        _employeeRepositoryMock.Setup(repo => repo.EmployeeExistsAsync("Rahul", "Shukla", "rahul@gmail.com")).ReturnsAsync(true);

        var result = await _employeeController.CreateEmployee(existingEmployee);

        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.AreEqual("Employee with the same name and email already exists.", badRequestResult.Value);
    }

    [Test]
    public async Task UpdateEmployee_ValidInput_UpdatesEmployee()
    {
        var existingEmployee = new EmployeeDto { Id = 1, FirstName = "Rahul", LastName = "Shukla", Email = "rahul@gmail.com" };
        var updatedEmployee = new EmployeeDto { Id = 1, FirstName = "Rahul", LastName = "Shukla", Email = "rahul@gmail.com" };
        _employeeRepositoryMock.Setup(repo => repo.GetEmployeeAsync(1)).ReturnsAsync(existingEmployee);
        _employeeRepositoryMock.Setup(repo => repo.UpdateEmployeeAsync(updatedEmployee));

        var result = await _employeeController.UpdateEmployee(1, updatedEmployee);

        Assert.IsInstanceOf(typeof(NoContentResult), result); ;
    }

    [Test]
    public async Task DeleteEmployee_ExistingEmployee_DeletesEmployee()
    {
        var existingEmployee = new EmployeeDto { Id = 1, FirstName = "Raul", LastName = "shukla", Email = "shukla@gmail.com" };
        _employeeRepositoryMock.Setup(repo => repo.GetEmployeeAsync(1)).ReturnsAsync(existingEmployee);

        var result = await _employeeController.DeleteEmployee(1);

        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(existingEmployee, okResult.Value);
    }
}
