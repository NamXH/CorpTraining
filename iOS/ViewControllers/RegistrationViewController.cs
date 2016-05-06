using System;
using UIKit;

namespace CorpTraining.iOS
{
    public class RegistrationViewController : UIViewController
    {
        public RegistrationViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIConstants.MainColor;

            var stackView = new UIStackView
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Alignment = UIStackViewAlignment.Fill,
                Distribution = UIStackViewDistribution.EqualSpacing,
                Spacing = UIConstants.SmallGap,
                TranslatesAutoresizingMaskIntoConstraints = false,
            };
            View.AddSubview(stackView);
            View.ConstrainLayout(() =>
                stackView.Frame.GetCenterY() == View.Frame.GetCenterY() &&
                stackView.Frame.Left == View.Frame.Left + UIConstants.HorizontalPad &&
                stackView.Frame.Right == View.Frame.Right - UIConstants.VerticalPad
            );

            var firstNameTextField = new UITextField
            {
                Placeholder = "First Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(firstNameTextField);
            firstNameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            firstNameTextField.Layer.BorderWidth = UIConstants.BorderWidth;
            firstNameTextField.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                firstNameTextField.Frame.Height == UIConstants.ControlsHeight
            );

            var lastNameTextField = new UITextField
            {
                Placeholder = "Last Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(lastNameTextField);
            lastNameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            lastNameTextField.Layer.BorderWidth = UIConstants.BorderWidth;
            lastNameTextField.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                lastNameTextField.Frame.Height == UIConstants.ControlsHeight
            );

            var emailTextField = new UITextField
            {
                Placeholder = "Email",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(emailTextField);
            emailTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            emailTextField.Layer.BorderWidth = UIConstants.BorderWidth;
            emailTextField.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                emailTextField.Frame.Height == UIConstants.ControlsHeight
            );

            var passwordTextField = new UITextField
            {
                Placeholder = "Password",
                SecureTextEntry = true,
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(passwordTextField);
            passwordTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            passwordTextField.Layer.BorderWidth = UIConstants.BorderWidth;
            passwordTextField.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                passwordTextField.Frame.Height == UIConstants.ControlsHeight
            );

            var confirmPasswordTextField = new UITextField
            {
                Placeholder = "Confirm Password",
                SecureTextEntry = true,
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(confirmPasswordTextField);
            confirmPasswordTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            confirmPasswordTextField.Layer.BorderWidth = UIConstants.BorderWidth;
            confirmPasswordTextField.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                confirmPasswordTextField.Frame.Height == UIConstants.ControlsHeight
            );

            var submitButton = new UIButton(UIButtonType.System)
            {
                BackgroundColor = UIColor.FromHSB(UIConstants.MainColorHue + 0.08f, UIConstants.MainColorSaturation + 0.1f, UIConstants.MainColorBrightness),
                Font = UIFont.BoldSystemFontOfSize(UIConstants.NormalFontSize),
            };
            stackView.AddArrangedSubview(submitButton);
            submitButton.SetTitle("Submit", UIControlState.Normal);
            submitButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            submitButton.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                submitButton.Frame.Height == UIConstants.ControlsHeight
            );
        }
    }
}

