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
        private ResultDTO _result;
        [ObservableProperty]
        private SubjectDTO _subject;
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

        #region Commands
        [RelayCommand]
        private void OnCopyResults()
        {
            StringBuilder resultBuilder = new StringBuilder(Subject.FullName);
            AppendCell(resultBuilder, Result.Test.Name);
            AppendCell(resultBuilder, Result.Date.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
            AppendRow(resultBuilder, "");
            AppendRow(resultBuilder, $"\"Average recognition time\nKnown: {AverageTimePerKnownImage} / Unknown: {AverageTimePerUnkownImage}\"");
            AppendCell(resultBuilder, $"\"Images Tested\n{Result.ImageSetTimes.Count}\"");
            AppendCell(resultBuilder, $"\"Max time allowed\n{Result.AvailableTimePerImageVariant}s\"");
            AppendRow(resultBuilder, "");
            AppendRow(resultBuilder, "IMAGE");
            AppendCell(resultBuilder, "RECOGNITION TIME");
            AppendCell(resultBuilder, "IS UNKNOWN?");

            for (int i = 0; i < Result.ImageSetTimes.Count; i++)
            {
                string imageName = Result.ImageSetTimes[i].ImageSet.Name;
                double imageRecognitionTime = Math.Round(Result.ImageSetTimes[i].Seconds, 2);
                string isUnknown = Result.ImageSetTimes[i].ImageSet.IsUnknown ? "Yes" : "No";
                AppendRow(resultBuilder, imageName);
                AppendCell(resultBuilder, imageRecognitionTime.ToString());
                AppendCell(resultBuilder, isUnknown);
            }

            Clipboard.SetText(resultBuilder.ToString());
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
        private void AppendCell(StringBuilder builder, string cell)
        {
            builder.Append($"\t{cell}");
        }
        private void AppendRow(StringBuilder builder, string row)
        {
            builder.Append($"\r\n{row}");
        }
        #endregion
    }
}
