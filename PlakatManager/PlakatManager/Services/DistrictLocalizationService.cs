
using NetTopologySuite.Geometries.Prepared;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System.Text.Json;

namespace ElectionMaterialManager.Services
{
    public class DistrictLocalizationService : IDistrictLocalizationService
    {
        public bool GetDistrict(out string district, out string city, double longitude, double latitude)
        {
            district = "";
            city = "";
            string folderPath = ".\\DistrictsGeoJSON\\";
            string[] geoJsonFiles = Directory.GetFiles(folderPath, "*.geojson");
            var geoJsonReader = new GeoJsonReader();
            Point point = new Point(longitude, latitude);

            foreach (var file in geoJsonFiles)
            {
                string geoJsonContent = File.ReadAllText(file);
             
                Geometry geometry = geoJsonReader.Read<Geometry>(geoJsonContent);
                var result = Contains(geometry, point);
                if (result)
                {
                    using JsonDocument doc = JsonDocument.Parse(geoJsonContent);
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("district", out var districtName))
                    {
                        district = districtName.GetString();
                    }
                    if (root.TryGetProperty("city", out var cityName))
                    {
                        city = cityName.GetString();
                    }
                    return true;
                }
            }
            return false;
        }

        private bool Contains(Geometry geom, Point point)
        {
            var prepGeom = PreparedGeometryFactory.Prepare(geom);

            if (prepGeom.Contains(point)) return true;
            return false;
        }
    }
}
