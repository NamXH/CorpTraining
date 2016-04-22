using System;
using UIKit;

namespace CorpTraining.iOS
{
    public class AccountViewController : UIViewController
    {
        public AccountViewController()
            : base()
        {
            Title = "Account";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var profileImage = new UIImageView();
            View.AddSubview(profileImage);
            profileImage.Image = UIImage.FromBundle("justin.png");

            var editButton = new UIButton(UIButtonType.System)
            {
                BackgroundColor = UIColor.FromHSB(UIConstants.MainColorHue, UIConstants.MainColorSaturation, UIConstants.MainColorBrightness),
                Font = UIFont.BoldSystemFontOfSize(UIConstants.NormalFontSize),
            };
            View.AddSubview(editButton);
            editButton.SetTitle("Edit Profile", UIControlState.Normal);
            editButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            editButton.Layer.CornerRadius = UIConstants.CornerRadius;

            var nameLabel = new UILabel();
            View.AddSubview(nameLabel);
            nameLabel.Text = "Justin Trudeau";
            nameLabel.Font = UIFont.FromName("Helvetica-Bold", 40f);


            View.BackgroundColor = UIColor.White;

            #region Layout
            View.ConstrainLayout(() =>
                profileImage.Frame.Top == View.Frame.Top + 100f &&
                profileImage.Frame.Left == View.Frame.Left + 80f &&
                profileImage.Frame.Height == 300f &&
                profileImage.Frame.Width == 300f &&

                editButton.Frame.Top == profileImage.Frame.Bottom + 50f &&
                editButton.Frame.Left == View.Frame.Left + 80f &&
                editButton.Frame.Height == UIConstants.ControlsHeight &&
                editButton.Frame.Width == 300f &&

                nameLabel.Frame.Top == profileImage.Frame.Top + 20f &&
                nameLabel.Frame.Left == profileImage.Frame.Right + 50f &&
                nameLabel.Frame.Height == UIConstants.ControlsHeight 
//                nameLabel.Frame.Width == 300f &&
            );
            #endregion
        }
    }
}

