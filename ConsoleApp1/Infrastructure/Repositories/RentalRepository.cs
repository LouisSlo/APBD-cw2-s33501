namespace ConsoleApp1.Infrastructure.Repositories;

using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.Domain.Rentals;
using ConsoleApp1.Domain.Users;

public class RentalRepository
{
    private readonly List<Rental> _rentals = new();
    public void Add(Rental rental) => _rentals.Add(rental);
    public List<Rental> GetAll() => _rentals;
    
    public List<Rental> GetActiveRentalsForUser(User user) => _rentals.Where(r => r.User.Id == user.Id && r.ReturnDate == null).ToList();
}
