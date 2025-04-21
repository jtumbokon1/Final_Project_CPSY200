using Final_Project_CPSY200.Models;
using Final_Project_CPSY200.Services;
using Microsoft.AspNetCore.Components;

namespace Final_Project_CPSY200.Components.Pages
{
    public partial class CategoryList : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        private DbAccessor dbAccessor = new DbAccessor();

        public List<Category> categories = new List<Category>();

        protected override void OnInitialized()
        {
            dbAccessor.InitializeDatabase();

            categories = dbAccessor.GetAllCategories();
        }

        public void EditCategory(Category category)
        {
            NavigationManager.NavigateTo($"/editcategory/{category.Category_Id}");
        }

        public void DeleteCategory(Category category)
        {
            dbAccessor.DeleteCategory(category.Category_Id.ToString());
            categories = dbAccessor.GetAllCategories();
        }

        public void ViewCategory(Category category)
        {
            NavigationManager.NavigateTo($"/viewcategory/{category.Category_Id}");
        }
    }
}