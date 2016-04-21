using System;
using UIKit;
using System.Collections.Generic;
using AVFoundation;
using Foundation;
using AVKit;

namespace CorpTraining.iOS
{
    public class LessonScreenAudioViewController : LessonScreenBaseViewController
    {
        public IList<Tuple<UIButton, UIButton>> QuestionsUIs { get; set; }

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
            var topPad = (float)NavigationController.NavigationBar.Frame.Size.Height + 20f;

            View.ConstrainLayout(() =>
                playerViewController.View.Frame.Top == View.Frame.Top + topPad &&
                playerViewController.View.Frame.Left == View.Frame.Left &&
                playerViewController.View.Frame.Right == View.Frame.Right &&
                playerViewController.View.Frame.Height == 200f
            );
            #endregion

            QuestionsUIs = new List<Tuple<UIButton, UIButton>>();

            var i = 1;
            foreach (var option in Screens[Index].Options)
            {
                var answerRadioButton = new UIButton(UIButtonType.RoundedRect);
                View.AddSubview(answerRadioButton);
                answerRadioButton.SetImage(UIImage.FromBundle("radio_enable.png"), UIControlState.Normal);
                answerRadioButton.SetImage(UIImage.FromBundle("radio_disable.png"), UIControlState.Disabled);
                if (i != 1)
                {
                    answerRadioButton.Enabled = false;
                }

                var answerTextButton = new UIButton(UIButtonType.System);
                View.AddSubview(answerTextButton);
                answerTextButton.SetTitle(option.Title, UIControlState.Normal);

                QuestionsUIs.Add(new Tuple<UIButton, UIButton>(answerRadioButton, answerTextButton));

                var optionTopPad = 300f + 50f * i;
                var leftPad = 400f;
                View.ConstrainLayout(() =>
                    answerRadioButton.Frame.Top == View.Frame.Top + optionTopPad &&
                    answerRadioButton.Frame.Left == View.Frame.Left + leftPad &&
                    answerRadioButton.Frame.Height == 20f &&
                    answerRadioButton.Frame.Width == 20f &&

                    answerTextButton.Frame.GetCenterY() == answerRadioButton.Frame.GetCenterY() &&
                    answerTextButton.Frame.Left == answerRadioButton.Frame.Left + 30f &&
                    answerTextButton.Frame.Height == UIConstants.ControlsHeight
                );

                i++;
            }

            i = 0;
            foreach (var tuple in QuestionsUIs)
            {
                tuple.Item2.TouchUpInside += (sender, e) =>
                {
                    tuple.Item1.Enabled = true;

                    foreach (var otherTuple in QuestionsUIs)
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

