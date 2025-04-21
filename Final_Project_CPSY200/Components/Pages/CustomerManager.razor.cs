using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;
using Microsoft.AspNetCore.Components;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class CustomerManager : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        private DbAccessor dbAccessor = new DbAccessor();

        public List<Customer> customers = new List<Customer>();

        protected override void OnInitialized()
        {
            dbAccessor.InitializeDatabase();

            customers = dbAccessor.GetAllCustomers();
        }

        public void EditCustomer(Customer customer)
        {
            NavigationManager.NavigateTo($"/editcustomer/{customer.Customer_Id}");
        }

        public void DeleteCustomer(Customer customer)
        {
            dbAccessor.DeleteCustomer(customer.Customer_Id.ToString());
            customers = dbAccessor.GetAllCustomers();
        }

        public void ViewCustomer(Customer customer)
        {
            NavigationManager.NavigateTo($"/viewcustomer/{customer.Customer_Id}");
        }
    }
}