using Microsoft.AspNetCore.Components;
using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class EditRental : ComponentBase
    {
        private Rental rental = new Rental();
        private List<Customer> customers = new();
        private List<Equipment> equipments = new();
        private bool isSaved = false;

        [Parameter]
        public int Rental_Id { get; set; }
        public int Customer_Id { get; set; }
        public int Equipment_Id { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }

        DbAccessor dbAccessor = new DbAccessor();

        protected override void OnInitialized()
        {
            rental = dbAccessor.GetRental(Rental_Id);
            customers = dbAccessor.GetAllCustomers();
            equipments = dbAccessor.GetAllEquipments();
        }

        private async Task UpdateRental()
        {
            var equipment = equipments.FirstOrDefault(e => e.Equipment_Id == rental.Equipment_Id.ToString());
            if (equipment != null && decimal.TryParse(equipment.Daily_Rate, out decimal dailyRate))
            {
                int rentalDays = (int)(rental.Return_Date - rental.Rental_Date).TotalDays;
                rental.Cost = rentalDays > 0 ? dailyRate * rentalDays : 0;
            }

            dbAccessor.UpdateRental(rental);
            isSaved = true;

            await Task.Delay(1000);
            NavigationManager.NavigateTo("/rentalmanager");
        }
    }
}