using Split.Core.DTOs;
using Split.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Split.BLL.Interfaces
{
    public interface ITripService
    {
       List<Trip> GetAllTrips();

        Trip? GetTripById(int id);

        void CreateTrip(string name , List<string> memberNames);

        void AddExpense(int tripId, string Description, int paidById , Dictionary<int, decimal> memberAmounts);

        public List<DebtSettelment> CalculateOptimizedSettlements(int tripId);

        void AddMemberToTrip(int tripId, string memberName);
        void AddReciptImage(int tripId, string imagePath);

        //void DeleteTrip(int tripId);
    }
}
