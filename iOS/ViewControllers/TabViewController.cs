using System;
using UIKit;
using System.Collections.Generic;

namespace CorpTraining.iOS
{
    public class TabViewController : UITabBarController
    {
        public List<UIViewController> Tabs { get; set; }

        public TabViewController()
        {
            Tabs = new List<UIViewController>();

            var lessonNavigation = new UINavigationController(new LessonsViewController());
            lessonNavigation.TabBarItem.Image = UIImage.FromBundle("book.png");
            lessonNavigation.TabBarItem.Title = "Lessons";
            Tabs.Add(lessonNavigation);

            var accountTab = new AccountViewController();
            Tabs.Add(accountTab);

//            var adminTab = new AdminViewController();
//            Tabs.Add(adminTab);

            var settingsTab = new SettingsViewController();
            Tabs.Add(settingsTab);

            ViewControllers = Tabs.ToArray();
        }
    }
}

