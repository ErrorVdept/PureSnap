using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PureSnap.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page, INotifyPropertyChanged
    {
        private NavigationService Navigator;
        private Page ImageEditorPage;
        public StartPage(NavigationService navigator)
        {
            InitializeComponent();
            Navigator = navigator;
            ImageEditorPage = new ImageEditorPage(Navigator);
            BtnEdit.DataContext = this;
            StartScreen.DataContext = this;
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

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Navigator.Navigate(ImageEditorPage);
        }
    }
}
