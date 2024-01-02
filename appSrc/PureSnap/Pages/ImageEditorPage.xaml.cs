using Microsoft.Win32;
using PureSnap.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;


namespace PureSnap.Pages
{
    /// <summary>
    /// Логика взаимодействия для ImageEditorPage.xaml
    /// </summary>
    public partial class ImageEditorPage : Page, INotifyPropertyChanged
    {
        private NavigationService Navigator;
        ImageGenerator ImageGenerator = new ImageGenerator();
        public ImageEditorPage(NavigationService navigator)
        {
            InitializeComponent();
            
            this.DataContext = this;
            Navigator = navigator;
            Templater = new Templater();
            
        }
        


        private int imageWidth = 0;
        private int imageHeight = 0;
        public int ImageWidth { get { return imageWidth; } set { imageWidth = value; OnPropertyChanged(nameof(ImageWidth)); } }
        public int ImageHeight { get { return imageHeight; } set { imageHeight = value; OnPropertyChanged(nameof(ImageHeight)); } }

        public System.Drawing.Image EditImage { get; set; }
        public string EditImagePath { get; set; }

        private Templater Templater { get; set; }
        private string TemplateContent = "";



        public double CornerRadius = 10;
        public int Margin = 50;
        private void LoadImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                
                LoadImageBtn.Content = openFileDialog.SafeFileName;
                EditImagePath = openFileDialog.FileName.ToString();
                EditImage = System.Drawing.Image.FromFile(EditImagePath);

                
                ImageWidth = EditImage.Width + 2* Margin;
                ImageHeight = EditImage.Height + 2 * Margin;

                BitmapSource overlayBitmapSource = ConvertToBitmapSource(EditImage);
                overlayBitmapSource = ImageGenerator.RoundCorners(overlayBitmapSource, CornerRadius);

                DrawingVisual drawingVisual = new DrawingVisual();

                var BG = ImageGenerator.GenerateImage(ImageWidth, ImageHeight);
                Debug.WriteLine(overlayBitmapSource.Width + " " + overlayBitmapSource.Height);
                using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                {
                    double offsetX = (BG.Width - overlayBitmapSource.PixelWidth) / 2;
                    double offsetY = (BG.Height - overlayBitmapSource.PixelHeight) / 2;
                    // Рисуем основное изображение
                    drawingContext.DrawImage(BG, new Rect(0, 0, BG.Width, BG.Height));


                    

                    // Рисуем изображение для наложения
                    drawingContext.DrawImage(overlayBitmapSource, new Rect(offsetX, offsetY, overlayBitmapSource.PixelWidth, overlayBitmapSource.PixelHeight));
                }
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                (int)ImageWidth,
                (int)ImageHeight,
                96, 96, PixelFormats.Pbgra32);

                // Рендерим DrawingVisual в RenderTargetBitmap
                renderTargetBitmap.Render(drawingVisual);
                BitmapSource resultBitmapSource = renderTargetBitmap;

                ResultImage.Source = resultBitmapSource;// ImageGenerator.GenerateImage(ImageWidth, ImageHeight);
            }
        }
        public BitmapSource ConvertToBitmapSource(System.Drawing.Image drawingImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Сохраняем изображение в поток
                drawingImage.Save(memoryStream, ImageFormat.Png);

                // Создаем BitmapSource из MemoryStream
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigator.GoBack();
        }
    }
}
