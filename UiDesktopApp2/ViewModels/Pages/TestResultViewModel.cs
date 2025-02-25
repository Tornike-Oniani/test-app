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
        #region Private members
        #endregion

        #region Observable properties
        [ObservableProperty]
        private ResultDTO result;
        [ObservableProperty]
        private SubjectDTO subject;
        #endregion

        #region Constructors
        public TestResultViewModel(GlobalState globalState, SubjectRepository subjectRepo)
        {
            Result = globalState.ResultToBrowse;
            Initialize(subjectRepo);
        }
        #endregion

        #region Private helpers
        private async Task Initialize(SubjectRepository subjectRepo)
        {
            Subject = await subjectRepo.GetSubjectWithId(Result.SubjectId);
        }
        #endregion
    }
}
