using System.Diagnostics;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.Helpers
{
    public class ResultTracker
    {
        private double _imageSetTime = 0;
        private readonly ResultDTO _result;

        public ResultTracker(int testId, int subjectId, GlobalState globalState)
        {
            _result = new ResultDTO()
            {
                TestId = testId,
                SubjectId = subjectId,
                ImageSetTimes = new List<ResultImageSetTimeDTO>()
            };
            globalState.CurrentTestResult = _result;
            if (globalState.SubjectToTest.Results == null)
            {
                globalState.SubjectToTest.Results = new List<ResultDTO>();
            }
            globalState.SubjectToTest.Results.Add(_result);
        }

        public void TrackResult(
            int imageSetId,
            double timeElapsed, 
            bool isNextImageAvailable, 
            bool recognized = false
            )
        {
            _imageSetTime += timeElapsed;

            // If this was the last image in set, track how much time was needed for the whole set if the image was not recognized
            if (!isNextImageAvailable || recognized)
            {
                _result.ImageSetTimes.Add(new ResultImageSetTimeDTO()
                {
                    ResultId = 0,
                    ImageSetId = imageSetId,
                    Seconds = _imageSetTime,
                    Recognized = recognized
                });
                _imageSetTime = 0;
            }
        }
        public ResultDTO GetResult()
        {
            return _result;
        }
    }
}
