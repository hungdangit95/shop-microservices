using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        private readonly ILogger _logger;
        private readonly OrderContext _context;
        public OrderContextSeed(ILogger logger, OrderContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await TrySeedAsync();
                    await _context.Database.MigrateAsync();
                }    
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            if(!_context.Orders.Any())
            {
                await _context.Orders.AddRangeAsync(
                    new Order
                    {
                        Username = "customer1",
                        FirstName = "customer1",
                        LastName = "customer",
                        EmailAddress = "customer1@local.com",
                        ShippingAddress = "Wollongong",
                        InvoiceAddress = "Autralia",
                        TotalPrice = 250
                    });
                await _context.SaveChangesAsync();
            }    
        }
    }
}
