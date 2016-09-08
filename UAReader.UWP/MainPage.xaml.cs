using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UAReader.UWP.Model;
using UAReader.UWP.View;
using UmiAoi.UWP;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
            currentView = SystemNavigationManager.GetForCurrentView();
            deviceFamily = Helper.GetDeviceFamily();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private DeviceFamily deviceFamily;
        private SystemNavigationManager currentView;
        private ObservableCollection<MenuModel> menuList;
        private ObservableCollection<FileListModel> fileList;

        #region event
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            currentView.BackRequested += Page_BackRequested;
            mainFrame.Navigated += MainFrame_Navigated;
            menuList = InitMenuList();
            fileList = InitFileList();
            mainNavigationList.ItemsSource = menuList;
            var result = mainFrame.Navigate(typeof(FileListView), fileList);
            mainNavigationList.SelectedIndex = 0;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            currentView.BackRequested -= Page_BackRequested;
            mainFrame.Navigated -= MainFrame_Navigated;
        }

        private void Page_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (mainFrame.CanGoBack)
            {
                mainFrame.GoBack();
                e.Handled = true;
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (deviceFamily == DeviceFamily.Desktop) {
                currentView.AppViewBackButtonVisibility = mainFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void mainNavigationList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickItem = e.ClickedItem as MenuModel;            
            if (clickItem == null) return;
            var selectedItem = mainNavigationList.SelectedItem as MenuModel;
            if (selectedItem == null || selectedItem != clickItem)
            {
                switch (clickItem.MenuType)
                {
                    case MenuType.FileList:
                        var result = mainFrame.Navigate(typeof(FileListView), fileList);
                        break;
                }
            }
            MenuToggle();
        }

        private void Menu_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MenuToggle();
        }
        #endregion

        public void MenuToggle()
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
        }

        #region Init
        private ObservableCollection<MenuModel> InitMenuList()
        {
            return new ObservableCollection<MenuModel>() {
                //new MenuModel() {
                //    MenuType = MenuType.Menu,
                //    Icon = "\ue700"
                //},
                new MenuModel() {
                    MenuType = MenuType.FileList,
                    Icon = "\uE1D3",
                    Desc = "文件列表"
                },
                new MenuModel() {
                    MenuType = MenuType.Setting,
                    Icon = "\uE713",
                    Desc = "设置"
                },
            };
        }

        private ObservableCollection<FileListModel> InitFileList()
        {
            return new ObservableCollection<FileListModel>()
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
        }
        #endregion
    }

    public enum MenuType
    {
        //Menu,
        FileList,
        Setting
    }
}
