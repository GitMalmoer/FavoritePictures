using System;
using System.Collections.Generic;
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
            //InitializePicture("http://cdn.differencebetween.net/wp-content/uploads/2012/01/Difference-Between-Example-and-Sample.jpg");
        }

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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Picture picture = new Picture(txtName.Text,txtURL.Text,txtDescription.Text);
            pictureManager.AddPictureToList(picture);

            PopulateList();

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = lstPictures.SelectedIndex;
            Picture picture = pictureManager.GetPictureFromList(index);

            var pictureBitmap = InitializePicture(picture.Url);

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

        private void PopulateList()
        {
            lstPictures.Items.Clear();
            if (pictureManager.GetPictureListCount() > 0)
            {
                for (int i = 0; i < pictureManager.GetPictureListCount(); i++)
                {
                    Picture picture = pictureManager.GetPictureFromList(i);
                    lstPictures.Items.Add(string.Format("{0,30} {1,30} {2,30}", picture.Name, picture.Url, picture.Description));
                }
            }
        }
    }
}
