
InterviewProject
│── Controllers
│   └── UsersController.cs
│── Data
│   └── AppDbContext.cs
│── Models
│   └── User.cs
│── Views
│   ├── Users
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Delete.cshtml
│   │   └── Details.cshtml
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml  
│── appsettings.json
│── Program.cs

dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

    ----Program.cs---

using InterviewProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(Options =>
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}");

app.Run();

----Appsettings.json----

    {
    "ConnectionStrings": {
        "Defaultconnection": "Server= TAJIM-ALI\\SQLEXPRESS02; Database=InterviewProjectDB; Trusted_Connection=True; TrustServerCertificate=True"
    },
  "Logging": {
        "LogLevel": {
            "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
        }
    },
  "AllowedHosts": "*"
}


        Models
        │   └── User.cs-----------

    using System.ComponentModel.DataAnnotations;

namespace InterviewProject.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}


------Data/AppDbContext.cs-------

using InterviewProject.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}


---------UsersController.cs-------
    using InterviewProject.Data;
using InterviewProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace InterviewProject.Controllers
{
    public class UsersController : Controller
    {
        public readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Users.ToList());
        }

        public IActionResult Details(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}


Views
│   ├── Users
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Delete.cshtml
│   │   └── Details.cshtml
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml  


    Index.cshtml--------------

    @model IEnumerable<InterviewProject.Models.User>
<h2>User List</h2>

<a asp-action="Create" class= "btn btn-sm" > Add New User</a>

<table class= "table" border = "1" cellpadding = "5" >
    < tr >
        < th > Name </ th >
        < th > Age </ th >
        < th > Email </ th >
        < th > Actions </ th >
    </ tr >

    @foreach(var item in Model)
    {
        < tr >
            < td > @item.Name </ td >
            < td > @item.Age </ td >
            < td > @item.Email </ td >
            < td >
                < a asp - action = "Details" asp - route - id = "@item.Id" > Details </ a > |
                < a asp - action = "Edit" asp - route - id = "@item.Id" > Edit </ a > |
                < a asp - action = "Delete" asp - route - id = "@item.Id" > Delete </ a >
            </ td >
        </ tr >
    }
</ table >


    Create.cshtml---------------

    @model InterviewProject.Models.User


<form asp-action = "Create" method="post">
    <label>Name</label>
    <input asp-for="Name" class= "form-control" />

    < label > Age </ label >
    < input asp -for= "Age" class= "form-control" />


    < label > Email </ label >
    < input asp -for= "Email" class= "form-control" />


    < button type = "submit" > Save </ button >
</ form >


---------Edit.cshtml---------

@model InterviewProject.Models.User

<h2> Edit User</h2>

<form asp-action="Edit" method="post">
    <input type="hidden" asp-for="Id" />

    <div>
        <label>Name</label>
        <input asp-for="Name" class= "form-control" />
    </ div >

    < div >
        < label > Age </ label >
        < input asp -for= "Age" class= "form-control" />
    </ div >

    < div >
        < label > Email </ label >
        < input asp -for= "Email" class= "form-control" />
    </ div >

    < button type = "submit" > Update </ button >
    < a asp - action = "Index" > Back </ a >
</ form >

----Details.cshtml-------
@model InterviewProject.Models.User

<h2> User Details</h2>

<table class= "table" >
    < tr >
        < th > Name </ th >
        < td > @Model.Name </ td >
    </ tr >
    < tr >
        < th > Age </ th >
        < td > @Model.Age </ td >
    </ tr >
    < tr >
        < th > Email </ th >
        < td > @Model.Email </ td >
    </ tr >
</ table >

< a asp - action = "Edit" asp - route - id = "@Model.Id" > Edit </ a > |
< a asp - action = "Index" > Back to List</a>


    ----Delete.cshtml-----

    @model InterviewProject.Models.User

<h2> Delete User</h2>

<h4>Are you sure you want to delete this?</h4>

<div>
    <p><b>Name:</ b > @Model.Name </ p >
    < p >< b > Age:</ b > @Model.Age </ p >
    < p >< b > Email:</ b > @Model.Email </ p >
</ div >

< form asp - action = "Delete" method = "post" >
    < input type = "hidden" asp -for= "Id" />
    < button type = "submit" > Delete </ button >
    < a asp - action = "Index" > Cancel </ a >
</ form >


    ----_ViewImports.cshtml------

    @using InterviewProject
@using InterviewProject.Models
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers


------------ _ViewStart.cshtml----

    @{
    Layout = null;
}


-------Database-----

    CREATE DATABASE InterviewProjectDB;

CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Email NVARCHAR(150) NOT NULL
);







