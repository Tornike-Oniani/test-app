using System.Diagnostics;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.Helpers
{
    public class ImageDisplay
    {
        private int _setIndex = 0;
        private int _imageIndex = 0;

        public List<ImageSetDTO> ImageSets { get; set; }

        public string GetCurrentImage()
        {
            return GetDisplayImageSource();
        }

        public string GetNextImage()
        {
            if (IsNextImageAvailable())
            {
                _imageIndex++;
            }
            else if (IsNextSetAvailable())
            {
                _imageIndex = 0;
                _setIndex++;
            }
            else
            {
                return null;
            }

            return GetDisplayImageSource();
        }
        public string JumpToNextSet()
        {
            if (!IsNextSetAvailable())
            {
                return null;
            }
            _setIndex++;
            _imageIndex = 0;
            return GetDisplayImageSource();
        }

        public bool IsNextSetAvailable()
        {
            return _setIndex + 1 < ImageSets.Count;
        }
        public bool IsNextImageAvailable(bool trace = false)
        {
            return _imageIndex + 1 < ImageSets[_setIndex].Images.Count;
        }

        private string GetDisplayImageSource()
        {
            return ImageSets[_setIndex].Images[_imageIndex].Source;
        }
    }
}
