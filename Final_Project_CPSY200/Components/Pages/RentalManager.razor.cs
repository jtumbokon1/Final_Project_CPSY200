using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;
using Microsoft.AspNetCore.Components;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class RentalManager : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        private DbAccessor dbAccessor = new DbAccessor();

        public List<Rental> rentals = new List<Rental>();
        public List<Customer> customers = new List<Customer>();
        public List<Equipment> equipments = new List<Equipment>();

        protected override void OnInitialized()
        {
            dbAccessor.InitializeDatabase();

            rentals = dbAccessor.GetAllRentals();
            customers = dbAccessor.GetAllCustomers();
            equipments = dbAccessor.GetAllEquipments();
        }

        public void EditRental(Rental rental)
        {
            NavigationManager.NavigateTo($"/editrental/{rental.Rental_Id}");
        }

        public void ViewRental(Rental rental)
        {
            NavigationManager.NavigateTo($"/viewrental/{rental.Rental_Id}");
        }
    }
}