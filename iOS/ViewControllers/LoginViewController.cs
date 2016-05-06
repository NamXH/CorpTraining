using System;
using UIKit;
using CoreGraphics;

namespace CorpTraining.iOS
{
    public class LoginViewController : UIViewController
    {
        public LoginViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Login";
            View.BackgroundColor = UIConstants.MainColor;

//            var logoImageView = new UIImageView();
//            View.AddSubview(logoImageView);
//            logoImageView.Image = UIImage.FromBundle("logo.png");
//            logo.Layer.BorderColor = new CGColor(255, 255, 255);
//            logo.Layer.BorderWidth = 1f;
//            logo.Layer.CornerRadius = 25f;

            var usernameTextField = new UITextField
            {
                Placeholder = "Email",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            View.AddSubview(usernameTextField);
            usernameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            usernameTextField.Layer.BorderWidth = UIConstants.BorderWidth;
            usernameTextField.Layer.CornerRadius = UIConstants.CornerRadius;

            var passwordTextField = new UITextField
            {
                Placeholder = "Password",
                BorderStyle = UITextBorderStyle.RoundedRect,
                SecureTextEntry = true,
            };
            View.AddSubview(passwordTextField);
            passwordTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            passwordTextField.Layer.BorderWidth = UIConstants.BorderWidth;
            passwordTextField.Layer.CornerRadius = UIConstants.CornerRadius;

            var loginButton = new UIButton(UIButtonType.System)
            {
                BackgroundColor = UIColor.FromHSB(UIConstants.MainColorHue + 0.08f, UIConstants.MainColorSaturation + 0.1f, UIConstants.MainColorBrightness),
                Font = UIFont.BoldSystemFontOfSize(UIConstants.NormalFontSize),
            };
            View.AddSubview(loginButton);
            loginButton.SetTitle("Login", UIControlState.Normal);
            loginButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            loginButton.Layer.CornerRadius = UIConstants.CornerRadius;
            loginButton.TouchUpInside += async (sender, e) =>
            {
                string token = null;
                try
                {
                    token = await UserUtil.AuthenticateUserAsync(usernameTextField.Text, passwordTextField.Text);
                }
                catch (Exception ex)
                {
                    var alert = UIAlertController.Create("Something goes wrong", String.Format("Please check your Internet connection and try again.{0} Details: {1}", Environment.NewLine, ex.Message), UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }

                if (!String.IsNullOrEmpty(token))
                {
                    UIApplication.SharedApplication.Windows[0].RootViewController = new TabViewController();
                }
                else
                {
                    var alert = UIAlertController.Create("Error", "Your email and password combination is incorrect.", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
            };

            var signUpButton = new UIButton(UIButtonType.System)
            {
                BackgroundColor = UIColor.White,
                Font = UIFont.BoldSystemFontOfSize(UIConstants.NormalFontSize),
            };
            View.AddSubview(signUpButton);
            signUpButton.SetTitle("Sign Up", UIControlState.Normal);
            signUpButton.SetTitleColor(UIColor.FromHSB(UIConstants.MainColorHue, UIConstants.MainColorSaturation, UIConstants.MainColorBrightness), UIControlState.Normal);
            signUpButton.Layer.CornerRadius = UIConstants.CornerRadius;
            signUpButton.TouchUpInside += (sender, e) =>
            {
                NavigationController.PushViewController(new RegistrationViewController(), true);
            };

            #region UI Layout
            var topPad = View.Frame.GetCenterY() - (UIConstants.ControlsHeight * 4 / 2) - UIConstants.ControlsHeight; // Half the total heights of all controls: the controls will be centered then go up a bit

            View.ConstrainLayout(() =>
//                logoImageView.Frame.Top == View.Frame.Top + topPad &&
//                logoImageView.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
//                logoImageView.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
//                logoImageView.Frame.GetCenterX() == View.Frame.GetCenterX() &&
//                logoImageView.Frame.Width == 500f &&

                usernameTextField.Frame.Top == View.Frame.Top + topPad &&
                usernameTextField.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                usernameTextField.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
                usernameTextField.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
                usernameTextField.Frame.Width <= UIConstants.MaximumControlsWidth &&
                usernameTextField.Frame.Height == UIConstants.ControlsHeight &&

                passwordTextField.Frame.Top == usernameTextField.Frame.Bottom + UIConstants.SmallGap &&
                passwordTextField.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                passwordTextField.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
                passwordTextField.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
                passwordTextField.Frame.Width <= UIConstants.MaximumControlsWidth &&
                passwordTextField.Frame.Height == UIConstants.ControlsHeight &&

                loginButton.Frame.Top == passwordTextField.Frame.Bottom + UIConstants.BigGap &&
                loginButton.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                loginButton.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
                loginButton.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
                loginButton.Frame.Width <= UIConstants.MaximumControlsWidth &&
                loginButton.Frame.Height == UIConstants.ControlsHeight &&

                signUpButton.Frame.Top == loginButton.Frame.Bottom + UIConstants.SmallGap &&
                signUpButton.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                signUpButton.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
                signUpButton.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
                signUpButton.Frame.Width <= UIConstants.MaximumControlsWidth &&
                signUpButton.Frame.Height == UIConstants.ControlsHeight
            );
            #endregion
        }
    }
}