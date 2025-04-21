using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;
using Microsoft.AspNetCore.Components;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class AddCategory : ComponentBase
    {
        private Category category;
        private bool isSaved = false;

        private DbAccessor dbAccessor = new DbAccessor();

        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Category_Id { get; set; }

        protected override void OnInitialized()
        {
            category = new Category();
        }
        private async Task SaveCategory()
        {
            dbAccessor.AddCategory(category);
            isSaved = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/categorylist");
        }
    }
}
