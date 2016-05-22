using System;
using UIKit;
using System.Text.RegularExpressions;
using System.Net.Mail;

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
            View.BackgroundColor = Constants.MainColor;

            var stackView = new UIStackView
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Alignment = UIStackViewAlignment.Fill,
                Distribution = UIStackViewDistribution.EqualSpacing,
                Spacing = Constants.SmallGap,
                TranslatesAutoresizingMaskIntoConstraints = false,
            };
            View.AddSubview(stackView);
            var barHeight = Constants.StatusBarHeight + (float)NavigationController.NavigationBar.Frame.Size.Height;
            var topPad = barHeight + Constants.VerticalPad * 2;
            View.ConstrainLayout(() =>
                stackView.Frame.Top == View.Frame.Top + topPad &&
                stackView.Frame.Left == View.Frame.Left + Constants.HorizontalPad &&
                stackView.Frame.Right == View.Frame.Right - Constants.VerticalPad
            );

            var firstNameTextField = new UITextField
            {
                Placeholder = "First Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(firstNameTextField);
            firstNameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            firstNameTextField.Layer.BorderWidth = Constants.BorderWidth;
            firstNameTextField.Layer.CornerRadius = Constants.CornerRadius;
            View.ConstrainLayout(() =>
                firstNameTextField.Frame.Height == Constants.ControlsHeight
            );

            var lastNameTextField = new UITextField
            {
                Placeholder = "Last Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(lastNameTextField);
            lastNameTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            lastNameTextField.Layer.BorderWidth = Constants.BorderWidth;
            lastNameTextField.Layer.CornerRadius = Constants.CornerRadius;
            View.ConstrainLayout(() =>
                lastNameTextField.Frame.Height == Constants.ControlsHeight
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
            emailTextField.Layer.BorderWidth = Constants.BorderWidth;
            emailTextField.Layer.CornerRadius = Constants.CornerRadius;
            View.ConstrainLayout(() =>
                emailTextField.Frame.Height == Constants.ControlsHeight
            );

            var phoneTextField = new UITextField
            {
                Placeholder = "Phone Number",
                BorderStyle = UITextBorderStyle.RoundedRect,
                KeyboardType = UIKeyboardType.NumberPad,
            };
            stackView.AddArrangedSubview(phoneTextField);
            phoneTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            phoneTextField.Layer.BorderWidth = Constants.BorderWidth;
            phoneTextField.Layer.CornerRadius = Constants.CornerRadius;
            View.ConstrainLayout(() =>
                phoneTextField.Frame.Height == Constants.ControlsHeight
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
            passwordTextField.Layer.BorderWidth = Constants.BorderWidth;
            passwordTextField.Layer.CornerRadius = Constants.CornerRadius;
            View.ConstrainLayout(() =>
                passwordTextField.Frame.Height == Constants.ControlsHeight
            );

            var confirmPasswordTextField = new UITextField
            {
                Placeholder = "Confirm Password",
                SecureTextEntry = true,
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(confirmPasswordTextField);
            confirmPasswordTextField.Layer.BorderColor = UIColor.Gray.CGColor;
            confirmPasswordTextField.Layer.BorderWidth = Constants.BorderWidth;
            confirmPasswordTextField.Layer.CornerRadius = Constants.CornerRadius;
            View.ConstrainLayout(() =>
                confirmPasswordTextField.Frame.Height == Constants.ControlsHeight
            );

            var submitButton = new UIButton(UIButtonType.System)
            {
                BackgroundColor = UIColor.FromHSB(Constants.MainColorHue + 0.08f, Constants.MainColorSaturation + 0.1f, Constants.MainColorBrightness),
                Font = UIFont.BoldSystemFontOfSize(Constants.NormalFontSize),
            };
            stackView.AddArrangedSubview(submitButton);
            submitButton.SetTitle("Submit", UIControlState.Normal);
            submitButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            submitButton.Layer.CornerRadius = Constants.CornerRadius;
            View.ConstrainLayout(() =>
                submitButton.Frame.Height == Constants.ControlsHeight
            );
            submitButton.TouchUpInside += async (sender, e) =>
            {
                submitButton.Enabled = false;
                var valid = ValidateInfo(firstNameTextField.Text, lastNameTextField.Text, emailTextField.Text, phoneTextField.Text, passwordTextField.Text, confirmPasswordTextField.Text);
                if (valid.Item1)
                {
                    var user = new User
                    {
                        FirstName = firstNameTextField.Text,
                        LastName = lastNameTextField.Text,
                        Email = emailTextField.Text,
                        Password = passwordTextField.Text,
                        Phone = phoneTextField.Text, // Not a required field
                    };

                    Tuple<bool, string> registrationResult = null;
                    try
                    {
//                        registrationResult = await UserUtil.RegisterUserThenLoginAsync(user); Can't use right now!!
                        registrationResult = await UserUtil.RegisterUserAsync(user);
                        if (registrationResult.Item1)
                        {
//                            UIApplication.SharedApplication.Windows[0].RootViewController = new TabViewController(); Can't use right now!!
                            var alert = UIAlertController.Create("Success", "Registration successful", UIAlertControllerStyle.Alert);
                            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                            PresentViewController(alert, true, null); 
                        }
                        else
                        {
                            var alert = UIAlertController.Create("Something goes wrong", registrationResult.Item2, UIAlertControllerStyle.Alert);
                            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                            PresentViewController(alert, true, null); 
                        }
                    }
                    catch (Exception ex)
                    {
                        var alert = UIAlertController.Create("Something goes wrong", String.Format("Please try again.{0} Details: {1}", Environment.NewLine, ex.Message), UIAlertControllerStyle.Alert);
                        alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                        PresentViewController(alert, true, null);
                        submitButton.Enabled = true;
                    }
                }
                else
                {
                    var alert = UIAlertController.Create("Error", valid.Item2, UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Retry", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null); 
                }
                submitButton.Enabled = true;
            };

            View.AddGestureRecognizer(new UITapGestureRecognizer(() =>
                    {
                        View.EndEditing(true);
                    }));
        }

        private Tuple<bool, string> ValidateInfo(string firstName, string lastName, string email, string phone, string password, string confirmPassword)
        {
            var requiredInfoIsCompleted = true;

            var alertMessage = "Please provide your:";

            if (String.IsNullOrWhiteSpace(firstName))
            {
                requiredInfoIsCompleted = false;
                alertMessage += " first name";
            }

            if (String.IsNullOrWhiteSpace(lastName))
            {
                if (requiredInfoIsCompleted)
                {
                    requiredInfoIsCompleted = false;
                }
                else
                {
                    alertMessage += ","; 
                }
                alertMessage += " last name";
            }

            if (String.IsNullOrWhiteSpace(email))
            {
                if (requiredInfoIsCompleted)
                {
                    requiredInfoIsCompleted = false;
                }
                else
                {
                    alertMessage += ","; 
                }
                alertMessage += " email";
            }

            if (String.IsNullOrWhiteSpace(password))
            {
                if (requiredInfoIsCompleted)
                {
                    requiredInfoIsCompleted = false;
                }
                else
                {
                    alertMessage += ","; 
                }
                alertMessage += " password";
            }

            if (String.IsNullOrWhiteSpace(confirmPassword))
            {
                if (requiredInfoIsCompleted)
                {
                    requiredInfoIsCompleted = false;
                }
                else
                {
                    alertMessage += ","; 
                }
                alertMessage += " confirm password";
            }

            if (!requiredInfoIsCompleted)
            {
                return new Tuple<bool, string>(false, alertMessage);
            }
            else if (!Regex.IsMatch(phone, @"^\d+$"))
            {
                return new Tuple<bool, string>(false, "Phone number must be numeric."); 
            }
            else if (password != confirmPassword)
            {
                return new Tuple<bool, string>(false, "Password confirmation doesn't match.");
            }
            else if (!IsValidEmail(email))
            {
                return new Tuple<bool, string>(false, "Email is invalid."); 
            }
            else
            {
                return new Tuple<bool, string>(true, null);
            }
        }

        private bool IsValidEmail(string emailaddress)
        {
            // http://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address/16403290#16403290
            // http://stackoverflow.com/questions/5342375/c-sharp-regex-email-validation
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return m.Address == emailaddress;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}

