using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PureSnap
{
    public partial class EditorWindow : UserControl, INotifyPropertyChanged
    {
        public EditorWindow()
        {
            InitializeComponent();
            LoadImageBtn.Content = "Load Image";
            LoadImageBtn.DataContext = this;
            ImgWidth.DataContext = this;
            ImgHeight.DataContext = this;
            Templater = new Templater();
        }
        private int imageWidth = 0;
        private int imageHeight = 0;
        public int ImageWidth { get { return imageWidth; } set {  imageWidth = value; OnPropertyChanged(nameof(ImageWidth)); } }
        public int ImageHeight { get {  return imageHeight; } set { imageHeight = value; OnPropertyChanged(nameof(ImageHeight)); } }

        public System.Drawing.Image EditImage { get; set; }
        public string EditImagePath { get; set; }

        private Templater Templater { get; set; }
        private string TemplateContent = "";

        private string resultHtml = "";
        public string ResultHtml { get { return resultHtml; } set { resultHtml = value; OnPropertyChanged(nameof(ResultHtml)); } }

        public async Task LoadImage()
        {
            // Get top level from the current control. Alternatively, you can use Window reference instead.
            var topLevel = TopLevel.GetTopLevel(this);

            // Start async operation to open the dialog.
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false
            });

            if (files.Count != 0)
            {
                LoadImageBtn.Content = files[0].Name;
                EditImagePath = files[0].Path.ToString().Replace("file:///", "");
                EditImage = System.Drawing.Image.FromFile(EditImagePath);
                
                ImageWidth = EditImage.Width + 100;
                ImageHeight = EditImage.Height + 100;

                using (StreamReader sr = new StreamReader("Templates/Screens/ScreenMainTemplate.html"))
                {
                    TemplateContent = sr.ReadToEnd();
                }
                ResultHtml = Templater.ProduceScreenTemplate(TemplateContent, EditImagePath, ImageWidth, ImageHeight);
                Debug.WriteLine(ResultHtml);
                //_htmlPanel.Text = ResultHtml;
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
