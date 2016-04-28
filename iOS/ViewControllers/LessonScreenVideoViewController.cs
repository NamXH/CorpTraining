using System;
using UIKit;
using System.Collections.Generic;
using AVFoundation;
using Foundation;
using AVKit;

namespace CorpTraining.iOS
{
    public class LessonScreenVideoViewController : LessonScreenBaseViewController
    {
        public LessonScreenVideoViewController(IList<Screen> screens, int index)
            : base(screens, index)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            var player = new AVPlayer(NSUrl.FromString(Screens[Index].VideoUrl));
            var playerViewController = new AVPlayerViewController
            {
                Player = player,      
            };
            AddChildViewController(playerViewController);
            View.AddSubview(playerViewController.View);

            #region Layout

            View.ConstrainLayout(() =>
                playerViewController.View.Frame.GetCenterY() == View.Frame.GetCenterY() &&
                playerViewController.View.Frame.Left == View.Frame.Left &&
                playerViewController.View.Frame.Right == View.Frame.Right
//                playerViewController.View.Frame.Height >= 240f
            );
            #endregion
        }
    }
}

