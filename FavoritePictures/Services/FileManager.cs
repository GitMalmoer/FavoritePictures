using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using FavoritePictures;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace FavoritePictures.Services
{
    public class FileManager
    {
        string path = Environment.CurrentDirectory;

        string versionToken = "PictureAppV3";

        public bool SaveAlbumToTxt(List<Album> albums)
        {
            bool saveOk = false;
            StreamWriter streamWriter = null;

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "(*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.InitialDirectory = path;

                if (saveFileDialog.ShowDialog() == true)
                {
                    string exepath = Path.GetFullPath(saveFileDialog.FileName);

                    //streamWriter = new StreamWriter(path + saveFileDialog.FileName); DOESNT WORK BECAUSE INVISIBLE CHAR IN CODE
                    // https://stackoverflow.com/questions/63989894/filestream-the-filename-directory-name-or-volume-label-syntax-is-incorrect

                    streamWriter = new StreamWriter(exepath);

                    //string[] Initialization = { versionToken, pictures.Count.ToString() };

                    streamWriter.WriteLine(versionToken);
                    streamWriter.WriteLine(albums.Count);

                    //streamWriter.WriteLine(JsonConvert.SerializeObject(Initialization));


                    for (int i = 0; i < albums.Count; i++)
                    {
                        streamWriter.WriteLine(albums[i].AlbumName);
                        streamWriter.WriteLine(albums[i].GetPicturesCount().ToString());

                        for (int j = 0; j < albums[i].GetPicturesCount(); j++)
                        {
                            var jsonpicture = new Picture()
                            {
                                Name = albums[i].GetPictureFromAlbum(j).Name,
                                Url = albums[i].GetPictureFromAlbum(j).Url,
                                Description = albums[i].GetPictureFromAlbum(j).Description,
                            };

                            var jsonSerializedObj = JsonConvert.SerializeObject(jsonpicture);
                            streamWriter.WriteLine(jsonSerializedObj);

                        }
                       

                        
                    }

                    saveOk = true;
                    MessageBox.Show("File has been saved into: " + exepath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                saveOk = false;
            }
            finally
            {
                if (saveOk == true)
                    streamWriter.Close();
            }

            return saveOk;
        }


        public List<Album> ReadAlbumsFromTxt(List<Album> albums)
        {
            List<Album> InnerAlbumList = new List<Album>();
            bool readOk = false;
            StreamReader streamReader = null;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "(*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.CurrentDirectory;

                if (openFileDialog.ShowDialog() == true)
                {
                    var path = Path.GetFullPath(openFileDialog.FileName);
                    streamReader = new StreamReader(path);

                    if (streamReader.ReadLine() == versionToken)
                    {
                        int numberOfRegisteredAlbums = Convert.ToInt32(streamReader.ReadLine());

                        for (int i = 0; i < numberOfRegisteredAlbums; i++)
                        {
                            var albumName = streamReader.ReadLine();
                            int numberOfRegisteredPictures = Convert.ToInt32(streamReader.ReadLine());
                            Album album = new Album
                            {
                                AlbumName = albumName,
                            };
                            
                            
                            for (int j = 0; j < numberOfRegisteredPictures; j++)
                            {
                                var pictureObject = streamReader.ReadLine();

                                Picture picture = JsonConvert.DeserializeObject<Picture>(pictureObject);
                                album.AddPictureToAlbum(picture);
                            }

                            InnerAlbumList.Add(album);


                        }
                    }
                    MessageBox.Show("Opened file: " + path);
                    readOk = true;
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                if (readOk == true)
                    streamReader.Close();
            }


            return InnerAlbumList;
        }

    }
    

}
