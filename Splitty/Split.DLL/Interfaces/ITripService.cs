using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Split.Core.Entities;


namespace Split.BLL.Interfaces
{
    public interface ITripService
    {
       List<Trip> GetAllTrips();

        Trip? GetTripById(int id);

        void CreateTrip(string name , List<string> memberNames);

        void AddExpense(int tripId, string Description, int paidById , decimal amount, List<int> splitWithMemebrIDs);

        void DeleteTrip(int tripId);
    }
}
