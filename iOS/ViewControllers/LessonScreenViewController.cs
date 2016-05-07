using System;
using UIKit;
using System.Collections.Generic;

namespace CorpTraining.iOS
{
    public class LessonScreenViewController : UIViewController
    {
        protected IList<Screen> Screens { get; set; }

        protected int Index { get; set; }

        public LessonScreenViewController(IList<Screen> screens, int index)
        {
            Screens = screens;
            Index = index;
        }

        protected virtual void PushNextScreen()
        {
            if (Index >= Screens.Count - 1) // Index is zero-based
            {
                return;
            }

            var lessonScreen = new LessonScreenViewController(Screens, Index + 1);
            NavigationController.PushViewController(lessonScreen, true);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (Index < Screens.Count - 1)
            {
                NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Next", UIBarButtonItemStyle.Plain, ((sender, e) =>
                        {
                            PushNextScreen();
                        })), true);
            }
            else
            {
                NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Submit", UIBarButtonItemStyle.Plain, ((sender, e) =>
                        {
                        })), true); 
            }

            var scrollView = new UIScrollView();

        }
    }
}

