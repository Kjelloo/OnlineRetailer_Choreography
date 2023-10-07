using Microsoft.EntityFrameworkCore;
using OrderApi.Domain.Repositories;
using SharedModels.Order;
using SharedModels.Order.Models;

namespace OrderApi.Infrastructure.EfCore.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly OrderApiContext db;

    public OrderRepository(OrderApiContext context)
    {
        db = context;
    }

    public Order Add(Order entity)
    {
        if (entity.Date == null)
            entity.Date = DateTime.Now;
        
        var newOrder = db.Orders.Add(entity).Entity;
        db.SaveChanges();
        return newOrder;
    }
    
    public Order Get(int id)
    {
        var order =  db.Orders.Include(o => o.OrderLines).FirstOrDefault(o => o.Id == id);
        return order;
    }

    public IEnumerable<Order> GetAll()
    {
        return db.Orders.ToList();
    }
    
    public Order Edit(Order entity)
    {
        db.Entry(entity).State = EntityState.Modified;
        db.SaveChanges();
        return entity;
    }
    
    public Order Remove(Order entity)
    {
        var order = db.Orders.FirstOrDefault(entity);
        db.Orders.Remove(order);
        db.SaveChanges();
        return order;
    }

    public IEnumerable<Order> GetByCustomer(int customerId)
    {
        var ordersForCustomer = from o in db.Orders
                                where o.CustomerId == customerId
                                select o;

        return ordersForCustomer.Include(o => o.OrderLines).ToList();
    }
}
