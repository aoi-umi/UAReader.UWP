using System.Collections.Generic;
using System.Collections.ObjectModel;
using UAReader.UWP.Model;
using UmiAoi.UWP;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UAReader.UWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FileListView : Page
    {
        public FileListView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var fileList = e.Parameter as ObservableCollection<FileListModel>;
            if (fileList != null) mainListView.ItemsSource = fileList;
            FileTypeFilter = new List<string>() {
                ".txt"
            };
            base.OnNavigatedTo(e);
        }

        private IList<string> FileTypeFilter;
        public async void OpenFileDialog()
        {
            var list = await Helper.GetFileList(FileTypeFilter);
        }
    }
}
