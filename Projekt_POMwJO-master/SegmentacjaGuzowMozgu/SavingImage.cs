using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using itk.simple;

namespace SegmentacjaGuzowMozgu
{
    class SavingImage
    {
        #region private members
        Image image;
        string extension;
        #endregion

        #region constructors
        public SavingImage() { }
        public SavingImage(itk.simple.Image _image) { image = _image; }
        public SavingImage(string _extension) { extension = _extension; }
        public SavingImage(itk.simple.Image _image, string _extension) { image = _image; extension = _extension; }
        #endregion

        #region accessors
        public Image Image
        {
            get { return image; }
            set { image = value; }
        }
        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }
        #endregion

        #region methods
        string getImagePath_Save()
        {
            string path = null;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.CheckPathExists = true;
            string ext = "Image File *." + extension + "|*." + extension + ";";
            dlg.DefaultExt = ext;
            //dlg.Filter = "Image Files (*.dcm, *.png, *.jpg, *.bmp, *.tif)|*.dcm;*.png;*.jpg;*.bmp;*.tif";
            dlg.Filter = ext;
            Nullable<bool> result = dlg.ShowDialog();
            if (result.Value)
            {
                path = dlg.FileName;
            }
            return path;
        }

        public void SaveImage()
        {
            string path = getImagePath_Save();
            if (!string.IsNullOrEmpty(path))
            {
                ImageFileWriter writer = new ImageFileWriter();
                writer.SetFileName(path);
                writer.Execute(image);
            }
            else
            { //exception}
            }
        #endregion
        }
    }
}