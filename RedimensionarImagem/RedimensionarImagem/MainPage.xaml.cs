using RedimensionarImagem.Services;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RedimensionarImagem
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        readonly IImageService _imageService;

        public MainPage()
        {
            InitializeComponent();
            _imageService = DependencyService.Resolve<IImageService>();
        }

        private async void TirarFoto_Clicked(object sender, System.EventArgs e) => await TirarFoto(1);

        private async void EscolherGaleria_Clicked(object sender, System.EventArgs e) => await TirarFoto(2);

        private async Task TirarFoto(int acao)
        {
            byte[] imagemOriginal;

            if (acao == 1)
                imagemOriginal = await new MediaService().TakePhotoAsync();
            else
                imagemOriginal = await new MediaService().PickPhotoAsync();

            if (imagemOriginal != null)
            {
                var imgGrande = Redimensionar(imagemOriginal, DimensaoImagemEnum.Grande);
                var imgMedia = Redimensionar(imagemOriginal, DimensaoImagemEnum.Medio);
                var imgMiniatura = Redimensionar(imagemOriginal, DimensaoImagemEnum.Miniatura);

                imgOriginal.Source = ConvertToImageSource(imagemOriginal);
                imagemGrande.Source = ConvertToImageSource(imgGrande);
                imagemMedia.Source = ConvertToImageSource(imgMedia);
                imagemMiniatura.Source = ConvertToImageSource(imgMiniatura);
            }
        }

        private ImageSource ConvertToImageSource(byte[] imgByte) => ImageSource.FromStream(() => new MemoryStream(imgByte));

        private byte[] Redimensionar(byte[] imagemOriginal, DimensaoImagemEnum dimensao)
        {
            var ms = _imageService.RedimensionarImagem(new MemoryStream(imagemOriginal), dimensao);
            return ms.ToArray();
        }
    }
}