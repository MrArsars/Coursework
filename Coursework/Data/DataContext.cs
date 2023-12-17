using Coursework.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Data;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<MotionSensorDataModel> MotionSensorData => Set<MotionSensorDataModel>();
}