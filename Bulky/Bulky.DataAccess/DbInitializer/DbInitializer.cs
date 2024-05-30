using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
namespace Bulky.DataAccess.DbInitializer
{
	public class DbInitializer : IDbInitializer
	{
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {
            // migrations if they are not applied
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }

            // create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                // if roles are not created, then we will create admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "samanehhajigholam@gmail.com",
                    Email = "samanehhajigholam@gmail.com",
                    Name = "Samaneh Hajigholam",
                    PhoneNumber = "1234567889",
                    StreetAddress = "40 happiness st.",
                    State = "ON",
                    PostalCode = "J6G 6A7",
                    City = "Toronto"
                }, "Admin123*").GetAwaiter().GetResult();
                // once the application user is created, retrieve that user obj from the db
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "samanehhajigholam@gmail.com");

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            return;


        }
    }
}

