using System.Drawing;
using System.Windows.Media;
using ImageGrabber.Wpf.Extensions;

namespace ImageGrabber.Application.Models
{
    public class GrabbedImageItem
    {
        #region Properties
        public string CameraName { get; private set; }
        public string GrabbedTime { get; private set; }
        public bool IsSelectedToSave { get; private set; }
        public Bitmap Image { get; private set; }
        public ImageSource ShowIamge { get; private set; }

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
            ShowIamge = image.ToImageSource();
        }

        public GrabbedImageItem(GrabbedImageItem item)
        {
            this.CameraName = item.CameraName;
            this.GrabbedTime = item.GrabbedTime;
            this.IsSelectedToSave = item.IsSelectedToSave;
            this.Image = item.Image;
            this.ShowIamge = Image.ToImageSource();
        }
        #endregion
    }
}
