using Castle.Components.DictionaryAdapter.Xml;
using CaseStudy.Helpers;
using CaseStudy.DAL.DomainClasses;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CaseStudy.DAL.DAO
{
    public class OrderDAO
    {
        private readonly AppDbContext _db;
        public OrderDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<int> AddOrder(int userid, OrderSelectionHelper[] selections)
        {
            int orderId = -1;
            // we need a transaction as multiple entities involved
            using (var _trans = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    Order order = new();
                    order.CustomerId = userid;
                    order.OrderDate = System.DateTime.Now;
                    order.OrderAmount = 0;
                    order.OrderTax = 0;
                    order.OrderTotal = 0;
                    //// calculate the totals and then add the tray row to the table
                    //Console.WriteLine((order).ToString);
                    foreach (OrderSelectionHelper selection in selections)
                    {
                        order.OrderAmount += selection.Item!.CostPrice * selection.Qty;
                    }
                    order.OrderTax = (order.OrderAmount / 100) * 13;
                    order.OrderTotal = order.OrderAmount + order.OrderTax;
                    await _db.Orders!.AddAsync(order);
                    await _db.SaveChangesAsync();

                    // then add each item to the trayitems table
                    foreach (OrderSelectionHelper selection in selections)
                    {
                        Product? product = _db.Products!.FirstOrDefault(t => t.Id == selection.Item!.Id);
                        if (product!=null)
                        {
                            //Enough stock(qty ordered < QtyOnHand)
                            if (product.QtyOnHand>=selection.Qty)
                            {
                                //Decrease the QtyOnHand in the products table by Qty
                                product.QtyOnHand -= selection.Qty;
                                //QtySold = Qty, QtyOrdered = Qty, QtyBackOrdered = 0 in the line items table
                                selection.Item!.QtySold = selection.Qty;
                                selection.Item!.QtyOrdered = selection.Qty;
                                selection.Item!.QtyBackOrdered = 0;
                            }
                            //Not enough stock (qty ordered > QtyOnHand)
                            else
                            {
                                //Increase the QtyOnBackOrdered by the difference between Qty and QtyOnHand in the products table
                                product.QtyOnBackOrder += selection.Qty - product.QtyOnHand;
                                //QtySold = QtyOnHand, QtyOrdered = Qty, QtyBackOrdered = Qty - QtyOnHand
                                selection.Item!.QtySold = product.QtyOnHand;
                                selection.Item!.QtyOrdered = selection.Qty;
                                selection.Item!.QtyBackOrdered = product.QtyOnBackOrder;
                                //Decrease the QtyOnHand to 0 in the products table
                                product.QtyOnHand = 0;
                            }
                            
                            if (product.QtyOnHand<0)
                            {
                                await _trans.RollbackAsync();
                                return orderId;
                            }
                            await _db.SaveChangesAsync();
                        }

                        OrderLineItem oItem = new();
                        oItem.OrderId = order.Id;
                        oItem.ProductId = selection.Item!.Id;
                        oItem.QtyOrdered = selection.Qty;
                        oItem.QtySold = selection.Item!.QtySold;
                        oItem.QtyBackOrdered = selection.Item!.QtyBackOrdered;
                        oItem.CostPrice = selection.Item!.CostPrice;
                        await _db.OrderLineItems!.AddAsync(oItem);
                        await _db.SaveChangesAsync();
                    }
                    await _trans.CommitAsync();
                    orderId = order.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await _trans.RollbackAsync();
                }
            }
            return orderId;
        }

        public async Task<List<Order>> GetAll(int id)
        {
            return await _db.Orders!.Where(order => order.CustomerId == id).ToListAsync<Order>();
        }

        public async Task<List<OrderDetailsHelper>> GetOrderDetails(int tid, string email)
        {
            Customer? user = _db.Customers!.FirstOrDefault(user => user.Email == email);
            List<OrderDetailsHelper> allDetails = new();
            // LINQ way of doing INNER JOINS
            var results = from t in _db.Orders
                          join ti in _db.OrderLineItems! on t.Id equals ti.OrderId
                          join mi in _db.Products! on ti.ProductId equals mi.Id
                          where (t.CustomerId == user!.Id && t.Id == tid)
                          select new OrderDetailsHelper
                          {
                              OrderId = t.Id,
                              CustomerId = user!.Id,
                              Description = mi.ProductName,
                              ProductId = ti.ProductId,
                              QtyOrdered = ti.QtyOrdered,
                              QtySold = ti.QtySold,
                              QtyBackOrdered = ti.QtyBackOrdered,
                              CostPrice = ti.CostPrice,
                              OrderDate = t.OrderDate.ToString("yyyy/MM/dd - hh:mm tt"),
                          };
            allDetails = await results.ToListAsync();
            return allDetails;
        }

    }
}
