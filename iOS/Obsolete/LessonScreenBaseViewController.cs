using System;
using UIKit;
using System.Collections.Generic;

namespace CorpTraining.iOS
{
    public class LessonScreenBaseViewController : UIViewController
    {
        protected IList<Screen> Screens { get; set; }

        protected int Index { get; set; }

        public LessonScreenBaseViewController(IList<Screen> screens, int index)
        {
            Screens = screens;
            Index = index;
        }

        protected virtual void PushNextScreen()
        {
            if (Index > Screens.Count - 1)
            {
                return;
            }
            else
            {
                // Display alert if needed!!
            }

            var lessonScreen = LessonScreenViewControllerGenerator.Generate(Screens, Index + 1);
            if (lessonScreen != null)
            {
                NavigationController.PushViewController(lessonScreen, true);
            }
            else
            {
                // Display alert if needed !!
            }
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
        }
    }
}

