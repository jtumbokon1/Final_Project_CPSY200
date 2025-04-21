using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;
using Microsoft.AspNetCore.Components;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class EquipmentManager : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        private DbAccessor dbAccessor = new DbAccessor();

        public List<Equipment> equipments = new List<Equipment>();

        protected override void OnInitialized()
        {
            dbAccessor.InitializeDatabase();

            equipments = dbAccessor.GetAllEquipments();
        }

        public void EditEquipment(Equipment equipment)
        {
            NavigationManager.NavigateTo($"/editequipment/{equipment.Equipment_Id}");
        }

        public void DeleteEquipment(Equipment equipment)
        {
            dbAccessor.DeleteEquipment(equipment.Equipment_Id.ToString());
            equipments = dbAccessor.GetAllEquipments();
        }

        public void ViewEquipment(Equipment equipment)
        {
            NavigationManager.NavigateTo($"/viewequipment/{equipment.Equipment_Id}");
        }
    }
}