using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using Your_Wallet.Areas.Identity.Data;
using Your_Wallet.Models.ViewModels;

namespace Your_Wallet.Models;

public class Transaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string? Note { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    [NotMapped]
    public string? AmountToDisplay
    {
        get
        {
            return ((Category == null || Category.Type == CategoryType.Expense) ? "- " : "+ ") + Amount.ToString("C0");
        }
    }

    public ApplicationUser? ApplicationUser { get; set; }

    public string? ApplicationUserId { get; set; }

    public static explicit operator TransactionViewModel(Transaction transaction)
    {
        return new TransactionViewModel
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Note = transaction.Note,
            CategoryId = transaction.CategoryId
        };
    }
}