using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;
using Microsoft.AspNetCore.Components;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class AddEquipment : ComponentBase
    {
        private Equipment equipment;
        private bool isSaved = false;

        // book database accessor  
        private DbAccessor dbAccessor = new DbAccessor();

        // used to navigate back to the Home page  
        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Equipment_Id { get; set; }

        protected override void OnInitialized()
        {
            equipment = new Equipment();
        }
        private async Task SaveEquipment()
        {
            dbAccessor.AddEquipment(equipment);
            isSaved = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/equipmentmanager");
        }
    }
}
