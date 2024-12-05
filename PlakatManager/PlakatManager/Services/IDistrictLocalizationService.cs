namespace ElectionMaterialManager.Services
{
    public interface IDistrictLocalizationService
    {

        bool GetDistrict(out string name, double Longitude, double Latitude);
    }
}
