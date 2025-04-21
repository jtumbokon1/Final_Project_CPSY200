using System.ComponentModel;
using System.Runtime.Intrinsics.X86;
using Dapper;
using Final_Project_CPSY200.Models;
using MySqlConnector;

namespace Final_Project_CPSY200.Services
{
    public class DbAccessor
    {
        protected MySqlConnection connection;

        public DbAccessor()
        {
            string dbHost = "localhost";
            string dbUser = "root";
            string dbPassword = "password";

            var builder = new MySqlConnectionStringBuilder
            {
                Server = dbHost,
                UserID = dbUser,
                Password = dbPassword,
                Database = "village_rental"
            };
            connection = new MySqlConnection(builder.ConnectionString);
        }

        public void InitializeDatabase()
        {
            connection.Open();

            var checkDbSql = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'village_rental';";
            var databaseExists = connection.QueryFirstOrDefault<string>(checkDbSql) != null;

            if (!databaseExists)
            {
                var sql = @"CREATE DATABASE IF NOT EXISTS village_rental;
                        USE village_rental;

                        DROP TABLE IF EXISTS rental;
                        DROP TABLE IF EXISTS equipment;
                        DROP TABLE IF EXISTS customer;
                        DROP TABLE IF EXISTS category;                      

                        CREATE TABLE IF NOT EXISTS category (
                            Category_Id INT PRIMARY KEY,
                            Category_Name VARCHAR(50)
                        );

                        CREATE TABLE IF NOT EXISTS equipment (
                            Equipment_Id INT PRIMARY KEY,
                            Category_Id INT,
                            Equipment_Name VARCHAR(50),
                            Description TEXT,
                            Daily_Rate DECIMAL(10, 2),
                            FOREIGN KEY (Category_Id) REFERENCES category(Category_Id) ON DELETE CASCADE
                        );

                        CREATE TABLE IF NOT EXISTS customer (
                            Customer_Id INT PRIMARY KEY AUTO_INCREMENT,
                            Last_Name VARCHAR(50),
                            First_Name VARCHAR(50),
                            Contact_Phone VARCHAR(50),
                            Email VARCHAR(50)
                        );

                        CREATE TABLE rental (
                            Rental_Id INT PRIMARY KEY AUTO_INCREMENT,
                            Date DATE,
                            Customer_Id INT,
                            Equipment_Id INT,
                            Rental_Date DATE,
                            Return_Date DATE,
                            Cost DECIMAL(10, 2),
                            FOREIGN KEY (Customer_Id) REFERENCES customer(Customer_Id) ON DELETE CASCADE,
                            FOREIGN KEY (Equipment_Id) REFERENCES equipment(Equipment_Id)
                        );

                        ALTER TABLE rental MODIFY COLUMN Rental_Id INT AUTO_INCREMENT;

                        INSERT INTO category (Category_Id, Category_Name) VALUES
                        (10, 'Power tools'),
                        (20, 'Yard equipment'),
                        (30, 'Compressors'),
                        (40, 'Generators'),
                        (50, 'Air Tools');
                        
                        INSERT INTO equipment (Equipment_Id, Category_Id, Equipment_Name, Description, Daily_Rate) VALUES
                        (101, 10, 'Hammer drill', 'Powerful drill for concrete and masonry', 25.99),
                        (201, 20, 'Chainsaw', 'Gas-powered chainsaw for cutting wood', 49.99),
                        (202, 20, 'Lawn mower', 'Self-propelled lawn mower with mulching function', 19.99),
                        (301, 30, 'Small Compressor', '5 Gallon Compressor-Portable', 14.99),
                        (501, 50, 'Brad Nailer', 'Brad Nailer. Requires 3/4 to 1 1/2 Brad Nails', 10.99);

                        INSERT INTO customer (Customer_Id, Last_Name, First_Name, Contact_Phone, Email) VALUES
                        (1001, 'Doe', 'John', '(555) 555-1212', 'jd@sample.net'),
                        (1002, 'Smith', 'Jane', '(555) 555-3434', 'js@live.com'),
                        (1003, 'Lee', 'Michael', '(555) 555-5656', 'ml@sample.net');

                        INSERT INTO rental (Rental_Id, Date, Customer_Id, Equipment_Id, Rental_Date, Return_Date, Cost) VALUES  
                        (1000, '2024-02-15', 1001, 201, '2024-02-20', '2024-02-23', 149.97),
                        (1001, '2024-02-16', 1002, 501, '2024-02-21', '2024-02-25', 43.96);";

                connection.Execute(sql);
            }

            connection.Close();
        }

