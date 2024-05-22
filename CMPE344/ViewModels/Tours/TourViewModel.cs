using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CMPE344.Models;

namespace CMPE344.ViewModels.Tours;

public class TourViewModel
{
    public TourViewModel()
    {
        
    }

    public TourViewModel(int tourId, int hotelId, int flightId, string title, string? description, DateTime startDate, DateTime endDate, int capacity, double price, int applied, string hotelName, string location, string origin, string destination, string airline, DateTime departureTime, DateTime arrivalTime)
    {
        TourId = tourId;
        HotelId = hotelId;
        FlightId = flightId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Capacity = capacity;
        Price = price;
        Applied = applied;
        HotelName = hotelName ?? throw new ArgumentNullException(nameof(hotelName));
        Location = location ?? throw new ArgumentNullException(nameof(location));
        Origin = origin ?? throw new ArgumentNullException(nameof(origin));
        Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        Airline = airline ?? throw new ArgumentNullException(nameof(airline));
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
    }

    public TourViewModel(Tour tour, Hotel hotel, Flight flight)
    {
        ArgumentNullException.ThrowIfNull(tour);
        ArgumentNullException.ThrowIfNull(hotel);
        ArgumentNullException.ThrowIfNull(flight);

        TourId = tour.TourId;
        HotelId = tour.HotelId;
        FlightId = tour.FlightId;

        Title = tour.Title;
        Description = tour.Description;
        StartDate = tour.StartDate;
        EndDate = tour.EndDate;
        Capacity = tour.Capacity;
        Price = tour.Price;
        Applied = tour.Applied;

        HotelName = hotel.HotelName;
        Location = hotel.Location;

        Origin = flight.Origin;
        Destination = flight.Destination;
        Airline = flight.Airline;
        DepartureTime = flight.DepartureTime;
        ArrivalTime = flight.ArrivalTime;
    }

    #region Tour

    public int TourId { get; set; }
    public int HotelId { get; set; }
    public int FlightId { get; set; }

    [Required(AllowEmptyStrings = false)]
    [DisplayName("Title")]
    [StringLength(45)]
    public string Title { get; set; } = string.Empty;

    [DisplayName("Description")]
    [StringLength(1023)]
    public string? Description { get; set; }

    [Required]
    [DisplayName("Start")]
    public DateTime StartDate { get; set; }

    public string StartDateString => StartDate.ToString("g");

    [Required]
    [DisplayName("End")]
    public DateTime EndDate { get; set; }

    public string EndDateString => EndDate.ToString("g");

    [Required]
    [DisplayName("Capacity")]
    [Range(0, int.MaxValue)]
    public int Capacity { get; set; }

    [Required]
    [DisplayName("Price")]
    public double Price { get; set; }

    public int Applied { get; set; }

    [DisplayName("Quota")]
    public int RemainingQuota => Capacity - Applied;

    #endregion

    #region Hotel

    [Required(AllowEmptyStrings = false)]
    [DisplayName("Name")]
    [StringLength(255)]
    public string HotelName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [DisplayName("Location")]
    [StringLength(255)]
    public string Location { get; set; } = string.Empty;

    #endregion

    #region Flight

    [Required(AllowEmptyStrings = false)]
    [DisplayName("Origin")]
    [StringLength(255)]
    public string Origin { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [DisplayName("Destination")]
    [StringLength(255)]
    public string Destination { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [DisplayName("Airline")]
    [StringLength(255)]
    public string Airline { get; set; } = string.Empty;

    [Required]
    [DisplayName("Departure")]
    public DateTime DepartureTime { get; set; }
    public string DepartureTimeString => DepartureTime.ToString("g");

    [Required]
    [DisplayName("Arrival")]
    public DateTime ArrivalTime { get; set; }
    public string ArrivalTimeString => ArrivalTime.ToString("g");

    #endregion
}