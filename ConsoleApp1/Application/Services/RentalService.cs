using ConsoleApp1.Domain.Users;

namespace ConsoleApp1.Application.Services;

using ConsoleApp1.Application.Policies;
using ConsoleApp1.Domain.Equipment;
using ConsoleApp1.Domain.Rentals;
using ConsoleApp1.Infrastructure.Repositories;

public class RentalService
{
    private readonly RentalRepository _rentalRepository;
    private readonly IUserLimitPolicy _limitPolicy;
    private readonly IPenaltyCalculator _pentaltyCalculator;

    public RentalService(RentalRepository rentalRepository, IUserLimitPolicy limitPolicy,
        IPenaltyCalculator pentaltyCalculator)
    {
        _rentalRepository = rentalRepository;
        _limitPolicy = limitPolicy;
        _pentaltyCalculator = pentaltyCalculator;
    }

    public Rental rentEquipment(User user, Equipment equipment, int durationInDays)
    {
        if (!equipment.IsAvaliable)
        {
            throw new InvalidOperationException($"The {equipment.Name} is currently unavailable.");
        }
        var activeRentals = _rentalRepository.GetActiveRentalsForUser(user).Count;
        var maxRentals = _limitPolicy.GetMaxActiveRentals(user);
        if (activeRentals >= maxRentals)
        {
            throw new InvalidOperationException($"The user {user.FirstName} has reached the maximum number ({maxRentals}) of active rentals.");
        }
        var rental = new Rental(user, equipment, durationInDays);
        equipment.MarkAsUnavailable();
        _rentalRepository.Add(rental);
        return rental;
    }

    public void ReturnEquipment(Rental rental, DateTime returnDate)
    {
        if (rental.ReturnDate.HasValue)
        {
            throw new InvalidOperationException("The equipment has already been returned.");
        }
        var pentaly = _pentaltyCalculator.CalculatePenalty(rental.DueDate, returnDate);
        rental.ReturnEquipment(returnDate, pentaly);
    }
}