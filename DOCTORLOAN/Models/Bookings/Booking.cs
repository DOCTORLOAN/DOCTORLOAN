namespace DOCTORLOAN.Models.Bookings;

public class Booking
{
    //public BookingType Type { get; set; }
    public int CustomerId { get; set; }
    public int? CustomerAddressId { get; set; }
    public int BookingTimes { get; set; }
    public DateOnly BookingDate { get; set; }
    public TimeOnly BookingStartTime { get; set; }
    public TimeOnly BookingEndTime { get; set; }
    //public BookingStatus Status { get; set; }
    public string Noted { get; set; }

    //public virtual Customer Customer { get; set; }
    //public virtual CustomerAddress CustomerAddresses { get; set; }

}