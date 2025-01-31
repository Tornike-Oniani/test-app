using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.Models
{
    public class TestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        List<ImageSetDTO> ImageSets { get; set; }
    }
}
