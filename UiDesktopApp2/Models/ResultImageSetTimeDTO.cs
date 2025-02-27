using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.Models
{
    public class ResultImageSetTimeDTO
    {
        public int Id { get; set; }
        public double Seconds { get; set; }
        public bool Recognized { get; set; }
        public int ImageSetId { get; set; }
        public ImageSetDTO ImageSet { get; set; }
        public int ResultId { get; set; }
    }
}
