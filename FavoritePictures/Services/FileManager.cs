using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        string versionToken = "PictureAppV1";
    
        public bool SaveToTxt(List<Picture> pictures)
        {
            bool saveOk = false;
            StreamWriter streamWriter = null;

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "|*.txt|All files (*.*)|*.*";
                saveFileDialog.InitialDirectory = path;

                if (saveFileDialog.ShowDialog() == true)
                {
                    string exepath = Path.GetFullPath(saveFileDialog.FileName);

                    //streamWriter = new StreamWriter(path + saveFileDialog.FileName); DOESNT WORK BECAUSE INVISIBLE CHAR IN CODE
                    // https://stackoverflow.com/questions/63989894/filestream-the-filename-directory-name-or-volume-label-syntax-is-incorrect

                    streamWriter = new StreamWriter(exepath);
                    
                    string[] Initialization = { versionToken,pictures.Count.ToString()};

                    //streamWriter.WriteLine(versionToken);
                    //streamWriter.WriteLine(pictures.Count);

                    streamWriter.WriteLine(JsonConvert.SerializeObject(Initialization));


                    for (int i = 0; i < pictures.Count; i++)
                    {
                        var jsonpicture = new Picture()
                        {
                            Name = pictures[i].Name,
                            Url = pictures[i].Url,
                            Description = pictures[i].Description,
                        };

                        var jsonSerializedObj = JsonConvert.SerializeObject(jsonpicture);
                        streamWriter.WriteLine(jsonSerializedObj);
                    }

                    saveOk = true;
                    MessageBox.Show("File has been saved into: " + exepath);
                }
            }
            catch(Exception e)
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

        public List<Picture> ReadFromTxt(List<Picture> pictures)
        {
            List<Picture> InnerPictureList = new List<Picture>();
            bool readOk = false;
            StreamReader streamReader = null;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "|*.txt|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.CurrentDirectory;

                if (openFileDialog.ShowDialog() == true)
                {
                    var path = Path.GetFullPath(openFileDialog.FileName);
                    streamReader = new StreamReader(path);

                    if (streamReader.ReadLine() == versionToken)
                    {
                        int numberOfRegisteredPictures = Convert.ToInt32(streamReader.ReadLine());

                        for (int i = 0; i < numberOfRegisteredPictures; i++)
                        {
                            var pictureObject = streamReader.ReadLine();

                            Picture deserializedObject = JsonConvert.DeserializeObject<Picture>(pictureObject);

                            InnerPictureList.Add(deserializedObject);
                        }
                    }
                    MessageBox.Show("Opened file: " + path);
                    readOk = true;
                }

                
            }
            catch(Exception e) {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                if(readOk == true)
                streamReader.Close();
            }


            return InnerPictureList;
        }

    }
    

}
