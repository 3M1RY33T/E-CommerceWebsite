# E-CommerceWebsite

This project is an online shopping website that uses Vue, Quasar and Node.js for its client-side operations, 
and .NET implementing MVC pattern for its server-side operations.
 
In this website, you can: 
- Register/Log in to your own account with encryption,
- view products from brands along with their cdescriptions and prices among other details,
- Search up the closest branches to you using your location information (using TomTom),
- Add the items you wish to your cart, which contains the price and product information,
- Submit the cart to make it into an order,
- View your past orders.

Products contain name, description, quantity available, quantity sold, quantity on back order, suggested 
price, price, and its tax attributes. An order contains products, the price, the tax, and the total is
calculated from each product's price value. The quantity available and quantity on back ordered values
of a product are affected in the event of a placed order.
