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
using itk.simple;
using System.ComponentModel;

namespace SegmentacjaGuzowMozgu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class BindingModel:INotifyPropertyChanged
    {
        #region private members
        string zoomValue;
        bool imageIsOpen;
        string actualImage;
        string imageSize;
        #endregion

        #region accessors
        public string ZoomValue
        {
            get { return zoomValue; }
            set { zoomValue = value; NotifyPropertyChanged("ZoomValue"); }
        }

        public string ActualImage
        {
            get { return actualImage; }
            set { actualImage = value; NotifyPropertyChanged("ActualImage"); }
        }

        public string ImageSize 
        {
            get { return imageSize; }
            set { imageSize = value; NotifyPropertyChanged("ImageSize"); }
        }

        public bool ImageIsOpen
        {
            get { return imageIsOpen; }
            set { imageIsOpen = value; NotifyPropertyChanged("ImageIsOpen"); }
        }
        #endregion

        #region inotifyproperty
        protected virtual void NotifyPropertyChanged(String propertyName = "")
            {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            bm=new BindingModel();
            bm.ZoomValue = "";
            bm.ImageIsOpen = false;
            bm.ActualImage = "";
            bm.ImageSize = "";
            this.DataContext = bm;
        }

        #region private members
        BindingModel bm;
        itk.simple.Image image;
        System.Drawing.Bitmap[] showImage;
        BitmapImage[] bi;
        int currentImage;
        bool imageIsOpen=false;
        string imageExtension;
        #endregion


        #region methods

        void ShowImage()
        {
            try
            {
                if (image == null) { return; }
                scrollViewer1.Visibility = Visibility.Visible;
                btnLeft.IsEnabled = false;
                btnRight.IsEnabled = false;
                ReadingImage ri = new ReadingImage(image);
                showImage = ri.CreateBitmapLayer(image);
                int n = showImage.Length;

                bi = new BitmapImage[n];
                for (int i = 0; i < n; i++)
                {
                    bi[i] = ri.ToWpfBitmap(showImage[i]);
                }
                ImageFrame.Source = bi[0] as BitmapSource;

                //release resources
                foreach (System.Drawing.Bitmap b in showImage)
                    b.Dispose();
                if (n > 1) btnRight.IsEnabled = true;
                currentImage = 0;
                bm.ImageIsOpen = true;
                imageIsOpen = bm.ImageIsOpen;
                bm.ZoomValue = "100 %";
                bm.ActualImage = "Widok:  " + (currentImage + 1) + " z " + n + "  ";
                bm.ImageSize = "|         Rozmiar: " + image.GetSize()[0] + "x" + image.GetSize()[1] + " ";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        void OpenImage()
        {
            try
            {
                if (image != null) image.Dispose();
                OpeningImage oi = new OpeningImage();
                oi.ReadImage();
                imageExtension = oi.Extension;
                image = oi.Image;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        #endregion

        #region MenuItems events
        private void open_Click(object sender, RoutedEventArgs e)
        {
            OpenImage();
            ShowImage();
        }

        private void zamknij_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (bm.ImageIsOpen)
            {
                try
                {
                    SavingImage si = new SavingImage(image, imageExtension);
                    si.SaveImage();
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }
        #endregion

        #region events

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            int n = showImage.Length;
            if (currentImage <= 1)
            {
                btnLeft.IsEnabled = false;
            }
            currentImage--;
            ImageFrame.Source = bi[currentImage] as BitmapSource;
            btnRight.IsEnabled = true;
            bm.ActualImage = "Widok:  " + (currentImage + 1) + " z " + n + "  ";
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            int n = showImage.Length;
            if (currentImage + 1 >= n - 1)
            {
                btnRight.IsEnabled = false;
            }
            currentImage++;
            ImageFrame.Source = bi[currentImage] as BitmapSource;
            btnLeft.IsEnabled = true;
            bm.ActualImage = "Widok:  " + (currentImage + 1) + " z " + n + "  ";
        }

        private void btnFiltry_Click(object sender, RoutedEventArgs e)
        {
            if (Toolbox.Visibility == Visibility.Collapsed)
                Toolbox.Visibility = Visibility.Visible;
            else
                Toolbox.Visibility = Visibility.Collapsed;
        }

        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (imageIsOpen)
            {
                double zoom = zoomSlider.Value * 100;
                bm.ZoomValue = zoom + " %";
            }
        }
        #endregion

    }
}
