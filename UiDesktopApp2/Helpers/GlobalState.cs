using System.Collections.ObjectModel;
using UiDesktopApp2.DataAccess.Entities;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.Helpers
{
    public partial class GlobalState : ObservableObject
    {
        public ObservableCollection<TestDTO> Tests { get; set; } = new ObservableCollection<TestDTO>();
        public ObservableCollection<SubjectDTO> Subjects { get; set; } = new ObservableCollection<SubjectDTO>();

        [ObservableProperty]
        private SubjectDTO _subjectToTest;
        [ObservableProperty]
        private ResultDTO _currentTestResult;
        [ObservableProperty]
        private TestDTO? _testToManage;
        [ObservableProperty]
        private TestDTO? _testToRun;
    }
}
