using Microsoft.AspNetCore.Components;
using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class EditCategory : ComponentBase
    {
        private Category category = new Category();
        private bool isSaved = false;

        [Parameter]
        public string Category_Id { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }

        DbAccessor dbAccessor = new DbAccessor();

        protected override void OnInitialized()
        {
            category = dbAccessor.GetCategory(Category_Id);
        }

        private async Task UpdateCategory()
        {
            dbAccessor.UpdateCategory(category);

            isSaved = true;

            await Task.Delay(1000);
            NavigationManager.NavigateTo("/categorylist");
        }
    }
}