using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.Models
{
    public partial class ResultDTO : ObservableObject
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double AvailableTimePerImageVariant { get; set; }
        public int TestId { get; set; }
        public TestDTO Test { get; set; }
        public int SubjectId { get; set; }
        public List<ResultImageSetTimeDTO> ImageSetTimes { get; set; }

        [ObservableProperty]
        private bool _isFlyoutOpen;
    }
}
