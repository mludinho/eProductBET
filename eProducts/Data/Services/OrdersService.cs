using eProducts.Helper;
using eProducts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace eProducts.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context;
        public OrdersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
        {
            var orders = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Product).Include(n => n.User).ToListAsync();

            if(userRole != "Admin")
            {
                orders = orders.Where(n => n.UserId == userId).ToList();
            }

            return orders;
        }

        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    ProductId = item.Product.Id,
                    OrderId = order.Id,
                    Price = item.Product.Price
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            await _context.SaveChangesAsync();

            // Send Order Email
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress("sender@mydomain.com", "Web Registration"),
            new System.Net.Mail.MailAddress(userEmailAddress));
           
            string subject = string.Format("Order Number {0}", order.Id.ToString());
            double totalItems = 0.00;
            string message = "<h2>Ordered Product Items</h2><table>";
            message += "<tr><th>Name</th><th>Quantity</th><th>Price</th></tr>";
            foreach (var item in items) {
                totalItems += item.Amount * item.Product.Price;
                message += string.Format("<tr><td>{0}</td><td>{1}</td><td>R{2}</td></tr>", item.Product.Name,item.Amount.ToString(), item.Product.Price.ToString());
            }
            message += string.Format("<tr><td>{0}</td><td>R{1}</td><td>{2}</td></tr>", "SubTotal", totalItems.ToString(), string.Empty);
            message += "</table>";

            (new EmailMessenger()).SendEmail(subject, message, "magwenyanem@gmail.com", userEmailAddress);
        }

       
    }
}
