namespace Your_Wallet.Models;

public class Category
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Icon { get; set; }
    
    public CategoryType Type { get; set; } = CategoryType.Expense;
}

public enum CategoryType
{
    Expense,
    Income
}