using Microsoft.Data.SqlClient;

using SqlServerLib;

namespace SqlConsole;

internal class Program {

    static void Main(string[] args) {
        //TestOrderController();
        TestCustomerController();
    }

    static void TestOrderController() {
        var connStr = "server=localhost\\sqlexpress;" +
        "database=SalesDb;" +
        "trusted_connection=true;" +
        "trustServerCertificate=true;";
        Connection connection = new Connection(connStr);
        connection.Open();

        OrderController ordCtrl = new OrderController(connection);
        var orders = ordCtrl.GetAll();
        foreach(var o in orders) {
            Console.WriteLine(o);
        }
        /*
        var order = ordCtrl.GetByPK(11111);
        if(order is null) {
            Console.WriteLine("Order not found.");
        } else {
            Console.WriteLine(order);

        }
        var newOrder = new Order {
            Id = 0, CustomerId = 1, Date = new DateTime(2024,6,21), Description = "A new order"
        };
        var rc = ordCtrl.Create(newOrder);
        if(rc)
            Console.WriteLine("Created Successfully!");
        else
            Console.WriteLine("Create Failed.");

        var id = 29;
        var rc = ordCtrl.Remove(id);
        Console.WriteLine(rc ? "Success!" : "Failed");
        */

        connection.Close();
    }
    static void TestCustomerController() { 

        var connStr = "server=localhost\\sqlexpress;" +
                "database=SalesDb;" +
                "trusted_connection=true;" +
                "trustServerCertificate=true;";
        Connection connection = new Connection(connStr);
        connection.Open();

        IController<Customer>? custCtrl = (IController<Customer>?)new CustomerController(connection);

        //var searchedCustomers = custCtrl.Search("er");
        //foreach(var c in searchedCustomers) {
        //    Console.WriteLine($"{c.Id} | {c.Name}");
        //}


        // Test the Remove() *********************************/
        //var removed = custCtrl.Remove(45);
        //Console.WriteLine($"Did the remove succeed? {removed}");

        /* Test the GetAll() *********************************/
        var customers = custCtrl!.GetAll();
        foreach(var c in customers) {
            Console.WriteLine($"{c.Id} | {c.Name}");
        }

        /* Test the GetByPK(id) ******************************/
        //var id = 15;
        //var customer = custCtrl.GetByPK(id);
        //Console.WriteLine($"Id {id} is {customer!.Name}");

        /* Test the Create(Customer)**************************/
        //SqlServerLib.Customer newCustomer = new SqlServerLib.Customer {
        //    Id = 0, Name = "MAX", 
        //    City = "Cincinnati", State = "OH", 
        //    Sales = 1000, Active = true
        //};
        //var added = custCtrl.Create(newCustomer);
        //Console.WriteLine($"Did the create succeed? {added}");

        /* Test the Change(Customer) **************************/
        //customer!.City = "Lexington";
        //customer.State = "KY";
        //var changed = custCtrl.Change(customer);
        //Console.WriteLine($"Did the change succeed? {changed}");
        //customer = custCtrl.GetByPK(id);
        //Console.WriteLine($"Id {id} is {customer!.Name} | {customer.City} | {customer.State}");

        connection.Close();
    }

    static void LearningCode() { 

        var connStr = "server=localhost\\sqlexpress;" +
                        "database=SalesDb;" +
                        "trusted_connection=true;" +
                        "trustServerCertificate=true;";
        var conn = new SqlConnection(connStr);
        conn.Open();
        if(conn.State != System.Data.ConnectionState.Open) {
            throw new Exception("The connection didn't open!");
        }
        Console.WriteLine("Connection opened...");

        var sql = "SELECT * from Customers;";
        var sqlcmd = new SqlCommand(sql, conn);
        var reader = sqlcmd.ExecuteReader();
        if(!reader.HasRows) {
            Console.WriteLine("The Customer returned no rows...");
        }
        Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
        while(reader.Read()) {
            Customer customer = new Customer();
            customer.Id = Convert.ToInt32(reader["Id"]);
            customer.Name = Convert.ToString(reader["Name"])!;
            customer.City = Convert.ToString(reader["City"])!;
            customer.State = Convert.ToString(reader["State"])!;
            customer.Sales = Convert.ToDecimal(reader["Sales"]);
            customer.Active = Convert.ToBoolean(reader["Active"]);
            customers.Add(customer.Id, customer);
            Console.WriteLine($"Id: {customer.Id} | Name: {customer.Name} | Sales: {customer.Sales:C}");
        }

        reader.Close(); // don't forget this!!!!
        conn.Close();

    }

}

