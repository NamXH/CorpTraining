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
            View.BackgroundColor = UIColor.FromHSB(UIConstants.MainColorHue, UIConstants.MainColorSaturation, UIConstants.MainColorBrightness);

            var logoImageView = new UIImageView();
            View.AddSubview(logoImageView);
            logoImageView.Image = UIImage.FromBundle("logo.png");
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
            loginButton.TouchUpInside += (sender, e) =>
            {
                NavigationController.PushViewController(new TabViewController(), true);
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

            #region UI Layout
//            var topPad = (float)NavigationController.NavigationBar.Frame.Size.Height + UIConstants.VerticalPad + 30f;
            var topPad = View.Frame.GetCenterY() - 170f - 100f; // 170 is half the total heights of all controls

            View.ConstrainLayout(() =>
                logoImageView.Frame.Top == View.Frame.Top + topPad &&
                logoImageView.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
                logoImageView.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
                logoImageView.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                logoImageView.Frame.Width == 500f &&

                usernameTextField.Frame.Top == logoImageView.Frame.Bottom + 40f &&
                usernameTextField.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
                usernameTextField.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
                usernameTextField.Frame.Height == UIConstants.ControlsHeight &&
                usernameTextField.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                usernameTextField.Frame.Width <= UIConstants.MaximumControlsWidth &&

                passwordTextField.Frame.Top == usernameTextField.Frame.Bottom + 10f &&
                passwordTextField.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
                passwordTextField.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
                passwordTextField.Frame.Height == UIConstants.ControlsHeight &&
                passwordTextField.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                passwordTextField.Frame.Width <= UIConstants.MaximumControlsWidth &&

                loginButton.Frame.Top == passwordTextField.Frame.Bottom + 40f &&
                loginButton.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
                loginButton.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
                loginButton.Frame.Height == UIConstants.ControlsHeight &&
                loginButton.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                loginButton.Frame.Width <= UIConstants.MaximumControlsWidth &&

                signUpButton.Frame.Top == loginButton.Frame.Bottom + 10f &&
                signUpButton.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
                signUpButton.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
                signUpButton.Frame.Height == UIConstants.ControlsHeight &&
                signUpButton.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                signUpButton.Frame.Width <= UIConstants.MaximumControlsWidth
            );
            #endregion
        }
    }
}