        public void AddCustomer(Customer customer)
        {
            connection.Open();

            var sql = @"INSERT INTO customer (Last_Name, First_Name, Contact_Phone, Email) 
                            VALUES (@Last_Name, @First_Name, @Contact_Phone, @Email)";
            connection.Execute(sql, new
            {
                customer.Last_Name,
                customer.First_Name,
                customer.Contact_Phone,
                customer.Email
            });
            connection.Close();
        }

        public List<Customer> GetAllCustomers()
        {
            var sql = "SELECT * FROM customer";
            connection.Open();
            var customers = connection.Query<Customer>(sql).ToList();
            connection.Close();
            return customers;
        }

        public Customer GetCustomer(string customer_Id)
        {
            var sql = "SELECT * FROM customer WHERE Customer_Id = @Customer_Id";
            connection.Open();
            var customer = connection.Query<Customer>(sql, new { Customer_Id = customer_Id }).FirstOrDefault();
            connection.Close();
            return customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            var sql = @"UPDATE customer 
                            SET Last_Name = @Last_Name, 
                                First_Name = @First_Name, 
                                Contact_Phone = @Contact_Phone, 
                                Email = @Email 
                            WHERE Customer_Id = @Customer_Id";

            connection.Open();
            connection.Execute(sql, new
            {
                customer.Customer_Id,
                customer.Last_Name,
                customer.First_Name,
                customer.Contact_Phone,
                customer.Email
            });
            connection.Close();
        }

        public void DeleteCustomer(string customer_Id)
        {
            var sql = "DELETE FROM customer WHERE Customer_Id = @Customer_Id";
            connection.Open();
            connection.Execute(sql, new { Customer_Id = customer_Id });
            connection.Close();
        }

        public void AddEquipment(Equipment equipment)
        {
            var sql = @"INSERT INTO equipment (Equipment_Id, Category_Id, Equipment_Name, Description, Daily_Rate) 
                    VALUES (@Equipment_Id, @Category_Id, @Equipment_Name, @Description, @Daily_Rate)";
            connection.Open();
            connection.Execute(sql, new
            {
                equipment.Equipment_Id,
                equipment.Category_Id,
                equipment.Equipment_Name,
                equipment.Description,
                equipment.Daily_Rate
            });
            connection.Close();
        }

        public List<Equipment> GetAllEquipments()
        {
            var sql = "SELECT * FROM equipment";
            connection.Open();
            var equipment = connection.Query<Equipment>(sql).ToList();
            connection.Close();
            return equipment;
        }

        public Equipment GetEquipment(string equipment_Id)
        {
            var sql = "SELECT * FROM equipment WHERE Equipment_Id = @Equipment_Id";
            connection.Open();
            var equipment = connection.Query<Equipment>(sql, new { Equipment_Id = equipment_Id }).FirstOrDefault();
            connection.Close();
            return equipment;
        }

        public void UpdateEquipment(Equipment equipment)
        {
            var sql = @"UPDATE equipment 
                            SET Category_Id = @Category_Id, 
                                Equipment_Name = @Equipment_Name, 
                                Description = @Description, 
                                Daily_Rate = @Daily_Rate 
                            WHERE Equipment_Id = @Equipment_Id";
            connection.Open();
            connection.Execute(sql, new
            {
                equipment.Equipment_Id,
                equipment.Category_Id,
                equipment.Equipment_Name,
                equipment.Description,
                equipment.Daily_Rate
            });
            connection.Close();
        }

