using System.Collections.ObjectModel;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.Helpers
{
    public partial class GlobalState : ObservableObject
    {
        public ObservableCollection<TestDTO> Tests { get; set; } = new ObservableCollection<TestDTO>();

        [ObservableProperty]
        private TestDTO? _testToManage;

        [ObservableProperty]
        private TestDTO? _testToRun;
    }
}
