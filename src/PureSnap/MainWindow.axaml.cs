using Avalonia.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PureSnap
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            BtnEdit.DataContext = this;
            StartScreen.DataContext = this;
            Editor.DataContext = this;
        }
        private bool isStartScreen = true;
        public bool IsStartScreen { get { return isStartScreen; } set { isStartScreen = value; OnPropertyChanged(nameof(IsStartScreen)); } }


        public void EditImage()
        {
            IsStartScreen = false;
            
        }
        public void CloseEditImage()
        {
            IsStartScreen = true;

        }


        




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}