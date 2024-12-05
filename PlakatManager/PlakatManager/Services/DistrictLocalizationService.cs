
using NetTopologySuite.Geometries.Prepared;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace ElectionMaterialManager.Services
{
    public class DistrictLocalizationService : IDistrictLocalizationService
    {
        public bool GetDistrict(out string name, double longitude, double latitude)
        {
            name = "";
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
                    name = Path.GetFileNameWithoutExtension(file);
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
