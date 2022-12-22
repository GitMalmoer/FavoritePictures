using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FavoritePictures
{
    public class Album
    {
        List<Picture> pictureList = new List<Picture>();
        public string AlbumName { get; set; }

        public void AddPictureToAlbum(Picture picture)
        {
            pictureList.Add(picture);
        }

        public void Remove(Picture picture)
        {
            pictureList.Remove(picture);
        }

        public Picture GetPictureFromAlbum(int index)
        {
            if (CheckIndex(index))
                return pictureList[index];
            else
                MessageBox.Show("error");
                return null;
            
        }

        public int GetPicturesCount()
        {
            return pictureList.Count;
        }

        public bool CheckIndex(int index)
        {
            if (index > -1)
            {
                return true;
            }
            else
            {
                //MessageBox.Show("Wrong index");
                return false;
            }
        }

    }
}
