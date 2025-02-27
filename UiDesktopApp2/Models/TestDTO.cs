using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.Models
{
    public class TestDTO : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasAlreadyRun { get; set; }
        public ObservableCollection<ImageSetDTO> ImageSets { get; set; }
        public bool ContainsEmptyImageSet 
        {
            get
            {
                return ImageSets.Count == 0 || ImageSets.Any(ist => ist.Images.Count == 0);
            } 
        }

        public TestDTO()
        {
            this.ImageSets = new ObservableCollection<ImageSetDTO>();
        }

        public void CheckForEmptySets()
        {
            OnPropertyChanged("ContainsEmptyImageSet");
        }
    }
}
