using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Linq;

namespace CorpTraining.iOS
{
    public class LessonsViewController : UIViewController
    {
        public IList<Lesson> Lessons { get; set; }

        public LessonsViewController()
            : base()
        {
            Title = "Lessons";
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            var lessonsTable = new UITableView();
            View.AddSubview(lessonsTable);

            var barHeight = UIConstants.StatusBarHeight;

            #region Layout
            View.ConstrainLayout(() =>
                lessonsTable.Frame.Top == View.Frame.Top &&
                lessonsTable.Frame.Left == View.Frame.Left &&
                lessonsTable.Frame.Right == View.Frame.Right &&
                lessonsTable.Frame.Bottom == View.Frame.Bottom
            );
            #endregion

            // Create loading indicator here!!

            Lessons = await LessonUtil.GetLessonsAsync();

            lessonsTable.Source = new LessonTableSource(this, Lessons);
            lessonsTable.ReloadData();
        }
    }

    public class LessonTableSource : UITableViewSource
    {
        public UIViewController Container { get; private set; }

        public IList<Lesson> Items { get; set; }

        private string cellIdentifier = "TableCell";

        public LessonTableSource(UIViewController container, IList<Lesson> items)
        {
            Container = container;
            Items = items;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Items.Count;
        }

        public async override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            // Create loading indicator here!!

            var screens = await LessonUtil.GetScreensByLessonAsync(Items[indexPath.Row].Id);
            // Filter for TEST !!
            screens = screens.Where(x => (x.Type == "audio_text") || (x.Type == "audio_question")).Take(8).ToList();

            var lessonScreen = LessonScreenViewControllerGenerator.Generate(screens, 0);
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

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellIdentifier);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
            }

            // If lesson is finished then add check mark
            cell.Accessory = UITableViewCellAccessory.Checkmark; // Place holder!!
            cell.TextLabel.Text = Items[indexPath.Row].Title;

            return cell;
        }
    }
}

