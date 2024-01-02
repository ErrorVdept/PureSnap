using PureSnap.Pages;
using PureSnap.Service;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PureSnap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<Page> pages = new List<Page>();
        public NavigationService NavigatorService { get; set; }
        public MainWindow()
        {
            //DirExplorer.InitApp();
            InitializeComponent();
            
            NavigatorService = Navigator.NavigationService;
            pages.Add(new StartPage(NavigatorService));
            CurrentPage = pages[0];
            //Editor.DataContext = this;
            //<wndw:EditorWindow Name="Editor" IsVisible="{Binding !IsStartScreen}"/>
        }
        private Page currentPage;
        public Page CurrentPage { get { return currentPage; } set { currentPage = value; OnPropertyChanged(nameof(CurrentPage)); NavigatorService.Navigate(currentPage); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        
    }
}