using System;
using UIKit;
using System.Collections.Generic;

namespace CorpTraining.iOS
{
    public static class LessonScreenViewControllerGenerator
    {
        /// <summary>
        /// Generate the appropriate ViewController for the screen at index.
        /// </summary>
        public static UIViewController Generate(IList<Screen> screens, int index)
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
                case "audio_text_image_textlist":
                    result = new LessonScreenAudioViewController(screens, index);
                    break;
                case "audio_text":
                    result = new LessonScreenAudioViewController(screens, index);
                    break; 
                case "recorder":
                    result = new LessonScreenRecorderViewController(screens, index);
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }
    }
}

