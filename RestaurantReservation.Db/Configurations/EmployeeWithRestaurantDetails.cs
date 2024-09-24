using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace RestaurantReservation.Db.Configurations
{
    public class EmployeeWithRestaurantDetails : IEntityTypeConfiguration<Models.Views.EmployeeWithRestaurantDetails>
    {
        public void Configure(EntityTypeBuilder<Models.Views.EmployeeWithRestaurantDetails> builder)
        {
            builder.HasNoKey();
            builder.ToView("EmployeesWithRestaurantDetails");
            builder.Property(e => e.EmployeeId).HasColumnName("employee_id");
            builder.Property(e => e.EmployeeFirstName).HasColumnName("First_Name");
            builder.Property(e => e.EmployeeLastName).HasColumnName("Last_Name");
            builder.Property(e => e.EmployeePosition).HasColumnName("Position");



        }
    }
}
