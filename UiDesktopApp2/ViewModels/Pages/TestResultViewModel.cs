﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TestResultViewModel(GlobalState globalState) : ObservableObject
    {
        [ObservableProperty]
        private SubjectDTO _person = globalState.SubjectToTest;
        [ObservableProperty]
        private ResultDTO _result = globalState.CurrentTestResult;
    }
}