        public void DeleteEquipment(string equipment_Id)
        {
            var sql = "DELETE FROM equipment WHERE Equipment_Id = @Equipment_Id";
            connection.Open();
            connection.Execute(sql, new { Equipment_Id = equipment_Id });
            connection.Close();
        }

        public List<Category> GetAllCategories()
        {
            var sql = "SELECT * FROM category";
            connection.Open();
            var categories = connection.Query<Category>(sql).ToList();
            connection.Close();
            return categories;
        }

        public Category GetCategory(string category_Id)
        {
            var sql = "SELECT * FROM category WHERE Category_Id = @Category_Id";
            connection.Open();
            var category = connection.Query<Category>(sql, new { Category_Id = category_Id }).FirstOrDefault();
            connection.Close();
            return category;
        }

        public void UpdateCategory(Category category)
        {
            var sql = @"UPDATE category 
                            SET Category_Name = @Category_Name 
                            WHERE Category_Id = @Category_Id";
            connection.Open();
            connection.Execute(sql, new
            {
                category.Category_Id,
                category.Category_Name
            });
            connection.Close();
        }

        public void DeleteCategory(string category_Id)
        {
            var sql = "DELETE FROM category WHERE Category_Id = @Category_Id";
            connection.Open();
            connection.Execute(sql, new { Category_Id = category_Id });
            connection.Close();
        }

        public void AddCategory(Category category)
        {
            var sql = @"INSERT INTO category (Category_Id, Category_Name) 
                    VALUES (@Category_Id, @Category_Name)";
            connection.Open();
            connection.Execute(sql, new
            {
                category.Category_Id,
                category.Category_Name
            });
            connection.Close();
        }

        public List<Rental> GetAllRentals()
        {
            var sql = "SELECT * FROM rental";
            connection.Open();
            var rentals = connection.Query<Rental>(sql).ToList();
            connection.Close();
            return rentals;
        }

        public Rental GetRental(int rental_Id)
        {
            var sql = "SELECT * FROM rental WHERE Rental_Id = @Rental_Id";
            connection.Open();
            var rental = connection.Query<Rental>(sql, new { Rental_Id = rental_Id }).FirstOrDefault();
            connection.Close();
            return rental;
        }

        public void UpdateRental(Rental rental)
        {
            var sql = @"UPDATE rental 
                            SET Customer_Id = @Customer_Id, 
                                Equipment_Id = @Equipment_Id, 
                                Rental_Date = @Rental_Date, 
                                Return_Date = @Return_Date, 
                                Cost = @Cost 
                            WHERE Rental_Id = @Rental_Id";
            connection.Open();
            connection.Execute(sql, new
            {
                rental.Rental_Id,
                rental.Customer_Id,
                rental.Equipment_Id,
                rental.Rental_Date,
                rental.Return_Date,
                rental.Cost
            });
            connection.Close();
        }

        public void AddRental(Rental rental)
        {
            var rateSql = "SELECT Daily_Rate FROM equipment WHERE Equipment_Id = @Equipment_Id";
            decimal dailyRate = connection.QuerySingle<decimal>(rateSql, new { rental.Equipment_Id });

            int rentalDays = (int)(rental.Return_Date - rental.Rental_Date).TotalDays;
            rental.Cost = dailyRate * rentalDays;

            rental.Date = DateTime.Now.Date;

            var sql = @"INSERT INTO rental (Date, Rental_Date, Customer_Id, Equipment_Id, Return_Date, Cost)
                VALUES (@Date, @Rental_Date, @Customer_Id, @Equipment_Id, @Return_Date, @Cost)";

            connection.Execute(sql, new
            {
                rental.Date,
                rental.Rental_Date,
                rental.Customer_Id,
                rental.Equipment_Id,
                rental.Return_Date,
                rental.Cost
            });
        }



