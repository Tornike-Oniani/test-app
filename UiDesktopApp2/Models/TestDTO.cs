using System.Collections.ObjectModel;

namespace UiDesktopApp2.Models
{
    public partial class TestDTO : ObservableObject
    {
        public int Id { get; set; }
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private bool _hasAlreadyRun;
        public ObservableCollection<ImageSetDTO> ImageSets { get; set; }
        [ObservableProperty]
        private bool _isFlyoutOpen;
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
