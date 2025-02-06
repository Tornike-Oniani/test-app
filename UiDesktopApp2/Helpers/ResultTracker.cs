using UiDesktopApp2.Models;

namespace UiDesktopApp2.Helpers
{
    public class ResultTracker
    {
        private double _imageSetTime = 0;
        private readonly ResultDTO _result;

        public ResultTracker(int testId, GlobalState globalState)
        {
            _result = new ResultDTO()
            {
                TestId = testId,
                ImageSetTimes = new List<ResultImageSetTimeDTO>(),
                VariantTimes = new List<ResultImageVariantTimeDTO>()
            };
            globalState.CurrentTestResult = _result;
            globalState.SubjectToTest.Results.Add(_result);
        }

        public void TrackResult(double timeElapsed, bool isNextImageAvailable)
        {
            _imageSetTime += timeElapsed;
            _result.VariantTimes.Add(new ResultImageVariantTimeDTO()
            {
                Seconds = (int)timeElapsed
            });

            // If this was the last image in set, track how much time was needed for the whole set
            if (!isNextImageAvailable)
            {
                _result.ImageSetTimes.Add(new ResultImageSetTimeDTO()
                {
                    Seconds = (int)_imageSetTime
                });
                _imageSetTime = 0;
            }
        }
    }
}
