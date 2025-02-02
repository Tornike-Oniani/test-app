using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.Models
{
    public class TestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<ImageSetDTO> ImageSets { get; set; }

        public TestDTO()
        {
            this.ImageSets = new ObservableCollection<ImageSetDTO>();
        }
    }
}
