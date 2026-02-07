# Employee CRUD Operations - Setup Guide

## Overview

This guide will help you set up and test the newly implemented Employee CRUD operations in your ASP.NET Core application.

## What Was Added

### 1. **Domain Layer** (No changes needed)
   - Employee entity already exists in `DigitalExcellenceHub.Domain/Entities/Employee.cs`
   - Inherits from `FullAuditableEntity<int>` with built-in audit tracking and soft delete

### 2. **Application Layer**
   - **DTOs**: 
     - `DigitalExcellenceHub.Application/Dtos/Employee/CreateEmployeeDto.cs`
     - `DigitalExcellenceHub.Application/Dtos/Employee/UpdateEmployeeDto.cs`
     - `DigitalExcellenceHub.Application/Dtos/Employee/EmployeeDto.cs`
   
   - **Service Interface**: 
     - `DigitalExcellenceHub.Application/Interfaces/IEmployeeService.cs`
   
   - **Service Implementation**: 
     - `DigitalExcellenceHub.Application/Services/EmployeeService.cs`
   
   - **Dependency Injection**: 
     - Updated `DigitalExcellenceHub.Application/DependencyInjection/ServiceExtensions.cs`
     - Registered `IEmployeeService` with `EmployeeService` implementation

### 3. **Infrastructure Layer**
   - **DbContext**: 
     - Updated `DigitalExcellenceHub.Infrastructure/Data/AppDbContext.cs`
     - Added `Employees` DbSet
   
   - **Repository**: Uses existing generic repository pattern
   - **Unit of Work**: Uses existing UnitOfWork implementation

### 4. **API Layer**
   - **Controller**: 
     - `DigitalExcellenceHub.Api/Controllers/EmployeesController.cs`
     - 7 RESTful endpoints for complete CRUD operations
   
   - **Documentation**: 
     - `DigitalExcellenceHub.Api/Controllers/CRUD_OPERATIONS_README.md`
   
   - **Testing**: 
     - `DigitalExcellenceHub.Api/EmployeeApi.http`

## Setup Instructions

### Step 1: Verify Database Configuration

Your application is already configured to use SQL Server:

**Connection String** (in `appsettings.json`):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DigitalExcellenceHubDB;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
}
```

Make sure SQL Server is running and accessible.

### Step 2: Add Database Migration

The Employee entity needs to be added to the database schema. Run the following commands:

```bash
# Navigate to the solution directory
cd C:\Users\ab.sayed\source\repos\Task

# Add migration for Employee entity
dotnet ef migrations add AddEmployeeEntityToDB --project DigitalExcellenceHub.Infrastructure --startup-project DigitalExcellenceHub.Api --context AppDbContext

# Apply migration to database
dotnet ef database update --project DigitalExcellenceHub.Infrastructure --startup-project DigitalExcellenceHub.Api --context AppDbContext
```

### Step 3: Build the Solution

```bash
dotnet build
```

### Step 4: Run the Application

```bash
cd DigitalExcellenceHub.Api
dotnet run
```

The application should start and display the URLs it's listening on:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
      Now listening on: http://localhost:5000
```

### Step 5: Test the API

You have several options to test the API:

#### Option A: Using Swagger UI

1. Navigate to: `https://localhost:7001/swagger`
2. Find the **Employees** section
3. Test each endpoint interactively

#### Option B: Using the HTTP File

1. Open `DigitalExcellenceHub.Api/EmployeeApi.http` in Visual Studio or VS Code
2. Update the `@baseUrl` if your port is different
3. Click "Send Request" on any test case

#### Option C: Using cURL

```bash
# Create an employee
curl -X POST "https://localhost:7001/api/employees" \
  -H "Content-Type: application/json" \
  -d "{\"firstName\":\"John\",\"lastName\":\"Doe\",\"email\":\"john.doe@example.com\",\"department\":\"IT\",\"position\":\"Software Engineer\"}"

# Get all employees
curl -X GET "https://localhost:7001/api/employees"

# Get employee by ID
curl -X GET "https://localhost:7001/api/employees/1"

# Update employee
curl -X PUT "https://localhost:7001/api/employees/1" \
  -H "Content-Type: application/json" \
  -d "{\"id\":1,\"firstName\":\"John\",\"lastName\":\"Doe\",\"email\":\"john.updated@example.com\",\"department\":\"IT\",\"position\":\"Senior Engineer\"}"

# Delete employee
curl -X DELETE "https://localhost:7001/api/employees/1"
```

#### Option D: Using Postman

