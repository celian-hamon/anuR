using anuR.Context;
using anuR.Models;
using anuR.Services;

namespace anuR.DataSeeder;

public class DataSeeder
{
    private readonly MainContext _context;
    private readonly IHashService _hashService;

    public DataSeeder(MainContext context, IHashService hashService)
    {
        _context = context;
        _hashService = hashService;
    }

    public void SeedData()
    {
        _context.Users.RemoveRange(_context.Users);
        _context.Sites.RemoveRange(_context.Sites);
        _context.Services.RemoveRange(_context.Services);

        _context.Users.Add(
            new User()
            {
                FirstName = "User",
                LastName = "AnuR",
                Email = "user@anur.com",
                PhoneNumber = Faker.Phone.Number(),
                Password = _hashService.GetHash("useruser"),
                IsAdmin = false,
            }
        );
        
        _context.Users.Add(
            new User()
            {
                FirstName = "Admin",
                LastName = "AnuR",
                Email = "admin@anur.com",
                PhoneNumber = Faker.Phone.Number(),
                Password = _hashService.GetHash("adminadmin"),
                IsAdmin = true,
            }
        );

        //create users 
        for (var i = 0; i < 1000; i++)
        {
            _context.Users.Add(
                new User()
                {
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last(),
                    Email = Faker.Internet.Email(),
                    PhoneNumber = Faker.Phone.Number(),
                    Password = _hashService.GetHash("password"),
                    IsAdmin = false,
                }
            );
        }

        //create sites
        for (var i = 0; i < 20; i++)
        {
            _context.Sites.Add(
                new Site()
                {
                    City = Faker.Address.City()
                }
            );
        }

        //create services
        for (var i = 0; i < 20; i++)
        {
            _context.Services.Add(
                new Service()
                {
                    Name = Faker.Name.FullName()
                }
            );
        }

        _context.SaveChanges();
    }
}