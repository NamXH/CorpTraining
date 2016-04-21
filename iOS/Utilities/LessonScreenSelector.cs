using System;
using UIKit;
using System.Collections.Generic;

namespace CorpTraining.iOS
{
    public static class LessonScreenSelector
    {
        public static UIViewController Select(IList<Screen> screens, int index)
        {
            if ((index > screens.Count - 1) || (index < 0))
            {
                return null;
            }

            UIViewController result = null;

            switch (screens[index].Type)
            {
                case "video":
                    result = new LessonScreenVideoViewController(screens, index);
                    break;
                case "audio_question":
                    result = new LessonScreenAudioViewController(screens, index);
                    break;
                case "audio_recorder":
                    result = new LessonScreenRecorderViewController(screens, index);
                    break;
                default:
                    result = new UIViewController();
                    result.View.BackgroundColor = UIColor.White;
                    break;
            }

            return result;
        }
    }
}

