namespace Expense_Tracker.Models;

public class Transaction
{
    public int Id { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateTime Date { get; set; } = DateTime.Now;
    
    public string? Note { get; set; }
    
    public int CategoryId { get; set; }
    
    public Category Category { get; set; }
    
}