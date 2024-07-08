# E-commerce Application Using ASP.NET Core MVC 🛒 🛍️

BulkyBook is an e-commerce platform built with ASP.NET Core MVC. It allows administrators to manage orders, users, and products efficiently. The application also includes user authentication, role-based authorization, and integration with Stripe for payment processing.

# Live Demo 🖥️

The website deployed on Azure Microsoft: https://bulkybook-samaneh.azurewebsites.net/

(The website is hosted on a free Azure service, which may occasionally cause availability issues. If the site doesn't load, please try again also let me know for the admin credentials so you can access all the features and functionalities😊)


https://github.com/shajigholam/Bulky/assets/137809894/db4c7660-a33a-4877-a6dc-6de101e5d83c

# Features 📖

•	User Authentication and Authorization: Uses ASP.NET Core Identity for user management.

•	Role Management: Different roles (Admin, Employee, Company, Customer) with specific permissions, modifiable by admin. Company users can place their order and pay later within four days.

•	Order Management: Admins can view, update, process, ship, and cancel orders.

•	Product Management: Admins can manage product details.

•	Payment Processing: Integrated with Stripe for handling payments.

•	Session Management: Keeps user sessions for a defined period.

•	Database Management: Repository Pattern to Access Database, Seed Database Migrations Automatically.

# Technologies Used 🛠️

•	ASP.NET Core MVC

•	Entity Framework Core

•	ASP.NET Core Identity

•	Stripe API

•	jQuery and DataTables

•	Bootstrap

•	user sessions for a defined period.

# Setting Up the Project Locally 💻

1. **Clone the Repository**
2. **Open the Project in Visual Studio**
3. **Configure Database Connection** - Ensure the connection string in `appsettings.json` points to your SQL Server.
4. **Apply Migrations** - Package Manager Console) -> `update-database`
5. **Run the Application**



