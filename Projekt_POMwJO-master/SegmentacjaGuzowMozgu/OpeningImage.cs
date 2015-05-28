using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using itk.simple;


namespace SegmentacjaGuzowMozgu
{
    class OpeningImage
    {
        #region private members
        string path=null;
        string extension=null;
        Image image;
        #endregion

        #region constructors
        public OpeningImage() { }
        #endregion

        #region accessors
        public string Path
        {
            get { return path; }
        }

        public string Extension
        {
            get { return extension; }
        }

        public Image Image
        {
            get { return image; }
        }
        #endregion

        #region methods
        void getImagePath()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg. CheckFileExists = true;
            dlg. CheckPathExists = true;
            dlg.Filter = "Image Files (*.dcm, *.png, *.jpg, *.bmp, *.tif)|*.dcm;*.png;*.jpg;*.bmp;*.tif";
            dlg.AddExtension = true;
            Nullable<bool> result = dlg.ShowDialog();
            if (result.Value)
            {
                path = dlg.FileName;
            }
        }

        public void ReadImage()
        {
            getImagePath();

            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    ImageFileReader reader = new ImageFileReader();
                    reader.SetFileName(path);
                    image = reader.Execute();
                    extension = path.Split('.').ElementAt(1);
                }
                catch(Exception ex) { }
            }
        }
        #endregion
    }
}
