namespace DOCTORLOAN.Models.Bookings;

public class Booking
{
    public int Type { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public string  Phone { get; set; }
    public int BookingTimes { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly BookingStartTime { get; set; }
    public TimeOnly BookingEndTime { get; set; }
    public string AddressLine { get; set; }
    public int ProvinceId { get; set; }
    public int DistrictId { get; set; }
    public int WardId { get; set; }
    public string Noted { get; set; }
}