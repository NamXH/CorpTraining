using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using AVFoundation;
using Foundation;
using AVKit;

namespace CorpTraining.iOS
{
    public class LessonScreenViewController : UIViewController
    {
        public int LessonId { get; set; }

        public IList<Screen> Screens { get; set; }

        public int Index { get; set; }

        public List<ScreenAnswer> Answers { get; set; }

        public string Answer { get; set; }

        public LessonScreenViewController(int lessonId, IList<Screen> screens, int index, List<ScreenAnswer> answers)
        {
            LessonId = lessonId;
            Screens = screens;
            Index = index;
            Answers = answers;
            Answer = null;
        }

        protected virtual void PushNextScreen()
        {
            if (Index >= Screens.Count - 1) // Index is zero-based
            {
                return;
            }

            var lessonScreen = new LessonScreenViewController(LessonId, Screens, Index + 1, Answers);
            NavigationController.PushViewController(lessonScreen, true);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (Index < Screens.Count - 1)
            {
                NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Next", UIBarButtonItemStyle.Plain, ((sender, e) =>
                        {
                            if (Answer != null)
                            {
                                Answers.Add(new ScreenAnswer
                                    {
                                        ScreenId = Screens[Index].Id,
                                        Option = Answer,
                                    });
                            }
                            PushNextScreen();
                        })), true);
            }
            else
            {
                NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Submit", UIBarButtonItemStyle.Plain, (async (sender, e) =>
                        {
                            if (Answer != null)
                            {
                                Answers.Add(new ScreenAnswer
                                    {
                                        ScreenId = Screens[Index].Id,
                                        Option = Answer,
                                    });
                            }

                            var loadingOverlay = new LoadingOverlay(View.Bounds);
                            string response = null;
                            try
                            {
                                View.Add(loadingOverlay);
                                response = await LessonUtil.SendLessonAnswers(LessonId, Answers);
                            }
                            catch (Exception ex)
                            {
                                var alert = UIAlertController.Create("Something goes wrong", String.Format("Please check your Internet connection and try again.{0} Details: {1}", Environment.NewLine, ex.Message), UIAlertControllerStyle.Alert);
                                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                                PresentViewController(alert, true, null);
                            }
                            finally
                            {
                                loadingOverlay.HideThenRemove();
                            }

                            string alertTitle = null;
                            string alertMessage = null;
                            if (response == "success")
                            {
                                alertTitle = "Congrats!";
                                alertMessage = "Your answer has been submitted successfully.";
                            }
                            else
                            {
//                                alertTitle = "Something goes wrong";
//                                alertMessage = response; 

                                // Fake for demo!!
                                alertTitle = "Congrats!";
                                alertMessage = "Your answer has been submitted successfully.";
                            }
                            var submissionAlert = UIAlertController.Create(alertTitle, alertMessage, UIAlertControllerStyle.Alert);
                            submissionAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (UIAlertAction obj) =>
                                    {
                                        NavigationController.PopToRootViewController(true);
                                    }));
                            PresentViewController(submissionAlert, true, null);
                        })), true); 
            }

            View.BackgroundColor = UIColor.White;

            var scrollView = new UIScrollView();
            View.AddSubview(scrollView);
            View.ConstrainLayout(() =>
                scrollView.Frame.Top == View.Frame.Top &&
                scrollView.Frame.Left == View.Frame.Left &&
                scrollView.Frame.Right == View.Frame.Right &&
                scrollView.Frame.Bottom == View.Frame.Bottom
            );

            var stackView = new UIStackView
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Alignment = UIStackViewAlignment.Leading,
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

            var mediaPlayerUrl = !String.IsNullOrWhiteSpace(Screens[Index].VideoUrl) ? Screens[Index].VideoUrl : Screens[Index].AudioUrl;
            if (!String.IsNullOrWhiteSpace(mediaPlayerUrl))
            {
                var player = new AVPlayer(NSUrl.FromString(mediaPlayerUrl));
                var playerViewController = new AVPlayerViewController
                {
                    Player = player,      
                };
                AddChildViewController(playerViewController);
                stackView.AddArrangedSubview(playerViewController.View);
                View.ConstrainLayout(() =>
                    playerViewController.View.Frame.Width == stackView.Frame.Width
                );
            }

            if (Screens[Index].Images != null)
            {
                foreach (var image in Screens[Index].Images)
                {
                    var imageView = new UIImageView(); 
                    using (var url = new NSUrl(image.Url))
                    {
                        using (var data = NSData.FromUrl(url))
                        {
                            imageView.Image = UIImage.LoadFromData(data);
                        }
                    } 
                    stackView.AddArrangedSubview(imageView);
                    View.ConstrainLayout(() =>
                        imageView.Frame.Width == stackView.Frame.Width
                    );
                }
            }

            if (Screens[Index].Texts != null)
            {
                foreach (var text in Screens[Index].Texts)
                {
                    var textLabel = new UILabel
                    { 
                        Text = text.TextValue,
                        Lines = 0,
                        LineBreakMode = UILineBreakMode.WordWrap,
                    };
                    stackView.AddArrangedSubview(textLabel);
                    View.ConstrainLayout(() =>
                        textLabel.Frame.Width == stackView.Frame.Width
                    );
                }
            }

            if (Screens[Index].Options != null)
            {
                // Question
                var questionLabel = new UILabel
                { 
                    Text = Screens[Index].Question,
                    Lines = 0,
                    LineBreakMode = UILineBreakMode.WordWrap,
                };
                stackView.AddArrangedSubview(questionLabel);
                View.ConstrainLayout(() =>
                    questionLabel.Frame.Width == stackView.Frame.Width
                );

                // Options
                var optionsUIs = new List<Tuple<UIButton, UIButton>>();

                var i = 1;
                foreach (var option in Screens[Index].Options)
                {
                    var optionStackView = new UIStackView
                    {
                        Axis = UILayoutConstraintAxis.Horizontal,
                        Alignment = UIStackViewAlignment.Center,
                        Distribution = UIStackViewDistribution.EqualSpacing,
                        Spacing = UIConstants.SmallGap,
                    };
                    stackView.AddArrangedSubview(optionStackView);

                    var optionRadioButton = new UIButton(UIButtonType.RoundedRect);
                    optionStackView.AddArrangedSubview(optionRadioButton);
                    optionRadioButton.SetImage(UIImage.FromBundle("radio_enable.png"), UIControlState.Normal);
                    optionRadioButton.SetImage(UIImage.FromBundle("radio_disable.png"), UIControlState.Disabled);
                    View.ConstrainLayout(() =>
                        optionRadioButton.Frame.Height == 20f &&
                        optionRadioButton.Frame.Width == 20f 
                    );
                    if (i == 1)
                    {
                        Answer = option.Title;
                    }
                    else
                    {
                        optionRadioButton.Enabled = false;
                    }

                    var optionTextButton = new UIButton(UIButtonType.System)
                    {
                        LineBreakMode = UILineBreakMode.WordWrap,
                        HorizontalAlignment = UIControlContentHorizontalAlignment.Left,
                        VerticalAlignment = UIControlContentVerticalAlignment.Center,
                    };
                    optionTextButton.SetTitle(option.Title, UIControlState.Normal);
                    optionStackView.AddArrangedSubview(optionTextButton);

                    var maxWidth = View.Frame.Width - UIConstants.HorizontalPad * 2 - optionRadioButton.Frame.Width;
                    var textSize = UIHelper.GetTextSize(option.Title, optionTextButton.Font, maxWidth, float.MaxValue);
                    var textHeight = textSize.Height;
                    View.ConstrainLayout(() =>
                        optionTextButton.Frame.Height == textHeight
                    );

                    optionsUIs.Add(new Tuple<UIButton, UIButton>(optionRadioButton, optionTextButton));
                    i++;
                }

                foreach (var tuple in optionsUIs)
                {
                    tuple.Item2.TouchUpInside += (sender, e) =>
                    {
                        Answer = tuple.Item2.Title(UIControlState.Normal);
                        tuple.Item1.Enabled = true;

                        foreach (var otherTuple in optionsUIs)
                        {
                            if (!Object.ReferenceEquals(sender, otherTuple.Item2))
                            {
                                otherTuple.Item1.Enabled = false;
                            }
                        }
                    };
                }
            }
        }
    }
}