using Microsoft.EntityFrameworkCore;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.Helpers;


namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class TestManageViewModel : ObservableObject
    {
        public GlobalState GlobalState { get; set; }

        public TestManageViewModel(ApplicationDbContext context, GlobalState globalState)
        {
            GlobalState = globalState;
        }
    }
}
