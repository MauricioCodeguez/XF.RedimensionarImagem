using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.IO;
using System.Threading.Tasks;

namespace RedimensionarImagem.Services
{
    public class MediaService
    {
        private bool CanTakePhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                return false;

            return true;
        }

        private bool CanPickPhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
                return false;

            return true;
        }

        public async Task<byte[]> PickPhotoAsync()
        {
            if (CanPickPhoto())
            {
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    storageStatus = results[Permission.Storage];
                }

                if (storageStatus == PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.PickPhotoAsync();

                    if (file == null)
                        return null;

                    byte[] imagemByte = null;

                    using (var memoryStream = new MemoryStream())
                    {
                        file.GetStream().CopyTo(memoryStream);
                        file.Dispose();
                        imagemByte = memoryStream.ToArray();
                    }

                    return imagemByte;
                }
            }

            return null;
        }

        public async Task<byte[]> TakePhotoAsync()
        {
            if (CanTakePhoto())
            {
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

                if (cameraStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    cameraStatus = results[Permission.Camera];
                }

                if (cameraStatus == PermissionStatus.Granted)
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        AllowCropping = true,
                        SaveToAlbum = true
                    });

                    if (file == null)
                        return null;

                    byte[] imagemByte = null;

                    using (var memoryStream = new MemoryStream())
                    {
                        file.GetStream().CopyTo(memoryStream);
                        file.Dispose();
                        imagemByte = memoryStream.ToArray();
                    }

                    return imagemByte;
                }
            }

            return null;
        }
    }
}