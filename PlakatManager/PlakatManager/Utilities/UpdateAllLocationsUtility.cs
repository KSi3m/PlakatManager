using ElectionMaterialManager.Entities;
using ElectionMaterialManager.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.Utilities
{
    public class UpdateAllLocationsUtility
    {
        public async Task Update(ElectionMaterialManagerContext dbContext, IServiceProvider services)
        {

            var districtService = services.GetRequiredService<IDistrictLocalizationService>();
            var electionItems = await dbContext.ElectionItems.ToListAsync();

            foreach (var item in electionItems)
            {
                if(item.Location.District == null)
                {
                    if(districtService.GetDistrict(out string district, out string city,
                        item.Location.Longitude, item.Location.Latitude))
                    {
                        item.Location.District = district;
                        if (item.Location.City == null) item.Location.City = city;
                    }
                }

            }
            await dbContext.SaveChangesAsync();
        }
    }
}
