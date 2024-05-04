namespace Expense_Tracker.Models;

public class Category
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Icon { get; set; }
    
    public CategoryType Type { get; set; }
}

public enum CategoryType
{
    Expense,
    Income
}