namespace RestaurantReservation.API.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string userName, string password);

    }
}
