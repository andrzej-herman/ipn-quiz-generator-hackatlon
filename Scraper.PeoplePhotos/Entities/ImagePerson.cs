using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.PeoplePhotos.Entities
{
    public class ImagePerson
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
