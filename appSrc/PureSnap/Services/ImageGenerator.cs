using PureSnap.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace PureSnap.Services
{
    public class ImageGenerator
    {
        private GradientDiagonal GradientDiagonal { get; set; }
        public ImageGenerator() 
        {
            GradientDiagonal = new GradientDiagonal();
        }
        public BitmapSource GenerateImage(int width, int height, string StartHex, string EndHex) 
        {
            BitmapSource resultImage = null;
            resultImage = GradientDiagonal.GenerateBackground((Color)ColorConverter.ConvertFromString(StartHex), (Color)ColorConverter.ConvertFromString(EndHex), width, height);
            return resultImage;
        }

        public BitmapSource RoundCorners(BitmapSource source, double cornerRadius)
        {
            // Создаем DrawingGroup
            var drawingGroup = new DrawingGroup();
            var geometry = new RectangleGeometry(new Rect(0, 0, source.PixelWidth, source.PixelHeight), cornerRadius, cornerRadius);
            drawingGroup.Children.Add(new GeometryDrawing(new ImageBrush(source), null, geometry));

            // Преобразуем DrawingGroup в DrawingVisual
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawDrawing(drawingGroup);
            }

            // Рендерим DrawingVisual в RenderTargetBitmap
            var renderTargetBitmap = new RenderTargetBitmap(
                source.PixelWidth,
                source.PixelHeight,
                96, 96, PixelFormats.Pbgra32);

            renderTargetBitmap.Render(drawingVisual);

            return renderTargetBitmap;
        }
    }
}
