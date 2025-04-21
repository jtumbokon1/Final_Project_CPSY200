using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;
using Microsoft.AspNetCore.Components;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class AddCustomer : ComponentBase
    {
        private Customer customer;
        private bool isSaved = false;

        private DbAccessor dbAccessor = new DbAccessor();

        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Customer_Id { get; set; }

        protected override void OnInitialized()
        {
            customer = new Customer(); 
        }
        private async Task SaveCustomer()
        {
            dbAccessor.AddCustomer(customer);
            isSaved = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/customermanager");
        }
    }
}
