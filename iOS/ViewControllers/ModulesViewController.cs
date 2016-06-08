using System;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace CorpTraining.iOS
{
    public class ModulesViewController : UIViewController
    {
        public IList<Module> Modules { get; set; }

        public ModulesViewController()
            : base()
        {
            Title = "Modules";
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            var modulesTable = new UITableView();
            View.AddSubview(modulesTable);

            View.ConstrainLayout(() =>
                modulesTable.Frame.Top == View.Frame.Top &&
                modulesTable.Frame.Left == View.Frame.Left &&
                modulesTable.Frame.Right == View.Frame.Right &&
                modulesTable.Frame.Bottom == View.Frame.Bottom
            );

            // Loading indicator
            var loadingOverlay = new LoadingOverlay(View.Bounds);
            View.AddSubview(loadingOverlay);

            try
            {
                Modules = await LessonUtil.GetModulesAsync();
            }
            catch (Exception e)
            {
                var alert = UIAlertController.Create("Something goes wrong", e.Message, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(alert, true, null); 
            }

            if (Modules != null)
            {
                modulesTable.Source = new ModulesTableSource(this, Modules);
                modulesTable.ReloadData();
            }

            loadingOverlay.HideThenRemove();
        }
    }

    public class ModulesTableSource : UITableViewSource
    {
        public UIViewController Container { get; private set; }

        public IList<Module> Items { get; set; }

        private string cellIdentifier = "TableCell";

        public ModulesTableSource(UIViewController container, IList<Module> items)
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

            IList<Lesson> lessons = null;
            try
            {
                lessons = await LessonUtil.GetLessonsByModuleAsync(Items[indexPath.Row].Id);
            }
            catch (Exception e)
            {
                var alert = UIAlertController.Create("Something goes wrong", e.Message, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                Container.PresentViewController(alert, true, null);
            }
            loadingOverlay.HideThenRemove();

            if ((lessons != null) && (lessons.Count > 0))
            {
                Container.NavigationController.PushViewController(new LessonsViewController(lessons), true);
            }
            else
            {
                var alert = UIAlertController.Create("Oops!", "There is nothing in this module.", UIAlertControllerStyle.Alert);
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

            cell.TextLabel.Text = Items[indexPath.Row].Name;
            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator; 

            return cell;
        }
    }
}

