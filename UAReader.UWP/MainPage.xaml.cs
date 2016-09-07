using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UAReader.UWP.Model;
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
            switch (item.Name)
            {
                case "menu":
                    MenuToggle();
                    break;
            }
        }

        private void Menu_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MenuToggle();
        }

        private void MenuToggle()
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
        }

        private ObservableCollection<MenuModel> menuList;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            menuList = new ObservableCollection<MenuModel>() {
                new MenuModel() {
                    Name = "menu",
                    Icon = "\ue700"                    
                }
            };
            mainNavigationList.ItemsSource = menuList;
        }
    }
}
