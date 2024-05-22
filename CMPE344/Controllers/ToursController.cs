using CMPE344.Models;
using CMPE344.Services;
using CMPE344.ViewModels.Tours;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMPE344.Controllers;
public class ToursController(IDatabase db) : Controller
{
    // GET: /Tours
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> IndexAsync()
    {
        return View(await db.GetToursAsync());
    }

    // GET: Tours/Create
    [HttpGet]
    [Authorize(Roles = "Travel Agent")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Tours/Create
    [HttpPost]
    [Authorize(Roles = "Travel Agent")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TourViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            int? hotelId = await db.CreateHotel(name: viewModel.HotelName, location: viewModel.Location);

            if (hotelId != null)
            {
                int? flightId = await db.CreateFlightAsync(origin: viewModel.Origin, destination: viewModel.Destination, airline: viewModel.Airline, departureTime: viewModel.DepartureTime, arrivalTime: viewModel.ArrivalTime);

                if (flightId != null)
                {
                    int? tourId = await db.CreateTourAsync(title: viewModel.Title, description: viewModel.Description, startDate: viewModel.StartDate, endDate: viewModel.EndDate, capacity: viewModel.Capacity, price: viewModel.Price, hotelId: hotelId.Value, flightId: flightId.Value, createdByUser: int.Parse(User.Claims.First(f => f.Type == "UserId").Value));

                    return RedirectToAction("Index");
                }
            }

        }
        return View();
    }

    // GET: Tours/Details/5
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        Tour? tour = await db.GetTourAsync(id);
        if (tour == null)
        {
            return NotFound();
        }

        Hotel? hotel = await db.GetHotelAsync(tour.HotelId);
        if (hotel == null)
        {
            return NotFound();
        }

        Flight? flight = await db.GetFlightAsync(tour.FlightId);
        if (flight == null)
        {
            return NotFound();
        }

        return View((new TourViewModel(tour, hotel, flight), await db.CheckCustomerTourBuy(int.Parse(User.Claims.First(f => f.Type == "UserId").Value), id)));
    }

    // GET: Tours/Delete/5
    [HttpGet]
    [Authorize(Roles = "Travel Agent")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        Tour? tour = await db.GetTourAsync(id);
        if (tour == null)
        {
            return NotFound();
        }

        Hotel? hotel = await db.GetHotelAsync(tour.HotelId);
        if (hotel == null)
        {
            return NotFound();
        }

        Flight? flight = await db.GetFlightAsync(tour.FlightId);
        if (flight == null)
        {
            return NotFound();
        }

        return View(new TourViewModel(tour, hotel, flight));
    }

    // POST: Tours/Delete/5
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Travel Agent")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmedAsync(int id)
    {
        var tour = await db.GetTourAsync(id);
        if (tour != null)
        {
            await db.DeleteTourAsync(id);
        }

        return RedirectToAction("Index");
    }

    // GET: Tours/Buy/5
    [HttpGet]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> BuyAsync(int id)
    {
        Tour? tour = await db.GetTourAsync(id);
        if (tour == null)
        {
            return NotFound();
        }

        Hotel? hotel = await db.GetHotelAsync(tour.HotelId);
        if (hotel == null)
        {
            return NotFound();
        }

        Flight? flight = await db.GetFlightAsync(tour.FlightId);
        if (flight == null)
        {
            return NotFound();
        }

        return View(new TourViewModel(tour, hotel, flight));
    }

    // POST: Tours/Buy/5
    [HttpPost, ActionName("Buy")]
    [Authorize(Roles = "Customer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BuyTourAsync(int tourId, bool alreadyBuy)
    {
        if (alreadyBuy)
        {
            await db.ReturnTourAsync(int.Parse(User.Claims.First(f => f.Type == "UserId").Value), tourId);
        }
        else
        {
            await db.BuyTourAsync(int.Parse(User.Claims.First(f => f.Type == "UserId").Value), tourId);
        }

        return RedirectToAction("Details", new { id = tourId });
    }

    // GET: Tours/Edit/5
    [HttpGet]
    [Authorize(Roles = "Travel Agent")]
    public async Task<IActionResult> EditAsync(int id)
    {
        var tour = await db.GetTourAsync(id);

        if (tour == null)
        {
            return NotFound();
        }

        Hotel? hotel = await db.GetHotelAsync(tour.HotelId);
        if (hotel == null)
        {
            return NotFound();
        }

        Flight? flight = await db.GetFlightAsync(tour.FlightId);
        if (flight == null)
        {
            return NotFound();
        }

        return View(new TourViewModel(tour, hotel, flight));
    }

    // POST: Tours/Edit
    [HttpPost]
    [Authorize(Roles = "Travel Agent")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TourViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            await db.UpdateHotelAsync(hotelId: viewModel.HotelId, name: viewModel.HotelName, location: viewModel.Location);

            await db.UpdateFlightAsync(flightId: viewModel.FlightId, origin: viewModel.Origin, destination: viewModel.Destination, airline: viewModel.Airline, departureTime: viewModel.DepartureTime, arrivalTime: viewModel.ArrivalTime);

            await db.UpdateTourAsync(tourId: viewModel.TourId, title: viewModel.Title, description: viewModel.Description, startDate: viewModel.StartDate, endDate: viewModel.EndDate, capacity: viewModel.Capacity, price: viewModel.Price);

            return RedirectToAction("Details", new { id = viewModel.TourId });
        }
        return View();
    }
}