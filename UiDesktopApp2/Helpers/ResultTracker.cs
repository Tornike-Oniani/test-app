using System.Diagnostics;
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

        public void TrackResult(double timeElapsed, bool isNextImageAvailable, bool skipped = false, bool recognized = false)
        {
            _imageSetTime += timeElapsed;
            _result.VariantTimes.Add(new ResultImageVariantTimeDTO()
            {
                Seconds = timeElapsed,
                Skipped = skipped
            });

            // If image was recognized track how much time it took to finish the set
            if (recognized)
            {
                _result.ImageSetTimes.Add(new ResultImageSetTimeDTO()
                {
                    Seconds = _imageSetTime,
                    Recognized = recognized
                });
                _imageSetTime = 0;
                return;
            }

            // If this was the last image in set, track how much time was needed for the whole set if the image was not recognized
            if (!isNextImageAvailable)
            {
                Trace.WriteLine($"Sum time: {_imageSetTime}");
                _result.ImageSetTimes.Add(new ResultImageSetTimeDTO()
                {
                    Seconds = _imageSetTime,
                    Recognized = recognized
                });
                _imageSetTime = 0;
            }
        }
    }
}
