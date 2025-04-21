using Microsoft.AspNetCore.Components;
using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class EditEquipment : ComponentBase
    {
        private Equipment equipment = new Equipment();
        private bool isSaved = false;

        [Parameter]
        public string Equipment_Id { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }

        DbAccessor dbAccessor = new DbAccessor();

        protected override void OnInitialized()
        {
            equipment = dbAccessor.GetEquipment(Equipment_Id);
        }

        private async Task UpdateEquipment()
        {
            dbAccessor.UpdateEquipment(equipment);

            isSaved = true;

            await Task.Delay(1000);
            NavigationManager.NavigateTo("/equipmentmanager");
        }
    }
}