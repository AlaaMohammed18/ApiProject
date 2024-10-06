using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Store.Data.Contexts;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class StoreIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager )
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Alaa Mohamed",
                    Email = "alaa@gmail.com",
                    UserName = "alaamohamed" , 
                    Address = new Address
                    {
                        FirstName = "Alaa", 
                        LastName = "Mohamed",
                        City = "October" , 
                        State = "Giza" , 
                        PostalCode = "1234",
                        Street = "3"
                    }
                }; 

                await userManager.CreateAsync(user , "Password123!");
            }

        }

    }
}
