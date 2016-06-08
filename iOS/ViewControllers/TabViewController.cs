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

            var lessonNavigation = new UINavigationController(new ModulesViewController());
            lessonNavigation.TabBarItem.Image = UIImage.FromBundle("book.png");
            lessonNavigation.TabBarItem.Title = "Lessons";
            Tabs.Add(lessonNavigation);

            var accountNavigation = new UINavigationController(new AccountViewController());
            accountNavigation.TabBarItem.Image = UIImage.FromBundle("user.png");
            accountNavigation.TabBarItem.Title = "Account";
            Tabs.Add(accountNavigation);

            var settingsNavigation = new UINavigationController(new SettingsViewController());
            settingsNavigation.TabBarItem.Image = UIImage.FromBundle("preferences.png");
            settingsNavigation.TabBarItem.Title = "Settings";
            Tabs.Add(settingsNavigation);

            ViewControllers = Tabs.ToArray();
        }
    }
}

