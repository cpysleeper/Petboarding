# CozyPaws Pet Boarding Management System

A web-based ASP.NET MVC application for managing pet boarding services.  
This system allows pet owners to manage pets and bookings, while employees and administrators manage operations through separate dashboards.

---

## Features

### Pet Owner
- Register and log in
- Manage pet profiles
- Add emergency contacts
- Add feeding plans
- Add pet medications
- Create boarding bookings
- Request booking cancellation
- View booking status updates

### Employee
- View all bookings
- Update booking status
- Handle cancellation requests
- View customer contact messages

### Admin
- Manage employee accounts
- Create employee accounts
- Edit employee information
- Delete employees
- View employee list dashboard

---

## Technologies Used

- ASP.NET MVC 5
- C#
- Entity Framework
- SQL Server
- ASP.NET Identity
- Bootstrap
- Razor Views

---

## Project Structure

```txt
Controllers/
Models/
Views/
Images/
Content/
Migrations/
```

---

## Roles

| Role | Description |
|------|-------------|
| Admin | Manages employees |
| Employee | Manages bookings and customer requests |
| PetOwner | Manages pets and bookings |

---

## How to Run

1. Clone the repository

```bash
git clone https://github.com/cpysleeper/Petboarding.git
```

2. Open solution in Visual Studio

3. Restore NuGet packages

4. Update database

```powershell
Update-Database
```

5. Run the project

---

## Notes

- Email service is used for account verification and password reset.
- Background images are customized for different layouts.
- Role-based authentication and authorization are implemented.

---

## Author

Panyi Chen
