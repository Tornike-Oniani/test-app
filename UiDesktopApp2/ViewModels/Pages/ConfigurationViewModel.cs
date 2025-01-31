using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class ConfigurationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _temp = "This is a configuration page";
        [ObservableProperty]
        private ObservableCollection<ImageSetDTO> _imageSets;
        [ObservableProperty]
        private List<string> _tempList;

        [RelayCommand]
        private void OnAddSet()
        {
            this.ImageSets.Add(new ImageSetDTO());
        }

        public ConfigurationViewModel()
        {
            this.ImageSets = new ObservableCollection<ImageSetDTO>()
            {
            };
            this.TempList = new List<string>()
            {
                "1",
                "2",
                "3",
                "4",
                "5"
            };
        }
    }
}
