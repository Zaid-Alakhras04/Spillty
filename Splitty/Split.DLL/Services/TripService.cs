using Split.BLL.Interfaces;
using Split.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Split.Core.Entities;
using Split.Core.DTOs; 

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
                    .ThenInclude(e => e.Shares) // 2. CRUCIAL: Added ThenInclude so the algorithm can see the exact amounts owed
                .Include(t => t.Images)
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

        // 3. Removed the "decimal amount" parameter
        public void AddExpense(int tripId, string Description, int paidById, Dictionary<int, decimal> memberAmounts)
        {
            decimal totalAmount = memberAmounts.Values.Sum();

            var newExpense = new Expenses // Assuming your entity is named 'Expense'
            {
                Name = Description,
                Amount = totalAmount, // 4. Using the calculated total here!
                TripID = tripId,
                MemberId = paidById,
                Shares = new List<ExpenseShare>()
            };

            foreach (var item in memberAmounts)
            {
                newExpense.Shares.Add(new ExpenseShare // 5. Fixed variable name to newExpense and 'Share' to 'Shares'
                {
                    MemberId = item.Key,
                    OwnedAmount = item.Value
                });
            }

            _context.Expenses.Add(newExpense);
            _context.SaveChanges();
        }

        public List<DebtSettelment> CalculateOptimizedSettlements(int tripId)
        {
            var trip = GetTripById(tripId);
            if (trip == null || trip.Members == null) return new List<DebtSettelment>();

            var balances = new Dictionary<int, decimal>();

            foreach (var member in trip.Members)
            {
                decimal totalPaid = trip.Expense?.Where(e => e.MemberId == member.Id).Sum(e => e.Amount) ?? 0;
                decimal totalOwed = trip.Expense?.SelectMany(e => e.Shares).Where(s => s.MemberId == member.Id).Sum(s => s.OwnedAmount) ?? 0;
                balances[member.Id] = totalPaid - totalOwed;
            }

            var debtors = balances.Where(b => b.Value < -0.01m)
                                  .Select(b => (MemberId: b.Key, Amount: Math.Abs(b.Value)))
                                  .OrderByDescending(b => b.Amount).ToList();

            var creditors = balances.Where(b => b.Value > 0.01m)
                                    .Select(b => (MemberId: b.Key, Amount: b.Value))
                                    .OrderByDescending(b => b.Amount).ToList();

            var settlements = new List<DebtSettelment>();
            int i = 0, j = 0;

            while (i < debtors.Count && j < creditors.Count)
            {
                decimal settledAmount = Math.Min(debtors[i].Amount, creditors[j].Amount);

                settlements.Add(new DebtSettelment
                {
                    FromName = trip.Members.First(m => m.Id == debtors[i].MemberId).Name,
                    ToName = trip.Members.First(m => m.Id == creditors[j].MemberId).Name,
                    Amount = settledAmount
                });

                debtors[i] = (debtors[i].MemberId, debtors[i].Amount - settledAmount);
                creditors[j] = (creditors[j].MemberId, creditors[j].Amount - settledAmount);

                if (debtors[i].Amount < 0.01m) i++;
                if (creditors[j].Amount < 0.01m) j++;
            }

            return settlements;
        }


        public void AddMemberToTrip(int tripId, string memberName)
        {

            if (string.IsNullOrWhiteSpace(memberName)) return;

            var trip = _context.Trips.Include(t => t.Members).FirstOrDefault(t => t.Id == tripId);

            if (trip != null)
            {
                trip.Members.Add(new Member
                {
                    Name = memberName.Trim()
                });

                _context.SaveChanges();
            }


        }

        public void AddReciptImage(int tripId, string imagePath)
        {
            var newImage = new ReciptImage
            {
                TripId = tripId,
                ImagePath = imagePath
            };

            _context.Set<ReciptImage>().Add(newImage);
            _context.SaveChanges();
        }

        /*public void DeleteTrip(int tripId)
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
        }*/
    }
}