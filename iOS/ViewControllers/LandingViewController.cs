using System;
using UIKit;

namespace CorpTraining.iOS
{
    public class LandingViewController : UIViewController
    {
        public LandingViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

//            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("landing.png"));

//            var getStartedButton = new UIButton(UIButtonType.System)
//            {
//                BackgroundColor = UIColor.FromHSB(UIConstants.MainColorHue, UIConstants.MainColorSaturation, UIConstants.MainColorBrightness),
//                Font = UIFont.BoldSystemFontOfSize(UIConstants.NormalFontSize),
//            };
//            View.AddSubview(getStartedButton);
//            getStartedButton.SetTitle("GET STARTED", UIControlState.Normal);
//            getStartedButton.SetTitleColor(UIColor.White, UIControlState.Normal);
//            getStartedButton.Layer.CornerRadius = 30f;
//            getStartedButton.TouchUpInside += (sender, e) =>
//            {
//                NavigationController.PushViewController(new LoginViewController(), true);
//            };

//            #region Layout
//            View.ConstrainLayout(() =>
//                getStartedButton.Frame.Top == View.Frame.GetCenterY() + 60f &&
//                getStartedButton.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
//                getStartedButton.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
//                getStartedButton.Frame.GetCenterX() == View.Frame.GetCenterX() &&
//                getStartedButton.Frame.Height == 70f &&
//                getStartedButton.Frame.Width == 300f 
//            );
//            #endregion
        }
    }
}

