using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;
using Microsoft.AspNetCore.Components;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class AddRental : ComponentBase
    {
        private Rental rental;
        private List<Customer> customers = new();
        private List<Equipment> equipments = new();
        private bool isSaved = false;

        private DbAccessor dbAccessor = new DbAccessor();

        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int Rental_Id { get; set; }

        protected override void OnInitialized()
        {
            rental = new Rental();
            customers = dbAccessor.GetAllCustomers();
            equipments = dbAccessor.GetAllEquipments();
        }
        private async Task SaveRental()
        {
            dbAccessor.AddRental(rental);
            isSaved = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/rentalmanager");
        }
    }
}
