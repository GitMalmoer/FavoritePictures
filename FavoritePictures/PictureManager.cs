using FavoritePictures.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoritePictures
{
    public class PictureManager
    {
        List<Picture> pictureList { set; get; }
        FileManager fileManager = new FileManager();

        public PictureManager()
        {
            pictureList= new List<Picture>();
        }

        public int GetPictureListCount()
        {
            return pictureList.Count;
        }

        public void AddPictureToList(Picture picture)
        {
            pictureList.Add(picture);
        }

        public void RemovePictureFromList(Picture picture)
        {
            pictureList.Remove(picture);
        }

        public Picture GetPictureFromList(int index)
        {
            return pictureList[index];
        }

        public bool SaveToTxt()
        {
            bool saveOk = false;

            if (fileManager.SaveToTxt(pictureList))
            {
                saveOk = true;
            }
            else
                saveOk = false;

            return saveOk;

        }

        public void ReadFromTxt()
        {
            pictureList = fileManager.ReadFromTxt(pictureList);
        }

    }
}
