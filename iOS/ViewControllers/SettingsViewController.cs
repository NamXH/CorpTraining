using System;
using UIKit;

namespace CorpTraining.iOS
{
    public class SettingsViewController : UIViewController
    {
        public SettingsViewController()
            : base()
        {
            Title = "Settings";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

//            var signOutButton = new UIButton(UIButtonType.RoundedRect)
//            {
//                BackgroundColor = UIColor.Red,
//                Font = UIFont.BoldSystemFontOfSize(UIConstants.NormalFontSize),
//            };
//            View.AddSubview(signOutButton);
//            signOutButton.SetTitle("Sign Out", UIControlState.Normal);
//            signOutButton.SetTitleColor(UIColor.White, UIControlState.Normal);
//            signOutButton.Layer.CornerRadius = UIConstants.CornerRadius;
//
//            View.BackgroundColor = UIColor.White;
//
//            #region Layout
//            View.ConstrainLayout(() =>
//                signOutButton.Frame.Left == View.Frame.Left + 300f &&
//                signOutButton.Frame.Right == View.Frame.Right - 300f &&
//                signOutButton.Frame.Bottom == View.Frame.Bottom - 100f &&
//                signOutButton.Frame.Height == UIConstants.ControlsHeight
//            );
//            #endregion
        }
    }
}
