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

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
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

    }
}
