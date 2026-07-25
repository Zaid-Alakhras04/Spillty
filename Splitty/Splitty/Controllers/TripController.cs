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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TripController(ITripService tripService, IEmailService emailService , IWebHostEnvironment webHostEnvironment)
        {
            _tripService = tripService;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
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

            ViewBag.Settlements = _tripService.CalculateOptimizedSettlements(id);

            return View(trip);
            

        }

        [HttpPost]
        public IActionResult AddExpense(int id, string Description, int paidById, Dictionary<int, decimal> memberAmounts)
        {
            var actualAmounts = memberAmounts
        .Where(m => m.Value > 0)
        .ToDictionary(m => m.Key, m => m.Value);

            if (actualAmounts.Any())
            {
                // 3. Pass 'id' to the service
                _tripService.AddExpense(id, Description, paidById, actualAmounts);
            }

            // 4. Redirect back to the trip using 'id'
            return RedirectToAction("Details", new { id = id });
        }

        /*[HttpPost]
        public IActionResult DeleteTrip(int id)
        {
            _tripService.DeleteTrip(id);
            return RedirectToAction("Index");
        }
        */

        [HttpPost]
        public IActionResult AddMember(int tripId, string memberName)
        {
            if (!string.IsNullOrWhiteSpace(memberName))
            {
                _tripService.AddMemberToTrip(tripId, memberName);
            }

            return RedirectToAction("Details", new { id = tripId });
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


        [HttpPost]
        public async Task<IActionResult> UploadReceipt(int tripId, IFormFile receiptImage)
        {
            if (receiptImage != null && receiptImage.Length > 0)
            {
                // Create a secure, unique filename
                string fileExtension = Path.GetExtension(receiptImage.FileName);
                string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                // Map to wwwroot/uploads/receipts
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "receipts");

                // Create the folder if it doesn't exist yet
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // The full physical path on the hard drive
                string physicalFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Copy the file to the folder
                using (var fileStream = new FileStream(physicalFilePath, FileMode.Create))
                {
                    await receiptImage.CopyToAsync(fileStream);
                }

                // Save the relative URL to the database
                string databasePath = $"/uploads/receipts/{uniqueFileName}";
                _tripService.AddReciptImage(tripId, databasePath);
            }

            return RedirectToAction("Details", new { id = tripId });
        }

    }
}
