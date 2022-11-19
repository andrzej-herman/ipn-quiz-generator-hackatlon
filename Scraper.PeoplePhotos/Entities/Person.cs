using Scraper.PeoplePhotos.Dtos;

namespace Scraper.PeoplePhotos.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }

        public List<ImagePerson> Images { get; set; }
    }
}