Import the following collection structure:
- GET: `https://localhost:7001/api/employees`
- POST: `https://localhost:7001/api/employees`
- GET: `https://localhost:7001/api/employees/{id}`
- PUT: `https://localhost:7001/api/employees/{id}`
- DELETE: `https://localhost:7001/api/employees/{id}`

## Available Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/employees` | Get all employees (paginated) |
| GET | `/api/employees/search` | Search and filter employees |
| GET | `/api/employees/{id}` | Get employee by ID |
| POST | `/api/employees` | Create new employee |
| PUT | `/api/employees/{id}` | Update employee |
| DELETE | `/api/employees/{id}` | Delete employee (soft delete) |
| GET | `/api/employees/{id}/exists` | Check if employee exists |

## Key Features Implemented

### 1. **Complete CRUD Operations**
   - ✅ Create new employees
   - ✅ Read employee details
   - ✅ Update employee information
   - ✅ Delete employees (soft delete)

### 2. **Pagination**
   - Page number and page size parameters
   - Total count and total pages in response
   - Navigation flags (HasNext, HasPrevious)

### 3. **Search & Filtering**
   - Search by name, email, or position
   - Filter by department
   - Combine search terms with filters

### 4. **Validation**
   - Required field validation
   - Email format validation
   - Phone number format validation
   - String length validation
   - Model state validation in controller

### 5. **Audit Tracking**
   - Automatic CreatedAt, CreatedBy timestamps
   - Automatic UpdatedAt, UpdatedBy timestamps
   - DeletedAt, DeletedBy for soft deletes

### 6. **Soft Delete**
   - Deleted records are marked as deleted, not removed
   - Automatic filtering of deleted records
   - Can be restored if needed

### 7. **Error Handling**
   - Proper HTTP status codes
   - Descriptive error messages
   - Model validation errors
   - Not found responses

### 8. **Clean Architecture**
   - Clear separation of concerns
   - Repository pattern for data access
   - Unit of Work for transaction management
   - Service layer for business logic
   - DTOs for data transfer

## Testing Checklist

Use this checklist to verify everything works:

- [ ] Application starts without errors
- [ ] Swagger UI loads at `/swagger`
- [ ] Can create a new employee
- [ ] Can retrieve all employees with pagination
- [ ] Can search employees by name
- [ ] Can filter employees by department
- [ ] Can get employee by ID
- [ ] Can update employee information
- [ ] Can delete employee (soft delete)
- [ ] Deleted employee doesn't appear in list
- [ ] Validation errors are returned for invalid data
- [ ] 404 is returned for non-existent employee
- [ ] Audit fields (CreatedAt, UpdatedAt) are populated

## Troubleshooting

### Issue: Migration fails

**Solution**: Ensure SQL Server is running and the connection string is correct.

```bash
# Test SQL Server connection
sqlcmd -S localhost -E
```

### Issue: Port already in use

**Solution**: Change the port in `DigitalExcellenceHub.Api/Properties/launchSettings.json`

### Issue: 404 Not Found on all endpoints

**Solution**: Make sure the application is running and you're using the correct base URL.

### Issue: Validation errors on create/update

**Solution**: Check the request body matches the DTO structure with required fields.

## Architecture Benefits

This implementation provides:

1. **Maintainability**: Clean separation between layers
2. **Testability**: Services can be easily unit tested
3. **Scalability**: Repository pattern allows for caching, etc.
4. **Security**: Soft delete prevents accidental data loss
5. **Auditability**: All changes are tracked automatically
6. **Flexibility**: Easy to add new features or entities

## Next Steps

Consider adding these enhancements:

1. **Authentication & Authorization**
   - Add JWT authentication to endpoints
   - Implement role-based access control
   - Restrict certain operations to admins

2. **Advanced Features**
   - Bulk import/export (CSV, Excel)
   - Employee photo upload
   - Document management
   - Advanced reporting

3. **Performance Optimization**
   - Add caching (Redis, Memory Cache)
   - Implement background jobs for heavy operations
   - Add database indexes for frequently queried fields

4. **Integration**
   - Link employees to users
   - Add department management
   - Implement employee hierarchy

5. **Validation & Business Rules**
   - Unique email constraint
   - Hire date validation (not in future)
   - Department from predefined list

## Support

For detailed API documentation, see:
- `DigitalExcellenceHub.Api/Controllers/CRUD_OPERATIONS_README.md`

For testing examples, see:
- `DigitalExcellenceHub.Api/EmployeeApi.http`

## Summary

You now have a fully functional CRUD API for managing employees with:
- 7 RESTful endpoints
- Pagination and search
- Validation and error handling
- Audit tracking and soft delete
- Clean Architecture implementation
- Complete documentation

Happy coding! 🚀
