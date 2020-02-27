using System.IO;

namespace RedimensionarImagem.Services
{
    public interface IImageService
    {
        MemoryStream RedimensionarImagem(Stream pStream, DimensaoImagemEnum pDimensaoImagemEnum);
    }
}