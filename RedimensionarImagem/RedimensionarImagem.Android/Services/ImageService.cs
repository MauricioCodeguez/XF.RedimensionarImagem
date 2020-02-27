using RedimensionarImagem.Droid.Services;
using RedimensionarImagem.Services;
using SkiaSharp;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageService))]

namespace RedimensionarImagem.Droid.Services
{
    public class ImageService : IImageService
    {
        private Size CalcularDimensoes(int width, int height, int largura)
        {
            Size newSize = new Size();

            if (height > width)
            {
                newSize.Width = (int)(width * ((float)largura / (float)height));
                newSize.Height = largura;
            }
            else
            {
                newSize.Width = largura;
                newSize.Height = (int)(height * ((float)largura / (float)width));
            }
            return newSize;
        }

        public MemoryStream RedimensionarImagem(Stream pStream, DimensaoImagemEnum pDimensaoImagemEnum)
        {
            SKBitmap bitmapOld = SKBitmap.Decode(new SKManagedStream(pStream));

            var newSize2 = CalcularDimensoes(bitmapOld.Width, bitmapOld.Height, (int)pDimensaoImagemEnum);

            SKBitmap bitmap = new SKBitmap((int)newSize2.Width, (int)newSize2.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul);

            SKSurface surface = SKSurface.Create(bitmap.Info);
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            var paint = new SKPaint();
            paint.IsAntialias = true;

            canvas.DrawBitmap(bitmapOld, new SKRect()
            {
                Location = new SKPoint(0, 0),
                Size = new SKSize((float)newSize2.Width, (float)newSize2.Height)
            }, paint);

            var data = surface.Snapshot().Encode(SKEncodedImageFormat.Jpeg, 80);

            MemoryStream ms = new MemoryStream();
            data.SaveTo(ms);
            ms.Position = 0;

            return ms;
        }
    }
}