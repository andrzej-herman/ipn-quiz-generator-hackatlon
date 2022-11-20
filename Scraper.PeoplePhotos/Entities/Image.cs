using System.ComponentModel.DataAnnotations;

namespace Scraper.PeoplePhotos.Entities
{
    public class Image
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Url { get; set; }
        public string Alt { get; set; }
        public int? FacesCount { get; set; }
        public string Ocr { get; set; }

        public List<ImagePerson> People { get; set; } = new List<ImagePerson>();
    }
}