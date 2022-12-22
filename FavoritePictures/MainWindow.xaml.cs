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
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
            AddPicture();
            
        }

        private void AddPicture()
        {
            int selectedAlbum = cmbAlbum.SelectedIndex;

            
            if (!string.IsNullOrEmpty(txtDescription.Text) && !string.IsNullOrEmpty(txtName.Text))
            {
                if (radioUrl.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(txtURL.Text))
                    {
                        AddPictureToPictureManager(selectedAlbum, txtURL.Text);
                        if(lstAlbums.SelectedIndex > -1 )
                        {
                            PopulateList();
                        }
                    }
                    else
                        MessageBox.Show("You need to add picture URL");
                }
                else
                {
                    if (!string.IsNullOrEmpty(pictureManager.PathToFile))
                    {
                        AddPictureToPictureManager(selectedAlbum, pictureManager.PathToFile);
                        if (lstAlbums.SelectedIndex > -1)
                        {
                            PopulateList();
                        }
                    }
                    else
                        MessageBox.Show("Add picture file");
                }
            }
            else
                MessageBox.Show("Name and Description can not be empty!");

            
            
        }

        private void AddPictureToPictureManager(int selectedAlbum,string uri)
        {
            Picture picture = new Picture(txtName.Text, uri, txtDescription.Text);
            if (pictureManager.albumList.Count > 0 && selectedAlbum > -1)
            {
                pictureManager.albumList[selectedAlbum].AddPictureToAlbum(picture);
                MessageBox.Show("Picture added to album: " + pictureManager.albumList[selectedAlbum].AlbumName);
                ClearInputs();
            }
            else if (pictureManager.albumList.Count <= 0)
            {
                MessageBox.Show("You didnt add any album! Create album first!");
            }
            else if (selectedAlbum < 0)
            {
                MessageBox.Show("You didnt select album in combobox!");
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedAlbum = lstAlbums.SelectedIndex;

            if (lstPictures.SelectedIndex > -1)
            {
                int index = lstPictures.SelectedIndex;

                Picture picture = pictureManager.albumList[selectedAlbum].GetPictureFromAlbum(index);

                var pictureBitmap = InitializePicture(picture.Url);

                imgBox.Source = pictureBitmap;
            }
        }

        private void menuOpenFrom_Click(object sender, RoutedEventArgs e)
        {
            pictureManager.ReadAlbumsFromTxt();
            PopulateAlbums();
        }

        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            pictureManager.SaveAlbumToTxt();
        }

        private void menuNew_Click(object sender, RoutedEventArgs e)
        {
            lstPictures.Items.Clear();
            lstAlbums.Items.Clear();
            txtDescription.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtURL.Text = string.Empty;
            imgBox.Source = null;
            InitializeGUI();
            pictureManager.ClearManager();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PopulateAlbums()
        {
            lstAlbums.Items.Clear();
            for (int i = 0; i < pictureManager.albumList.Count; i++)
            {
                string albumName = pictureManager.albumList[i].AlbumName;

            lstAlbums.Items.Add(albumName);
            }

            // when you click add album this needs to deselect all listboxes otherwise it will bug program
            lstAlbums.SelectedIndex = -1;
            lstPictures.SelectedIndex = -1;

        }


        private void PopulateList()
        {
            int selectedAlbum = lstAlbums.SelectedIndex;

            lstPictures.Items.Clear();
            if (pictureManager.albumList.Count > 0 && CheckIndex(selectedAlbum))
            {
                for (int i = 0; i < pictureManager.albumList[selectedAlbum].GetPicturesCount(); i++)
                {   
                    Picture picture = pictureManager.GetPictureFromAlbum(selectedAlbum,i);
                    lstPictures.Items.Add(string.Format("{0,-15} {1,-30} {2,-30}", picture.Name, picture.Url, picture.Description));
                }
            }
        }

        private bool CheckIndex(int index)
        {
            if (index > -1)
                return true;
            else return false;
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
                    lblPath.Content = "File is saved in app";
                    lblPath.Foreground = System.Windows.Media.Brushes.Green;
                }
            }

        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            int selectedAlbum = lstAlbums.SelectedIndex;
            int selectedPicture = lstPictures.SelectedIndex;

                pictureManager.RemovePictureFromAlbum(selectedAlbum,selectedPicture);
                imgBox.Source = null;
                PopulateList();
        }

        private void btnAddAlbumName_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtAlbumName.Text))
            {
                Album album = new Album()
                {
                    AlbumName = txtAlbumName.Text
                };
                pictureManager.albumList.Add(album);
                PopulateAlbums();
                txtAlbumName.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Album name cant be empty! Its not fun when you cant categorize your pictures! Remember to always organize your files!");
            }
        }

        private void cmbAlbum_DropDownOpened(object sender, EventArgs e)
        {
            cmbAlbum.Items.Clear();
            var listOfAlbums = pictureManager.albumList.Count;
            if (listOfAlbums > 0)
                for (int i = 0; i < listOfAlbums; i++)
                {
                    cmbAlbum.Items.Add(pictureManager.albumList[i].AlbumName);
                }

        }

        private void lstAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // this unselects the item in the first listbox with pictures
            lstPictures.SelectedIndex = -1;
            // when album is changed the picture is hidden
            imgBox.Source = null;

            PopulateList();
        }

        private void ClearInputs()
        {
            txtAlbumName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtName.Text = string.Empty;
            txtURL.Text = string.Empty;

            // setting the saved path to file in manager to empty string
            pictureManager.PathToFile = string.Empty;
            lblPath.Content = string.Empty;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.N) &&
                (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                MessageBox.Show("You pressed Ctrl+n so the app was cleaned!");

                menuNew_Click(sender, e);
            }

        }

        private void btnDeleteAlbum_Click(object sender, RoutedEventArgs e)
        {
            int selectedAlbum = lstAlbums.SelectedIndex;

            if (CheckIndex(selectedAlbum))
            {
                string message = "Do you want to delete album named: " + lstAlbums.SelectedItem.ToString() + "\n This action cannot be undone!";

                ConfirmationWindow confirmationWindow = new ConfirmationWindow(message);
                confirmationWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                confirmationWindow.ShowDialog();

                if (confirmationWindow.DialogResult == true)
                {
                    pictureManager.RemoveAlbum(selectedAlbum);
                    PopulateAlbums();
                }
            }
        }
    }
    
}

