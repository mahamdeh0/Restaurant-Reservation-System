﻿using RestaurantReservation.Db.Models.Entities;

namespace RestaurantReservation.Db.Repositories
{
    public class TableRepository : Repository<Table>
    {
        public TableRepository(RestaurantReservationDbContext context) : base(context) { }

    }
}
