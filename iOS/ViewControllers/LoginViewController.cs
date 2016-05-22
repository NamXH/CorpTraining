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
            View.BackgroundColor = Constants.MainColor;

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
                AutocapitalizationType = UITextAutocapitalizationType.None,
                KeyboardType = UIKeyboardType.EmailAddress,
            };
            View.AddSubview(usernameTextField);
            usernameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            usernameTextField.Layer.BorderWidth = Constants.BorderWidth;
            usernameTextField.Layer.CornerRadius = Constants.CornerRadius;

            var passwordTextField = new UITextField
            {
                Placeholder = "Password",
                BorderStyle = UITextBorderStyle.RoundedRect,
                SecureTextEntry = true,
            };
            View.AddSubview(passwordTextField);
            passwordTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            passwordTextField.Layer.BorderWidth = Constants.BorderWidth;
            passwordTextField.Layer.CornerRadius = Constants.CornerRadius;

            var loginButton = new UIButton(UIButtonType.System)
            {
                BackgroundColor = UIColor.FromHSB(Constants.MainColorHue + 0.08f, Constants.MainColorSaturation + 0.1f, Constants.MainColorBrightness),
                Font = UIFont.BoldSystemFontOfSize(Constants.NormalFontSize),
            };
            View.AddSubview(loginButton);
            loginButton.SetTitle("Login", UIControlState.Normal);
            loginButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            loginButton.Layer.CornerRadius = Constants.CornerRadius;
            loginButton.TouchUpInside += async (sender, e) =>
            {
                Tuple<string, string> response = null; 
                try
                {
                    loginButton.Enabled = false;
                    response = await UserUtil.AuthenticateUserAsync(usernameTextField.Text, passwordTextField.Text);
                }
                catch (Exception ex)
                {
                    var alert = UIAlertController.Create("Something goes wrong", String.Format("Please check your Internet connection and try again.{0} Details: {1}", Environment.NewLine, ex.Message), UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                    loginButton.Enabled = true;
                }

                if (!String.IsNullOrEmpty(response.Item2))
                {
                    UIApplication.SharedApplication.Windows[0].RootViewController = new TabViewController();
                }
                else
                {
                    var alert = UIAlertController.Create("Error", response.Item1, UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Retry", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
                loginButton.Enabled = true;
            };

            var signUpButton = new UIButton(UIButtonType.System)
            {
                BackgroundColor = UIColor.White,
                Font = UIFont.BoldSystemFontOfSize(Constants.NormalFontSize),
            };
            View.AddSubview(signUpButton);
            signUpButton.SetTitle("Sign Up", UIControlState.Normal);
            signUpButton.SetTitleColor(UIColor.FromHSB(Constants.MainColorHue, Constants.MainColorSaturation, Constants.MainColorBrightness), UIControlState.Normal);
            signUpButton.Layer.CornerRadius = Constants.CornerRadius;
            signUpButton.TouchUpInside += (sender, e) =>
            {
                signUpButton.Enabled = false;
                NavigationController.PushViewController(new RegistrationViewController(), true);
                signUpButton.Enabled = true;
            };

            #region UI Layout
            var topPad = View.Frame.GetCenterY() - (Constants.ControlsHeight * 4 / 2) - Constants.ControlsHeight; // Half the total heights of all controls: the controls will be centered then go up a bit

            View.ConstrainLayout(() =>
//                logoImageView.Frame.Top == View.Frame.Top + topPad &&
//                logoImageView.Frame.Left >= View.Frame.Left + UIConstants.HorizontalPad &&
//                logoImageView.Frame.Right >= View.Frame.Right - UIConstants.HorizontalPad &&
//                logoImageView.Frame.GetCenterX() == View.Frame.GetCenterX() &&
//                logoImageView.Frame.Width == 500f &&

                usernameTextField.Frame.Top == View.Frame.Top + topPad &&
                usernameTextField.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                usernameTextField.Frame.Left >= View.Frame.Left + Constants.HorizontalPad &&
                usernameTextField.Frame.Right >= View.Frame.Right - Constants.HorizontalPad &&
                usernameTextField.Frame.Width <= Constants.MaximumControlsWidth &&
                usernameTextField.Frame.Height == Constants.ControlsHeight &&

                passwordTextField.Frame.Top == usernameTextField.Frame.Bottom + Constants.SmallGap &&
                passwordTextField.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                passwordTextField.Frame.Left >= View.Frame.Left + Constants.HorizontalPad &&
                passwordTextField.Frame.Right >= View.Frame.Right - Constants.HorizontalPad &&
                passwordTextField.Frame.Width <= Constants.MaximumControlsWidth &&
                passwordTextField.Frame.Height == Constants.ControlsHeight &&

                loginButton.Frame.Top == passwordTextField.Frame.Bottom + Constants.BigGap &&
                loginButton.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                loginButton.Frame.Left >= View.Frame.Left + Constants.HorizontalPad &&
                loginButton.Frame.Right >= View.Frame.Right - Constants.HorizontalPad &&
                loginButton.Frame.Width <= Constants.MaximumControlsWidth &&
                loginButton.Frame.Height == Constants.ControlsHeight &&

                signUpButton.Frame.Top == loginButton.Frame.Bottom + Constants.SmallGap &&
                signUpButton.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                signUpButton.Frame.Left >= View.Frame.Left + Constants.HorizontalPad &&
                signUpButton.Frame.Right >= View.Frame.Right - Constants.HorizontalPad &&
                signUpButton.Frame.Width <= Constants.MaximumControlsWidth &&
                signUpButton.Frame.Height == Constants.ControlsHeight
            );
            #endregion

            View.AddGestureRecognizer(new UITapGestureRecognizer(() =>
                    {
                        View.EndEditing(true);
                    }));
        }
    }
}