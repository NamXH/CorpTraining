using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;

namespace CorpTraining.iOS
{
    public class LessonScreenViewController : UIViewController
    {
        protected IList<Screen> Screens { get; set; }

        protected int Index { get; set; }

        public LessonScreenViewController(IList<Screen> screens, int index)
        {
            Screens = screens;
            Index = index;
        }

        protected virtual void PushNextScreen()
        {
            if (Index >= Screens.Count - 1) // Index is zero-based
            {
                return;
            }

            var lessonScreen = new LessonScreenViewController(Screens, Index + 1);
            NavigationController.PushViewController(lessonScreen, true);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (Index < Screens.Count - 1)
            {
                NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Next", UIBarButtonItemStyle.Plain, ((sender, e) =>
                        {
                            PushNextScreen();
                        })), true);
            }
            else
            {
                NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Submit", UIBarButtonItemStyle.Plain, ((sender, e) =>
                        {
                        })), true); 
            }

            View.BackgroundColor = UIColor.White;

            var scrollView = new UIScrollView
            {
            };
            View.AddSubview(scrollView);
            View.ConstrainLayout(() =>
                scrollView.Frame.Top == View.Frame.Top  &&
                scrollView.Frame.Left == View.Frame.Left &&
                scrollView.Frame.Right == View.Frame.Right &&
                scrollView.Frame.Bottom == View.Frame.Bottom
            );

            var stackView = new UIStackView
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Alignment = UIStackViewAlignment.Fill,
                Distribution = UIStackViewDistribution.EqualSpacing,
                Spacing = UIConstants.BigGap,
            };
            scrollView.AddSubview(stackView);

            var twiceHorizontalPad = UIConstants.HorizontalPad * 2;
            View.ConstrainLayout(() =>
                stackView.Frame.Top == scrollView.Frame.Top + UIConstants.VerticalPad &&
                stackView.Frame.Bottom == scrollView.Frame.Bottom - UIConstants.VerticalPad &&
                stackView.Frame.Left == scrollView.Frame.Left + UIConstants.HorizontalPad &&
                stackView.Frame.Width == scrollView.Frame.Width - twiceHorizontalPad // required!
            );
            scrollView.ContentSize = stackView.Frame.Size;

            #region For test
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

            var firstNameTextField2 = new UITextField
            {
                Placeholder = "First Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(firstNameTextField2);
            firstNameTextField2.Layer.BorderColor = UIColor.Gray.CGColor;
            firstNameTextField2.Layer.BorderWidth = UIConstants.BorderWidth;
            firstNameTextField2.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                firstNameTextField2.Frame.Height == UIConstants.ControlsHeight
            );

            var firstNameTextField3 = new UITextField
            {
                Placeholder = "First Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(firstNameTextField3);
            firstNameTextField3.Layer.BorderColor = UIColor.Gray.CGColor;
            firstNameTextField3.Layer.BorderWidth = UIConstants.BorderWidth;
            firstNameTextField3.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                firstNameTextField3.Frame.Height == UIConstants.ControlsHeight
            );

            var firstNameTextField4 = new UITextField
            {
                Placeholder = "First Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(firstNameTextField4);
            firstNameTextField4.Layer.BorderColor = UIColor.Gray.CGColor;
            firstNameTextField4.Layer.BorderWidth = UIConstants.BorderWidth;
            firstNameTextField4.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                firstNameTextField4.Frame.Height == UIConstants.ControlsHeight
            );

            var firstNameTextField5 = new UITextField
            {
                Placeholder = "First Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(firstNameTextField5);
            firstNameTextField5.Layer.BorderColor = UIColor.Gray.CGColor;
            firstNameTextField5.Layer.BorderWidth = UIConstants.BorderWidth;
            firstNameTextField5.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                firstNameTextField5.Frame.Height == UIConstants.ControlsHeight
            );

            var firstNameTextField6 = new UITextField
            {
                Placeholder = "First Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(firstNameTextField6);
            firstNameTextField6.Layer.BorderColor = UIColor.Gray.CGColor;
            firstNameTextField6.Layer.BorderWidth = UIConstants.BorderWidth;
            firstNameTextField6.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                firstNameTextField6.Frame.Height == UIConstants.ControlsHeight
            );

            var firstNameTextField7 = new UITextField
            {
                Placeholder = "First Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(firstNameTextField7);
            firstNameTextField7.Layer.BorderColor = UIColor.Gray.CGColor;
            firstNameTextField7.Layer.BorderWidth = UIConstants.BorderWidth;
            firstNameTextField7.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                firstNameTextField7.Frame.Height == UIConstants.ControlsHeight
            );

            var firstNameTextField8 = new UITextField
            {
                Placeholder = "First Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(firstNameTextField8);
            firstNameTextField8.Layer.BorderColor = UIColor.Gray.CGColor;
            firstNameTextField8.Layer.BorderWidth = UIConstants.BorderWidth;
            firstNameTextField8.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                firstNameTextField8.Frame.Height == UIConstants.ControlsHeight
            );

            var firstNameTextField9 = new UITextField
            {
                Placeholder = "First Name",
                BorderStyle = UITextBorderStyle.RoundedRect,
            };
            stackView.AddArrangedSubview(firstNameTextField9);
            firstNameTextField9.Layer.BorderColor = UIColor.Gray.CGColor;
            firstNameTextField9.Layer.BorderWidth = UIConstants.BorderWidth;
            firstNameTextField9.Layer.CornerRadius = UIConstants.CornerRadius;
            View.ConstrainLayout(() =>
                firstNameTextField9.Frame.Height == UIConstants.ControlsHeight
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
            #endregion
        }
    }
}