using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using Your_Wallet.Areas.Identity.Data;

namespace Your_Wallet.Models;

public class Transaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string? Note { get; set; }
    [BindNever]

    public int CategoryId { get; set; }
    [BindNever]

    public Category Category { get; set; }
    
    [NotMapped]
    public string? AmountToDisplay
    {
        get
        {
            return ((Category == null || Category.Type == CategoryType.Expense) ? "- " : "+ ") + Amount.ToString("C0");
        }
    }
}