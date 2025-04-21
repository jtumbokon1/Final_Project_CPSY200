using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project_CPSY200.Models
{
    public class Rental
    {
        public int Rental_Id { get; set; }
        public DateTime Date { get; set; }
        public int Customer_Id { get; set; }
        public int Equipment_Id { get; set; }
        public DateTime Rental_Date { get; set; }
        public DateTime Return_Date { get; set; }
        public decimal Cost { get; set; }
    }
}
