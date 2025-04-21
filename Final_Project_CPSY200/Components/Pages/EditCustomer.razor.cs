using Microsoft.AspNetCore.Components;
using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class EditCustomer : ComponentBase
    {
        private Customer customer = new Customer();
        private bool isSaved = false;

        [Parameter]
        public string Customer_Id { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }

        DbAccessor dbAccessor = new DbAccessor();

        protected override void OnInitialized()
        {
            customer = dbAccessor.GetCustomer(Customer_Id);
        }

        private async Task UpdateCustomer()
        {
            dbAccessor.UpdateCustomer(customer);

            isSaved = true;

            await Task.Delay(1000);
            NavigationManager.NavigateTo("/customermanager");
        }
    }
}