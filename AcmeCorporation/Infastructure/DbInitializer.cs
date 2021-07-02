using AcmeCorporation.Areas.Identity.Data;
using AcmeCorporation.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeCorporation.Infastructure
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(DrawContext context)
        {
            // Delete the database, if it already exists. 
            context.Database.EnsureDeleted();

            // Create the database, if it does not already exists. This operation
            // is necessary, if you dont't use the in-memory database.
            context.Database.EnsureCreated();

            // Look for any UserProfile
            if (context.UserProfile.Any())
            {
                return;   // DB has been seeded
            }

            Product product = new Product();
            DateTime date = DateTime.Now;
            DateTime date2 = DateTime.Now;
            var userProfileList = new List<UserProfile>
            {
                new UserProfile
                {
                    Id = 1,
                    FirstName = "Billy",
                    LastName = "Bongo",
                    BirthDate = date,
                    UserInDrawId = 1,
                    User = new User
                    {
                        Email = "test@test.com",
                    },
                    UserInDraw = new UserInDraw
                    {
                        Id = 1,
                        HasEnteredAmount = 2
                    }
                },
                new UserProfile
                {
                    Id = 2,
                    FirstName = "Boboa",
                    LastName = "Lupin",
                    BirthDate = date2,
                    UserInDrawId = 2,
                    User = new User
                    {
                        Email = "tester@tester.com",
                    },
                    UserInDraw = new UserInDraw
                    {
                        Id = 2,
                        HasEnteredAmount = 1
                    }
                },
                new UserProfile
                {
                    Id = 3,
                    FirstName = "Jonjon",
                    LastName = "jonson",
                    BirthDate = date2,
                    UserInDrawId = 3,
                    User = new User
                    {
                        Email = "testa@testa.com",
                    },
                    UserInDraw = new UserInDraw
                    {
                        Id = 3,
                        HasEnteredAmount = 2
                    }
                }
            };

            List<Product> products = new List<Product>
            {
                new Product 
                {
                    Id = 1,
                    ProductName="A",
                    UserProfileId = 1,
                    ProductSerialNumberId = 1
                },
                new Product 
                {
                    Id = 2,
                    ProductName="B",
                    UserProfileId = 2,
                    ProductSerialNumberId = 2
                },
                new Product
                {
                    Id = 3,
                    ProductName="C",
                    UserProfileId = 3,
                    ProductSerialNumberId = 3
                }
            };

            
            List<ProductSerialNumbers> productSerialNumber = new List<ProductSerialNumbers>
            {
                new ProductSerialNumbers 
                { 
                    Id = 1,
                    ProductsId=1, 
                    ProductSerialNumber = Guid.Parse("0349624d-e3e6-4bc4-ae5c-7d72cec3e2d9"),
                    ValidForLottery = false
                },
                new ProductSerialNumbers
                {
                    Id = 2,
                    ProductsId=2,
                    ProductSerialNumber = Guid.Parse("55D658AD-338E-4885-8626-D0872918FA42"),
                    ValidForLottery = true
                },
                new ProductSerialNumbers
                {
                    Id = 3,
                    ProductsId=3,
                    ProductSerialNumber = Guid.Parse("29186B89-9B20-40A4-8EF5-13D2A114A9A8"),
                    ValidForLottery = true
                }
            };

            context.ProductSerialNumber.AddRange(productSerialNumber);
            context.SaveChanges();
            context.UserProfile.AddRange(userProfileList);
            context.SaveChanges();

            context.Product.AddRange(products);
            context.SaveChanges();
        }
    }
}
