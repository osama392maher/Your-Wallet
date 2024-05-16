namespace Your_Wallet.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))] // this so Syncfusion can serialize the enum
        public CategoryType Type { get; set; } = CategoryType.Expense;

        public static explicit operator Category(CategoryViewModel viewModel)
        {
            return new Category
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Icon = viewModel.Icon,
                Type = viewModel.Type
            };
        }

    }

    // casting to category type
}
