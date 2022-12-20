using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;


// https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/5f4bd7a6-f763-4518-9b81-bdfd40ce3fc9/d26yer1-421bb5b8-9fc2-4d5a-b2d1-1e1f81b26b82.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzVmNGJkN2E2LWY3NjMtNDUxOC05YjgxLWJkZmQ0MGNlM2ZjOVwvZDI2eWVyMS00MjFiYjViOC05ZmMyLTRkNWEtYjJkMS0xZTFmODFiMjZiODIucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.p5vfqGmq9kIylfG3glHGa20CAPUtoWlAxKEGpIvGOi8
// http://cdn.differencebetween.net/wp-content/uploads/2012/01/Difference-Between-Example-and-Sample.jpg

namespace FavoritePictures
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PictureManager pictureManager = new PictureManager();
        public MainWindow()
        {
            InitializeComponent();
            InitializeGUI();
        }

        private void InitializeGUI()
        {
            radioUrl.IsChecked = true;
            btnAddFile.IsEnabled = false;
        }

        /// <summary>
        /// This method gets file path or url and returns bitmapimage which you can later put into img container(tool in wpf)
        /// </summary>
        public BitmapImage InitializePicture(string url)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            try
            {
                bitmapImage.UriSource = new Uri($"{url}");
                bitmapImage.EndInit();
            }
            catch (Exception e)
            {
                MessageBox.Show("The picture failed to load. " + e.Message);
                imgBox.Source = null;
            }
            return bitmapImage;    
        }
        /// <summary>
        /// This method adds Picture to the listbox if user fills the text filles or adds file. This method contains checkers isnullorempty
        /// picture file is saved into string in picture mangager at the end of the method it sets it to string empty.
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Picture picture = new Picture();
            if (!string.IsNullOrEmpty(txtDescription.Text) && !string.IsNullOrEmpty(txtName.Text))
            {
                if (radioUrl.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(txtURL.Text))
                    {
                        picture = new Picture(txtName.Text, txtURL.Text, txtDescription.Text);
                        pictureManager.AddPictureToList(picture);
                        PopulateList();
                    }
                    else
                        MessageBox.Show("You need to add picture URL");
                }
                else
                {
                    if (!string.IsNullOrEmpty(pictureManager.PathToFile))
                    {
                        picture = new Picture(txtName.Text, pictureManager.PathToFile, txtDescription.Text);
                        pictureManager.AddPictureToList(picture);
                        PopulateList();
                    }
                    else
                        MessageBox.Show("Add picture file");
                }
            }
            else
                MessageBox.Show("Name and Description can not be empty!");

            // setting the saved path to file in manager to empty string
            pictureManager.PathToFile = string.Empty;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstPictures.SelectedIndex > -1)
            {
                int index = lstPictures.SelectedIndex;

                Picture picture = pictureManager.GetPictureFromList(index);

                var pictureBitmap = InitializePicture(picture.Url);

                imgBox.Source = pictureBitmap;
            }
        }

        private void menuOpenFrom_Click(object sender, RoutedEventArgs e)
        {
            pictureManager.ReadFromTxt();
            PopulateList();
        }

        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            pictureManager.SaveToTxt();
        }

        private void menuNew_Click(object sender, RoutedEventArgs e)
        {
            lstPictures.Items.Clear();
            txtDescription.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtURL.Text = string.Empty;
            imgBox.Source = null;
            InitializeGUI();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PopulateList()
        {
            lstPictures.Items.Clear();
            if (pictureManager.GetPictureListCount() > 0)
            {
                for (int i = 0; i < pictureManager.GetPictureListCount(); i++)
                {
                    Picture picture = pictureManager.GetPictureFromList(i);
                    lstPictures.Items.Add(string.Format("{0,-15} {1,-30} {2,-30}", picture.Name, picture.Url, picture.Description));
                }
            }
        }

        private void radioFile_Checked(object sender, RoutedEventArgs e)
        {
            if (radioUrl.IsChecked == true)
            {
                txtURL.IsEnabled = true;
                btnAddFile.IsEnabled = false;
            }
            else
            {
                txtURL.IsEnabled = false;
                btnAddFile.IsEnabled = true;
            }
        }

        private void radioUrl_Checked(object sender, RoutedEventArgs e)
        {
            if (radioUrl.IsChecked == true)
            {
                txtURL.IsEnabled = true;
                btnAddFile.IsEnabled = false;
            }
            else
            {
                txtURL.IsEnabled = false;
                btnAddFile.IsEnabled = true;
            }
        }

        private void btnAddFile_Click(object sender, RoutedEventArgs e)
        {
            if (radioFile.IsChecked == true)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.InitialDirectory = path;

                openFileDialog.RestoreDirectory = false;
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedFile = Path.GetFullPath(openFileDialog.FileName);
                    MessageBox.Show("You selected: " + selectedFile);
                    pictureManager.PathToFile = selectedFile;
                }
            }

        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            int index = lstPictures.SelectedIndex;

                pictureManager.RemovePictureFromList(index);
                imgBox.Source = null;
                PopulateList();

        }
    }
}
