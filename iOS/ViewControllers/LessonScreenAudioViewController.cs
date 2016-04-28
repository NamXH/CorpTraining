using System;
using UIKit;
using System.Collections.Generic;
using AVFoundation;
using Foundation;
using AVKit;
using CoreGraphics;

namespace CorpTraining.iOS
{
    public class LessonScreenAudioViewController : LessonScreenBaseViewController
    {
        public LessonScreenAudioViewController(IList<Screen> screens, int index)
            : base(screens, index)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            var player = new AVPlayer(NSUrl.FromString(Screens[Index].AudioUrl));
            var playerViewController = new AVPlayerViewController
            {
                Player = player,      
            };
            AddChildViewController(playerViewController);
            View.AddSubview(playerViewController.View);

            #region Layout
            var barHeight = UIConstants.StatusBarHeight + (float)NavigationController.NavigationBar.Frame.Size.Height;

            View.ConstrainLayout(() =>
                playerViewController.View.Frame.Top == View.Frame.Top + barHeight &&
                playerViewController.View.Frame.Left == View.Frame.Left &&
                playerViewController.View.Frame.Right == View.Frame.Right &&
                playerViewController.View.Frame.Height == 100f
            );
            #endregion

            var text = Screens[Index].Text ?? Screens[Index].Question;

            var textLabel = new UILabel
            { 
                Text = text,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
            };
            View.AddSubview(textLabel);

            View.ConstrainLayout(() =>
                textLabel.Frame.Top == playerViewController.View.Frame.Bottom + UIConstants.BigGap &&
                textLabel.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                textLabel.Frame.Left >= View.Frame.Left + UIConstants.SmallHorizontalPad &&
                textLabel.Frame.Right >= View.Frame.Right - UIConstants.SmallHorizontalPad
            );

            // Calculate textLabel's height
            var textLabelWidth = View.Frame.Width - UIConstants.SmallHorizontalPad * 2;
            // Correct but complicated
            //                var labelSize = neww NSString(text).GetBoundingRect(
            //                                    new CGSize(textLabelWidth, float.MaxValue), 
            //                                    NSStringDrawingOptions.UsesLineFragmentOrigin, 
            //                                    new UIStringAttributes(){ Font = textLabel.Font }, 
            //                                    null);
            var labelSize = textLabel.SizeThatFits(new CGSize(textLabelWidth, float.MaxValue));
            var textLabelHeight = labelSize.Height;

            if (Screens[Index].Images.Count > 0)
            {
                // Display first image for demo
                var image = new UIImageView(); 
                View.AddSubview(image);
                using (var url = new NSUrl(Screens[Index].Images[0].Url))
                {
                    using (var data = NSData.FromUrl(url))
                    {
                        image.Image = UIImage.LoadFromData(data);
                    }
                }

                var imageTopPad = barHeight + playerViewController.View.Frame.Height + UIConstants.BigGap + textLabelHeight + UIConstants.BigGap;
                View.ConstrainLayout(() =>
                    image.Frame.Top == View.Frame.Top + imageTopPad &&
                    image.Frame.Left == View.Frame.Left &&
                    image.Frame.Right == View.Frame.Right 
//                    image.Frame.Height == 400f
                );
            }

            // Assumming there are 1 audio play and 1 piece of text above
            if (Screens[Index].Options != null)
            {
                var optionsUIs = new List<Tuple<UIButton, UIButton>>();

                var optionsTopPad = barHeight + playerViewController.View.Frame.Height + UIConstants.BigGap + textLabelHeight + UIConstants.BigGap;
                   
                var i = 1;
                foreach (var option in Screens[Index].Options)
                {
                    var optionRadioButton = new UIButton(UIButtonType.RoundedRect);
                    View.AddSubview(optionRadioButton);
                    optionRadioButton.SetImage(UIImage.FromBundle("radio_enable.png"), UIControlState.Normal);
                    optionRadioButton.SetImage(UIImage.FromBundle("radio_disable.png"), UIControlState.Disabled);
                    if (i != 1)
                    {
                        optionRadioButton.Enabled = false;
                    }

                    var optionTextButton = new UIButton(UIButtonType.System);
                    View.AddSubview(optionTextButton);
                    optionTextButton.SetTitle(option.Title, UIControlState.Normal);

                    optionsUIs.Add(new Tuple<UIButton, UIButton>(optionRadioButton, optionTextButton));

                    var topPad = optionsTopPad + (UIConstants.ControlsHeight + UIConstants.SmallGap) * (i - 1);
                    View.ConstrainLayout(() =>
                        optionRadioButton.Frame.Top == View.Frame.Top + topPad &&
                        optionRadioButton.Frame.Left == View.Frame.Left + UIConstants.HorizontalPad &&
                        optionRadioButton.Frame.Height == 20f &&
                        optionRadioButton.Frame.Width == 20f &&

                        optionTextButton.Frame.GetCenterY() == optionRadioButton.Frame.GetCenterY() &&
                        optionTextButton.Frame.Left == optionRadioButton.Frame.Left + 30f &&
                        optionTextButton.Frame.Height == UIConstants.ControlsHeight
                    );

                    i++;
                }

                i = 0;
                foreach (var tuple in optionsUIs)
                {
                    tuple.Item2.TouchUpInside += (sender, e) =>
                    {
                        tuple.Item1.Enabled = true;

                        foreach (var otherTuple in optionsUIs)
                        {
                            if (!Object.ReferenceEquals(sender, otherTuple.Item2))
                            {
                                otherTuple.Item1.Enabled = false;
                            }
                        }
                    };
                    i++;
                }
            }
        }
    }
}

