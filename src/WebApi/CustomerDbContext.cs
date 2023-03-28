using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using WebApi.Models;

namespace WebApi
{
    public class CustomerDbContext : DbContext
    {
        public DbSet<Customer> customers { get; set; }

        public CustomerDbContext(DbContextOptions options) : base(options) 
        {
            InitTableIfNotExists();
        }

        private void InitTableIfNotExists()
        {
            string cmd = @"CREATE TABLE IF NOT EXISTS public.customers 
                                    (id int8 NOT NULL GENERATED ALWAYS AS IDENTITY,
	                                firstname varchar NOT NULL,
	                                lastname varchar NOT NULL);";

            using var command = this.Database.GetDbConnection().CreateCommand();
            command.CommandText = cmd;
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        
    }
}
