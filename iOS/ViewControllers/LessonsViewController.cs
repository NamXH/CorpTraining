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

            View.ConstrainLayout(() =>
                lessonsTable.Frame.Top == View.Frame.Top &&
                lessonsTable.Frame.Left == View.Frame.Left &&
                lessonsTable.Frame.Right == View.Frame.Right &&
                lessonsTable.Frame.Bottom == View.Frame.Bottom
            );

            // Loading indicator
            var loadingOverlay = new LoadingOverlay(View.Bounds);
            View.AddSubview(loadingOverlay);

            try
            {
                Lessons = await LessonUtil.GetLessonsAsync();
            }
            catch (Exception e)
            {
                var alert = UIAlertController.Create("Something goes wrong", String.Format("Please check your Internet connection and try again.{0} Details: {1}", Environment.NewLine, e.Message), UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(alert, true, null); 
            }

            if (Lessons != null)
            {
                lessonsTable.Source = new LessonTableSource(this, Lessons);
                lessonsTable.ReloadData();
            }

            loadingOverlay.HideThenRemove();
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
            // Loading indicator
            var loadingOverlay = new LoadingOverlay(Container.View.Bounds);
            Container.View.Add(loadingOverlay);

            List<Screen> screens = null;
            try
            {
                screens = (await LessonUtil.GetScreensByLessonAsync(Items[indexPath.Row].Id)).ToList();
            }
            catch (Exception e)
            {
                var alert = UIAlertController.Create("Something goes wrong", String.Format("Please check your Internet connection and try again.{0} Details: {1}", Environment.NewLine, e.Message), UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                Container.PresentViewController(alert, true, null);
            }
            loadingOverlay.HideThenRemove();

            if ((screens != null) && (screens.Count > 0))
            {
                Container.NavigationController.PushViewController(new LessonScreenViewController(screens, 0), true);
            }
            else
            {
                var alert = UIAlertController.Create("Oops!", "There is nothing in this lesson.", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                Container.PresentViewController(alert, true, null); 
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

            cell.TextLabel.Text = Items[indexPath.Row].Title;

            // If lesson is finished then add check mark
            if (true) // Place holder!!
            {
                cell.Accessory = UITableViewCellAccessory.Checkmark; 
            }

            return cell;
        }
    }
}

