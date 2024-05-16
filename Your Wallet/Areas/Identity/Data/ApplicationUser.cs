using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Your_Wallet.Models;

namespace Your_Wallet.Areas.Identity.Data;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public string? ProfilePicture { get; set; }

    [NotMapped]
    public IFormFile? Image { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();


    public Decimal? StartingBalance { get; set; }

    public bool NameSet { get; set; } = false;
}

