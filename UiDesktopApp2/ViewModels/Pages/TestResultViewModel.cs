using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiDesktopApp2.DataAccess.Repositories;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TestResultViewModel : ObservableObject
    {
        #region Observable properties
        [ObservableProperty]
        private ResultDTO result;
        [ObservableProperty]
        private SubjectDTO subject;
        [ObservableProperty]
        private double _averageTimePerKnownImage = 0;
        [ObservableProperty]
        private double _averageTimePerUnkownImage = 0;
        #endregion

        #region Constructors
        public TestResultViewModel(GlobalState globalState, SubjectRepository subjectRepo)
        {
            Result = globalState.ResultToBrowse;
            CalculateAvaregaRecognitionTimes();
            Initialize(subjectRepo);
        }
        #endregion

        #region Private helpers
        private async Task Initialize(SubjectRepository subjectRepo)
        {
            Subject = await subjectRepo.GetSubjectWithId(Result.SubjectId);
        }
        private void CalculateAvaregaRecognitionTimes()
        {
            AverageTimePerKnownImage = Math.Round(
                Result.ImageSetTimes.Where(ist => !ist.ImageSet.IsUnknown).Sum(ist => ist.Seconds)
                /
                Result.ImageSetTimes.Count(ist => !ist.ImageSet.IsUnknown),
                2
                );
            AverageTimePerUnkownImage = Math.Round(
                Result.ImageSetTimes.Where(ist => ist.ImageSet.IsUnknown).Sum(ist => ist.Seconds)
                /
                Result.ImageSetTimes.Count(ist => ist.ImageSet.IsUnknown),
                2
                );
        }
        #endregion
    }
}
