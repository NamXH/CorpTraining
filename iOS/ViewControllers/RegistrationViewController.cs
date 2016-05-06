using System;
using UIKit;
using System.Text.RegularExpressions;

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
                AutocapitalizationType = UITextAutocapitalizationType.None,
                KeyboardType = UIKeyboardType.EmailAddress,
            };
            stackView.AddArrangedSubview(emailTextField);
            emailTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            emailTextField.Layer.BorderWidth = UIConstants.BorderWidth;
            emailTextField.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                emailTextField.Frame.Height == UIConstants.ControlsHeight
            );

            var phoneTextField = new UITextField
            {
                Placeholder = "Phone Number",
                BorderStyle = UITextBorderStyle.RoundedRect,
                KeyboardType = UIKeyboardType.NumberPad,
            };
            stackView.AddArrangedSubview(phoneTextField);
            phoneTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            phoneTextField.Layer.BorderWidth = UIConstants.BorderWidth;
            phoneTextField.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                phoneTextField.Frame.Height == UIConstants.ControlsHeight
            );

            var passwordTextField = new UITextField
            {
                Placeholder = "Password",
                SecureTextEntry = true,
                AutocapitalizationType = UITextAutocapitalizationType.None,
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
            submitButton.TouchUpInside += async (sender, e) =>
            {
                var valid = ValidateInfoAndDisplayAlert(firstNameTextField.Text, lastNameTextField.Text, emailTextField.Text, phoneTextField.Text, passwordTextField.Text, confirmPasswordTextField.Text);
                if (valid)
                {
                    var user = new User
                    {
                        FirstName = firstNameTextField.Text,
                        LastName = lastNameTextField.Text,
                        Email = emailTextField.Text,
                        Password = passwordTextField.Text,
                        Phone = phoneTextField.Text, // Not a required field
                    };

                    string registrationResult = null;
                    try
                    {
                        registrationResult = await UserUtil.RegisterUserAsync(user);
                        if (registrationResult == "success")
                        {
                            UIApplication.SharedApplication.Windows[0].RootViewController = new TabViewController();
                        }
                        else
                        {
                            var alert = UIAlertController.Create("Something goes wrong", "", UIAlertControllerStyle.Alert);
                            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                            PresentViewController(alert, true, null); 
                        }
                    }
                    catch (Exception ex)
                    {
                        var alert = UIAlertController.Create("Something goes wrong", String.Format("Please try again.{0} Details: {1}", Environment.NewLine, ex.Message), UIAlertControllerStyle.Alert);
                        alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                        PresentViewController(alert, true, null);
                    }
                }
            };
        }

        private bool ValidateInfoAndDisplayAlert(string firstName, string lastName, string email, string phone, string password, string confirmPassword)
        {
            var result = true;

            var alertMessage = "Please provide your:";

            if (String.IsNullOrWhiteSpace(firstName))
            {
                result = false;
                alertMessage += " first name";
            }

            if (String.IsNullOrWhiteSpace(lastName))
            {
                if (result)
                {
                    result = false;
                }
                else
                {
                    alertMessage += ","; 
                }
                alertMessage += " last name";
            }

            if (String.IsNullOrWhiteSpace(email))
            {
                if (result)
                {
                    result = false;
                }
                else
                {
                    alertMessage += ","; 
                }
                alertMessage += " email";
            }

            if (String.IsNullOrWhiteSpace(password))
            {
                if (result)
                {
                    result = false;
                }
                else
                {
                    alertMessage += ","; 
                }
                alertMessage += " password";
            }

            if (String.IsNullOrWhiteSpace(confirmPassword))
            {
                if (result)
                {
                    result = false;
                }
                else
                {
                    alertMessage += ","; 
                }
                alertMessage += " confirm password";
            }

            if (!result)
            {
                var alert = UIAlertController.Create("Error", alertMessage, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Retry", UIAlertActionStyle.Default, null));
                PresentViewController(alert, true, null);
            }
            else if (!Regex.IsMatch(phone, @"^\d+$"))
            {
                result = false;
                var alert = UIAlertController.Create("Error", "Phone number must be numeric.", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Retry", UIAlertActionStyle.Default, null));
                PresentViewController(alert, true, null);
            }
            else if (password != confirmPassword)
            {
                result = false;
                var alert = UIAlertController.Create("Error", "Password confirmation doesn't match.", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Retry", UIAlertActionStyle.Default, null));
                PresentViewController(alert, true, null);
            }

            return result;
        }
    }
}

