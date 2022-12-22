using FavoritePictures.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FavoritePictures
{
    public class PictureManager
    {
        FileManager fileManager = new FileManager();
        public string PathToFile { get; set; }
        
        // adding protection level by setting private setter so that it wont be manipulated outside of the class
        public List<Album> albumList { private set; get; }


        public PictureManager()
        {
            albumList= new List<Album>();
        }
        public void RemoveAlbum(int index)
        {
            if(CheckIndex(index))
            {
                if (albumList.Contains(albumList[index]))
                albumList.RemoveAt(index);
            }
        }
            

        public void RemovePictureFromAlbum(int selectedAlbum,int selectedPicture)
        {
            if (CheckIndex(selectedAlbum) && CheckIndex(selectedPicture))
            {
                albumList[selectedAlbum].Remove(GetPictureFromAlbum(selectedAlbum,selectedPicture));
            }
            else
                MessageBox.Show("Wrong selection!");
        }


        public Picture GetPictureFromAlbum(int albumindex,int pictureIndex)
        {
            if (CheckIndex(albumindex) && CheckIndex(pictureIndex))
                return albumList[albumindex].GetPictureFromAlbum(pictureIndex);
            else
                return null;
        }

        public bool SaveAlbumToTxt()
        {
            bool saveOk = false;

            if (fileManager.SaveAlbumToTxt(albumList))
            {
                saveOk = true;
            }
            else
                saveOk = false;

            return saveOk;

        }

        public void ReadAlbumsFromTxt()
        {
            albumList = fileManager.ReadAlbumsFromTxt(albumList);
        }

        public void ClearManager()
        {
            PathToFile = string.Empty;
            albumList.Clear();
        }

        public bool CheckIndex(int index)
        {
            if(index > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
