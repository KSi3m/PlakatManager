namespace ElectionMaterialManager.Services
{
    public interface IDistrictLocalizationService
    {

        bool GetDistrict(out string district, out string city, double Longitude, double Latitude);
    }
}
