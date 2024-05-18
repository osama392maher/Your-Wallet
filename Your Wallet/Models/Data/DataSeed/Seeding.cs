using Newtonsoft.Json;
using Your_Wallet.Models.Data;

namespace Your_Wallet.Models.Data.DataSeed
{
    public static class Seeding
    {
        public static void SeedCategories(MainContext context)
        {
            var categories = context.Categories
                .Where(x => x.ApplicationUserId == null).Any();
          
            if (!categories)
            { 

                var data = File.ReadAllText("./Models/Data/DataSeed/categories.json");
                var categoriesData = JsonConvert.DeserializeObject<List<Category>>(data);
                if(categoriesData != null)
                context.Categories.AddRange(categoriesData);

                context.SaveChanges();
            }
        }
    }
}
