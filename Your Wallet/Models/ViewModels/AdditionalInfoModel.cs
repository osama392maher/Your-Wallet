
namespace Your_Wallet.Models.ViewModels
{
    public class AdditionalInfoModel
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; }

        public Decimal? StartingBalance { get; set; }

    }
}
