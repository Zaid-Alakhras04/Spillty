using Split.BLL.Interfaces;
using Split.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Split.Core.Entities;

namespace Split.BLL.Services
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext _context;

        public TripService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Trip> GetAllTrips()
        {
            return _context.Trips.Include(t => t.Members).ToList();
        }

        public Trip? GetTripById(int id)
        {
            return _context.Trips
                .Include(t => t.Members)
                .Include(t => t.Expense)
                .FirstOrDefault(t => t.Id == id);
        }

        public void CreateTrip(string name, List<string> memberNames)
        {
            var newTrip = new Trip
            {
                Name = name,

            };

            if (memberNames != null)
            {
                foreach (var nameString in memberNames)
                {
                    if (!string.IsNullOrWhiteSpace(nameString))
                    {
                        newTrip.Members.Add(new Member
                        {
                            Name = nameString.Trim()
                        });
                    }
                }
            }
            _context.Trips.Add(newTrip);
            _context.SaveChanges();
        }

        public void AddExpense(int tripId, string Description, int paidById, decimal amount, List<int> splitWithMemebrIDs)
        {
            var trip = _context.Trips.Include(t => t.Members).FirstOrDefault(t => t.Id == tripId);
            if (trip == null || splitWithMemebrIDs == null || !splitWithMemebrIDs.Any())
            {
                return;
            }
            var expense = new Expenses
            {
                Name = Description,
                Amount = amount,
                TripID = tripId,
                MemberId = paidById
            };

            foreach (var splitId in splitWithMemebrIDs)
            {
                expense.Shares.Add(new ExpenseShare
                {
                    MemberId = splitId,
                    OwnedAmount = amount / splitWithMemebrIDs.Count
                });
            }

            _context.Expenses.Add(expense);
            _context.SaveChanges();
        }

        public void DeleteTrip(int tripId)
        {
            var expenses = _context.Expenses.Where(e => e.TripID == tripId).ToList();

            foreach (var expense in expenses)
            {
                
                var shares = _context.ExpenseShares.Where(s => s.ExpenseId == expense.Id).ToList();
                _context.ExpenseShares.RemoveRange(shares);
            }

            _context.Expenses.RemoveRange(expenses);

            var members = _context.Members.Where(m => m.TripID == tripId).ToList();
            _context.Members.RemoveRange(members);

            var trip = _context.Trips.Find(tripId);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
            }

            _context.SaveChanges();
        }
    }
}
