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

            var signOutButton = new UIButton(UIButtonType.RoundedRect)
            {
                BackgroundColor = UIColor.Red,
                Font = UIFont.BoldSystemFontOfSize(UIConstants.NormalFontSize),
            };
            View.AddSubview(signOutButton);
            signOutButton.SetTitle("Sign Out", UIControlState.Normal);
            signOutButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            signOutButton.Layer.CornerRadius = UIConstants.CornerRadius;
            signOutButton.TouchUpInside += async (sender, e) =>
            {
                signOutButton.Enabled = false;
                UserUtil.LogOutUserByTokenAsync(UserUtil.GetCurrentToken());

                var loginViewController = new LoginViewController();
                UIApplication.SharedApplication.Windows[0].RootViewController = new UINavigationController(loginViewController);
                signOutButton.Enabled = true;
            };

            View.ConstrainLayout(() =>
                signOutButton.Frame.Left == View.Frame.Left + UIConstants.HorizontalPad &&
                signOutButton.Frame.Right == View.Frame.Right - UIConstants.HorizontalPad &&
                signOutButton.Frame.GetCenterY() == View.Frame.GetCenterY() &&
                signOutButton.Frame.Height == UIConstants.ControlsHeight
            );
        }
    }
}