        //public void InitializeDatabase()
        //{
        //    connection.Open();

        //    var sql = @"CREATE DATABASE IF NOT EXISTS village_rental;
        //            USE village_rental;

        //            DROP TABLE IF EXISTS rental;
        //            DROP TABLE IF EXISTS equipment;
        //            DROP TABLE IF EXISTS customer;
        //            DROP TABLE IF EXISTS category;


        //            CREATE TABLE IF NOT EXISTS category (
        //                Category_Id INT PRIMARY KEY,
        //                Category_Name VARCHAR(50)
        //            );

        //            CREATE TABLE IF NOT EXISTS equipment (
        //                Equipment_Id INT PRIMARY KEY,
        //                Category_Id INT,
        //                Equipment_Name VARCHAR(50),
        //                Description TEXT,
        //                Daily_Rate DECIMAL(10, 2),
        //                FOREIGN KEY (Category_Id) REFERENCES category(Category_Id) ON DELETE CASCADE
        //            );

        //            CREATE TABLE IF NOT EXISTS customer (
        //                Customer_Id INT PRIMARY KEY AUTO_INCREMENT,
        //                Last_Name VARCHAR(50),
        //                First_Name VARCHAR(50),
        //                Contact_Phone VARCHAR(50),
        //                Email VARCHAR(50)
        //            );

        //            CREATE TABLE IF NOT EXISTS rental (
        //                Rental_Id INT PRIMARY KEY,
        //                Date DATE,   
        //                Customer_Id INT,
        //                Equipment_Id INT,
        //                Rental_Date DATE,
        //                Return_Date DATE,
        //                Cost DECIMAL(10, 2),
        //                FOREIGN KEY (Customer_Id) REFERENCES customer(Customer_Id) ON DELETE CASCADE,
        //                FOREIGN KEY (Equipment_Id) REFERENCES equipment(Equipment_Id) ON DELETE CASCADE
        //            );

        //            INSERT INTO category (Category_Id, Category_Name) VALUES
        //            (10, 'Power tools'),
        //            (20, 'Yard equipment'),
        //            (30, 'Compressors'),
        //            (40, 'Generators'),
        //            (50, 'Air Tools');

        //            INSERT INTO equipment (Equipment_Id, Category_Id, Equipment_Name, Description, Daily_Rate) VALUES
        //            (101, 10, 'Hammer drill', 'Powerful drill for concrete and masonry', 25.99),
        //            (201, 20, 'Chainsaw', 'Gas-powered chainsaw for cutting wood', 49.99),
        //            (202, 20, 'Lawn mower', 'Self-propelled lawn mower with mulching function', 19.99),
        //            (301, 30, 'Small Compressor', '5 Gallon Compressor-Portable', 14.99),
        //            (501, 50, 'Brad Nailer', 'Brad Nailer. Requires 3/4 to 1 1/2 Brad Nails', 10.99);

        //            INSERT INTO customer (Customer_Id, Last_Name, First_Name, Contact_Phone, Email) VALUES
        //            (1001, 'Doe', 'John', '(555) 555-1212', 'jd@sample.net'),
        //            (1002, 'Smith', 'Jane', '(555) 555-3434', 'js@live.com'),
        //            (1003, 'Lee', 'Michael', '(555) 555-5656', 'ml@sample.net');

        //            INSERT INTO rental (Rental_Id, Date, Customer_Id, Equipment_Id, Rental_Date, Return_Date, Cost) VALUES  
        //            (1000, '2024-02-15', 1001, 201, '2024-02-20', '2024-02-23', 149.97),
        //            (1001, '2024-02-16', 1002, 501, '2024-02-21', '2024-02-25', 43.96);";

        //    connection.Execute(sql);
        //    connection.Close();
        //}
    }
}
