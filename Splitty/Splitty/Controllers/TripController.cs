using Microsoft.AspNetCore.Mvc;
using Split.BLL.Services;
using Split.BLL.Interfaces;
using System.Collections.Generic;
using Microsoft.Identity.Client;

namespace Splitty.Controllers
{
    public class TripController : Controller
    {
        private readonly ITripService _tripService;
        private readonly IEmailService _emailService;

        public TripController(ITripService tripService, IEmailService emailService)
        {
            _tripService = tripService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var trips = _tripService.GetAllTrips();
            return View(trips);
        }

        [HttpPost]
        public IActionResult Create(string name , List<string> memberNames)
        {
            _tripService.CreateTrip(name, memberNames);

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var trip = _tripService.GetTripById(id);

            if (trip == null)
            {
                return NotFound();

            }
            else
            {
                return View(trip);
            }

        }

        [HttpPost]
        public IActionResult AddExpense(int id, string description, int paidById, decimal amount, List<int> splitWithMemebrIDs)
        {
            _tripService.AddExpense(id, description, paidById, amount, splitWithMemebrIDs);
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public IActionResult DeleteTrip(int id)
        {
            _tripService.DeleteTrip(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ShareTrip(int tripId, string email)
        {
            // 1. Fetch the trip to get its name, expenses, and members
            var trip = _tripService.GetTripById(tripId);

            if (trip == null)
            {
                return NotFound();
            }

            // 2. Calculate the data for the summary
            decimal totalCost = trip.Expense != null ? trip.Expense.Sum(e => e.Amount) : 0;
            List<string> participants = trip.Members != null ? trip.Members.Select(m => m.Name ?? "Unknown").ToList() : new List<string>();

            // 3. Generate a dynamic, clickable link back to this exact trip page
            string shareableLink = Url.Action("Details", "Trip", new { id = tripId }, Request.Scheme) ?? "";

            // 4. Send the email!
            await _emailService.SendTripSummaryAsync(email, trip.Name ?? "Unnamed Trip", totalCost, participants, shareableLink);
            // 5. Redirect the user back to the trip dashboard
            return RedirectToAction("Details", new { id = tripId });
        }

    }
}
