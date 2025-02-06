using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.Helpers
{
    public class ImageDisplay
    {
        private int setIndex = 0;
        private int imageIndex = -1;

        public List<ImageSetDTO> ImageSets { get; set; }

        public string GetNextImage()
        {
            if (IsNextImageAvailable())
            {
                imageIndex++;
            }
            else if (IsNextSetAvailable())
            {
                imageIndex = 0;
                setIndex++;
            }
            else
            {
                return null;
            }

            return GetDisplayImageSource();
        }

        public bool IsNextSetAvailable()
        {
            return setIndex + 1 < ImageSets.Count;
        }
        public bool IsNextImageAvailable()
        {
            return imageIndex + 1 < ImageSets[setIndex].Images.Count;
        }

        private string GetDisplayImageSource()
        {
            return ImageSets[setIndex].Images[imageIndex].Source;
        }
    }
}
