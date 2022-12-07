using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageGrabber.Wpf.Extensions;
using Prism.Mvvm;

namespace ImageGrabber.Application.Models
{
    /// <summary>
    /// grabbed image item object
    /// </summary>
    public class GrabbedImageItem : BindableBase
    {
        #region Private Fileds
        private bool _isSelectedToSave;
        #endregion

        #region Properties
        /// <summary>
        /// camera name
        /// </summary>
        public string CameraName { get; private set; }
        /// <summary>
        /// grabbed time
        /// </summary>
        public string GrabbedTime { get; private set; }
        /// <summary>
        /// if this item is selected from view then return <see langword="true"/>, otherwise return <see langword="false"/>
        /// </summary>
        public bool IsSelectedToSave
        {
            get => _isSelectedToSave;
            set => SetProperty(ref _isSelectedToSave, value);
        }
        /// <summary>
        /// grabbed image
        /// </summary>
        public Bitmap Image { get; private set; }
        /// <summary>
        /// convert bitmap to image source for showing in the view controller
        /// </summary>
        public ImageSource ShowImage { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public GrabbedImageItem(string cameraName, string grabbedTime, bool isSelectedToSave, Bitmap image)
        {
            CameraName = cameraName;
            GrabbedTime = grabbedTime;
            IsSelectedToSave = isSelectedToSave;
            Image = image;
            ShowImage = image.ToImageSource();
        }

        public GrabbedImageItem(GrabbedImageItem item)
        {
            this.CameraName = item.CameraName;
            this.GrabbedTime = item.GrabbedTime;
            this.IsSelectedToSave = item.IsSelectedToSave;
            this.Image = item.Image;
            this.ShowImage = Image.ToImageSource();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// save image
        /// </summary>
        public void Save()
        {
            if (false == Directory.Exists($@"./out")) Directory.CreateDirectory(@$"./out");

            using (var fileStream = new FileStream($@"./out/{CameraName}_{DateTime.Parse(GrabbedTime):yyyyMMddHHmmssfff}.bmp", FileMode.CreateNew))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(ShowImage as BitmapSource));
                encoder.Save(fileStream);
            }
        }
        #endregion
    }
}
