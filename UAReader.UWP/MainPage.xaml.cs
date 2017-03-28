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
            deviceFamily = Helper.CurrDeviceFamily;
            NavigationCacheMode = NavigationCacheMode.Enabled;
            size = Helper.MeasureStringSize(new MeasureFontRequest() { Input = "a", MaxLines = 1, FontSize = Textbox1.FontSize });            
        }

        
        

        private Size size;
        private DeviceFamily deviceFamily;
        private SystemNavigationManager currentView;
        private ObservableCollection<MenuModel> menuList;
        private ObservableCollection<FileListModel> fileList;


        private void AddEvent()
        {
            currentView.BackRequested += Page_BackRequested;
            mainFrame.Navigated += MainFrame_Navigated;
            grid.SizeChanged += Textbox_SizeChanged;
        }

        private void RemoveEvent()
        {
            currentView.BackRequested -= Page_BackRequested;
            mainFrame.Navigated -= MainFrame_Navigated;
            grid.SizeChanged -= Textbox_SizeChanged;
        }

        #region event
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePadding();
            AddEvent();
            menuList = InitMenuList();
            fileList = InitFileList();
            mainNavigationList.ItemsSource = menuList;
            var result = mainFrame.Navigate(typeof(FileListView), fileList);
            mainNavigationList.SelectedIndex = 0;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            RemoveEvent();
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
            bool result = false;
            if (selectedItem == null || selectedItem != clickItem)
            {
                switch (clickItem.MenuType)
                {
                    case MenuType.FileList:
                        result = mainFrame.Navigate(typeof(FileListView), fileList);
                        break;
                    case MenuType.Setting:
                        result = mainFrame.Navigate(typeof(SettingView));
                        break;
                    case MenuType.OpenFile:
                        OpenFile();
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
                new MenuModel() {
                    MenuType = MenuType.OpenFile,
                    Icon = "",
                    Desc = "打开文件（test）"
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

        private void Pre_Click(object sender, RoutedEventArgs e)
        {
            if (currIndex > 0)
            {
                s = fullS.Substring(0, currIndex);
                UpdateText(OperateType.Prep);
            }
            else
            {
                s = fullS;
                UpdateText(OperateType.None);
            }
            //if (preIndexStack.Count > 0)
            //{
            //    var last = preIndexStack.Last();
            //    if (!string.IsNullOrEmpty(fullS) && fullS.Length > last)
            //    {
            //        s = fullS.Substring(last);
            //        preIndexStack.RemoveAt(preIndexStack.Count - 1);
            //        UpdateText(false);
            //    } 
            //}
            sv.ChangeView(0, sv.VerticalOffset - sv.ActualHeight, sv.ZoomFactor);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (index > 0 && fullS != null && currIndex < fullS.Length)
            {           
                UpdateText(OperateType.Next);
            }
            sv.ChangeView(0, sv.VerticalOffset + sv.ActualHeight, sv.ZoomFactor);
        }
        string s = string.Empty;
        string fullS = string.Empty;
        int index = -1;
        int currIndex = 0;
        //List<int> preIndexStack = new List<int>();
        private async void OpenFile()
        {
            var list = await Helper.GetFileList(new List<string> { ".txt",".js" });
            if (list != null && list.Count > 0)
            {
                var file = list[0];
                currIndex = 0;
                //preIndexStack.Clear();
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    using (StreamReader read = new StreamReader(stream))
                    {
                        Textbox1.Text = fullS = s = read.ReadToEnd();
                        sv.ChangeView(0, 0, sv.ZoomFactor);
                        UpdateText(OperateType.None);
                    }
                }
            }

        }

        private void Textbox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            grid.SizeChanged -= Textbox_SizeChanged;
            UpdatePadding();
            UpdateText(OperateType.None);
            grid.SizeChanged += Textbox_SizeChanged;
        }

        private void UpdatePadding()
        {
            if (size.Height > 0)
            {
                var height = size.Height * (int)(grid.ActualHeight / size.Height);
                if (grid.ActualHeight > height)
                    sv.Margin = new Thickness(0, 0, 0, grid.ActualHeight - height);
                //Textbox1.MaxLines = (int)(grid.ActualHeight / size.Height);
            }
        }

        private void UpdateText(OperateType op)
        {
            try
            {
                s = currIndex <= 0 ? fullS : fullS.Substring(currIndex);
                index = Helper.GetStringAvailableIndex(new MeasureFontRequest()
                {
                    Input = s,
                    FontFamily = Textbox.FontFamily,
                    FontSize = Textbox.FontSize,
                    AvailableSize = new Size(grid.ActualWidth, grid.ActualHeight),
                    IsNext = op != OperateType.Prep,
                });
                if (index > 0)
                {
                    switch (op)
                    {
                        case OperateType.None:
                            Textbox.Text = s.Substring(0, index);
                            break;
                        case OperateType.Next:
                            currIndex += index;
                            Textbox.Text = s.Substring(currIndex, index);
                            break;
                        case OperateType.Prep:
                            currIndex -= index;
                            Textbox.Text = fullS.Substring(currIndex, index);
                            break;
                    }
                }
                else
                {
                    Textbox.Text = "";
                }
            }
            catch (Exception ex) {
                Textbox.Text = ex.ToString();
            }
        }
    }

    public enum MenuType
    {
        //Menu,
        FileList,
        Setting,
        OpenFile
    }

    public enum OperateType
    {
        None,
        Prep,
        Next,
    }
}
