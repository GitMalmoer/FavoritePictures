using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoritePictures
{
    public class Picture
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public Picture()
        {

        }
        public Picture(string name, string url, string description)
        {
            Name = name;
            Url = url;
            Description = description;
        }
    }

}
