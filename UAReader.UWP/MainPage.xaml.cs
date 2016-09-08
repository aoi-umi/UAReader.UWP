using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UAReader.UWP.Model;
using UAReader.UWP.View;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UAReader.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void mainNavigationList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as MenuModel;
            if (item == null) return;
            switch (item.MenuType)
            {
                case MenuType.Menu:
                    break;
                case MenuType.Test:
                    var result = mainFrame.Navigate(typeof(FileListView), fileList);
                    break;
            }
            MenuToggle();
        }

        private void Menu_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MenuToggle();
        }

        public void MenuToggle()
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
        }

        private ObservableCollection<MenuModel> menuList;
        private ObservableCollection<FileListModel> fileList;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            menuList = new ObservableCollection<MenuModel>() {
                new MenuModel() {
                    MenuType = MenuType.Menu,
                    Icon = "\ue700"
                },
                new MenuModel() {
                    MenuType = MenuType.Test,
                    Icon = "\ue700"
                },
            };
            fileList = new ObservableCollection<FileListModel>()
            {
                new FileListModel() {
                    Img = "ms-appx:///Assets/Square44x44Logo.png",
                    Title = "title1"
                },
                new FileListModel() {
                    Img = "ms-appx:///Assets/Square44x44Logo.png",
                    Title = "title2"
                },
            };
            mainNavigationList.ItemsSource = menuList;
        }
    }

    public enum MenuType
    {
        Menu,
        Test
    }
}
