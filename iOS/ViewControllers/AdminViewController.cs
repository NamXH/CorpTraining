using System;
using UIKit;

namespace CorpTraining.iOS
{
    public class AdminViewController : UIViewController
    {
        public AdminViewController()
            : base()
        {
            TabBarItem.Image = UIImage.FromBundle("computer.png");
            TabBarItem.Title = "Admin";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
        }
    }
}

