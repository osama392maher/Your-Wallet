using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Your_Wallet.Models;

public class Category
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Icon { get; set; }
    
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))] // this so Syncfusion can serialize the enum
    public CategoryType Type { get; set; } = CategoryType.Expense;

    [NotMapped] public string TitleDisplay => $"{Icon} {Title}";
    
 
}

public enum CategoryType
{
    [EnumMember(Value = "Expense")]
    Expense,
    [EnumMember(Value = "Income")]
    Income
}