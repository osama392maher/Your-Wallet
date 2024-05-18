using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Your_Wallet.Areas.Identity.Data;
using Your_Wallet.Models.ViewModels;

namespace Your_Wallet.Models;

public class Category
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Icon { get; set; }
    
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))] // this so Syncfusion can serialize the enum
    public CategoryType Type { get; set; } = CategoryType.Expense;

    [NotMapped] public string TitleDisplay => $"{Icon} {Title}";

    public string? ApplicationUserId { get; set; } // Foreign key for the ApplicationUser
    public ApplicationUser? ApplicationUser { get; set; } // Navigation property for the ApplicationUser
    public virtual ICollection<Transaction> Transactions { get; set; }

    public static explicit operator CategoryViewModel(Category category)
    {
        return new CategoryViewModel
        {
            Id = category.Id,
            Title = category.Title,
            Icon = category.Icon,
            Type = category.Type
        };
    }

}

public enum CategoryType
{
    [EnumMember(Value = "Expense")]
    Expense,
    [EnumMember(Value = "Income")]
    Income
}