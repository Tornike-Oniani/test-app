using System.Collections.ObjectModel;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.Helpers
{
    public partial class GlobalState : ObservableObject
    {
        public ObservableCollection<TestDTO> Tests { get; set; } = new ObservableCollection<TestDTO>();
        public ObservableCollection<PersonDTO> Subjects { get; set; } = new ObservableCollection<PersonDTO>();

        [ObservableProperty]
        private PersonDTO _subjectToTest;
        [ObservableProperty]
        private TestDTO? _testToManage;
        [ObservableProperty]
        private TestDTO? _testToRun;
    }
}
