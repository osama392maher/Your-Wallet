using System.ComponentModel.DataAnnotations.Schema;

namespace Your_Wallet.Models;

public class Transaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string? Note { get; set; }

    public int CategoryId { get; set; }

    public Category? Category { get; set; }
    
    [NotMapped]
    public string? AmountToDisplay
    {
        get
        {
            return ((Category == null || Category.Type == CategoryType.Expense) ? "- " : "+ ") + Amount.ToString("C0");
        }
    }
}