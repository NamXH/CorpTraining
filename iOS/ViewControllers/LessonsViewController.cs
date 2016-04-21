using System;
using UIKit;
using System.Collections.Generic;
using Foundation;

namespace CorpTraining.iOS
{
    public class LessonsViewController : UIViewController
    {
        public UITableView LessonsTable { get; set; }

        public LessonsViewController()
            : base()
        {
            TabBarItem.Image = UIImage.FromBundle("book.png");
            TabBarItem.Title = "Lessons";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            LessonsTable = new UITableView();
            View.AddSubview(LessonsTable);

            #region Hardcoded
            var lesson1 = new Lesson();
            lesson1.Title = "Clay Debray & the Muskrat";
            var lesson2 = new Lesson();
            lesson2.Title = "Jason Horse, Cree Soccer";

            var items = new List<Tuple<Lesson, bool>>
            {
                new Tuple<Lesson, bool>(lesson1, true),
                new Tuple<Lesson, bool>(lesson2, false),
            };
            #endregion

            LessonsTable.Source = new LessonTableSource(this, items);

            #region Layout
            View.ConstrainLayout(() =>
                LessonsTable.Frame.Top == View.Frame.Top &&
                LessonsTable.Frame.Left == View.Frame.Left &&
                LessonsTable.Frame.Right == View.Frame.Right &&
                LessonsTable.Frame.Bottom == View.Frame.Bottom
            );
            #endregion
        }
    }

    public class LessonTableSource : UITableViewSource
    {
        public UIViewController Container { get; private set; }

        public IList<Tuple<Lesson, bool>> Items { get; set; }

        private string cellIdentifier = "TableCell";

        public LessonTableSource(UIViewController container, IList<Tuple<Lesson, bool>> items)
        {
            Container = container;
            Items = items;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Items.Count;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            if (!Items[indexPath.Row].Item2)
            {
                #region Hardcoded
                var screens = new List<Screen>
                {
                        new Screen
                        {
                            Type = "audio_recorder", 
                        },
//                    new Screen
//                    {
//                        type = "video",
//                        video_url = "https://www.lessonbasket.com/desktopmodules/lessonbasket/projects/24/54/604Billy%20Int%202.mp4",
//                    },
//                    new Screen
//                    {
//                        type = "audio_question",
//                        audio_url = "https://www.lessonbasket.com/desktopmodules/lessonbasket/projects/24/55/70588221.7831J%20Horse%20Q1%20.mp3",
//                        questions = new List<string>
//                        {
//                            "Thunder Horton's",
//                            "Sweetgrass",
//                            "Thunderchild",
//                            "Poundmaker",
//                            "Starbucks",
//                        },
//                    },
                };
                #endregion

                var lessonScreen = LessonScreenSelector.Select(screens, 0);
                if (lessonScreen != null)
                {
                    Container.NavigationController.PushViewController(lessonScreen, true);
                }
                else
                {
                    // Display alert if needed !!
                }

                tableView.DeselectRow(indexPath, true);
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellIdentifier);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }

            // If lesson is finished then add check mark
            if (Items[indexPath.Row].Item2)
            {
                cell.Accessory = UITableViewCellAccessory.Checkmark;
            }

            cell.TextLabel.Text = Items[indexPath.Row].Item1.Title;

            return cell;
        }
    }
}

