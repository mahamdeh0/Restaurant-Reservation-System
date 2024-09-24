﻿using RestaurantReservation.Db.Models.Entities;
using RestaurantReservation.Db.Models.Views;

namespace RestaurantReservation.Db.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetReservationsByCustomerAsync(int customerId);
        Task<List<ReservationDetails>> GetReservationDetailsAsync();

    }
}
