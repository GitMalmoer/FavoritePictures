using FavoritePictures.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FavoritePictures
{
    public class PictureManager
    {
        List<Picture> pictureList { set; get; }
        FileManager fileManager = new FileManager();
        public string PathToFile { get; set; }

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

        public void RemovePictureFromList(int index)
        {
            if (CheckIndex(index))
            {
                pictureList.Remove(GetPictureFromList(index));
            }
            else
                MessageBox.Show("Wrong selection!");
        }

        public Picture GetPictureFromList(int index)
        {
            if (CheckIndex(index))
                return pictureList[index];
            else
                return null;
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

        public bool CheckIndex(int index)
        {
            if(index > -1)
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
