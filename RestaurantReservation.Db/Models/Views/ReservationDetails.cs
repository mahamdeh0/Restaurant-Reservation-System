namespace RestaurantReservation.Db.Models.Views
{
    public class ReservationDetails
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string RestaurantName { get; set; }
    }

}
