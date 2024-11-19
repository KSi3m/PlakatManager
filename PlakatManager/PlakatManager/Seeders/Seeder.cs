namespace ElectionMaterialManager.Entities.Seeders
{
    public class Seeder
    {
        private readonly ElectionMaterialManagerContext _dbContext;

        public Seeder(ElectionMaterialManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {

            if( _dbContext.Database.CanConnect() ) {

                var hasData = _dbContext.Users.Any()
                        || _dbContext.Statuses.Any()
                        || _dbContext.Tags.Any();

                if(!hasData)
                {
                    var user1 = new User()
                    {
                        FirstName = "Jan",
                        LastName = "Glin",
                        Email = "janglin@gmail.com",
                        Address = new Address()
                        {
                            Country = "Poland",
                            City = "Warsaw",
                            Street = "Krakowskie Przedmiescie",
                            PostalCode = "23-557"
                        }
                    };
                    _dbContext.Users.Add(user1);

                    var tags = new List<Tag>()
                    {
                        new Tag() {Value = "Campaign"},
                        new Tag() {Value = "Pre-campaign"},
                        new Tag() {Value = "For people"},
                        new Tag() {Value = "Meetings"}
                    };

                    _dbContext.Tags.AddRange(tags);
                    var statuses = new List<Status>()
                    {
                        new Status() {Name = "To do"},
                        new Status() {Name = "Done"},
                        new Status() {Name = "To be removed"},
                    };
                    _dbContext.Statuses.AddRange(statuses);

                    _dbContext.SaveChanges();
                }
            
            
            
            
            
            
            
            }

        }
    }
}
