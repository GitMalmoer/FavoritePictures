using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoritePictures
{
    public class AlbumManager
    {
        public List<PictureManager> pictureManagerList { get; set; }

        public string PathToFile { get; set; }

        public AlbumManager() 
        { 
        pictureManagerList= new List<PictureManager>();
        }

        public void AddToAlbum(PictureManager pictureManager)
        {
            pictureManagerList.Add(pictureManager);

        }
    }
}
