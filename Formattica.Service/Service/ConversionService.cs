using Formattica.Service.IService;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;

namespace Formattica.Service.Service
{
    public class ConversionService: IConversionService
    {

        public async Task<(byte[]? ConvertedBytes, string? ContentType, string? FileExtension)> ConvertImage(IFormFile file, string targetFormat)
        {
            if(file == null || file.Length == 0 || string.IsNullOrEmpty(targetFormat))
                return (null, null, null);

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using var image = await Image.LoadAsync(memoryStream);
            using var output = new MemoryStream();

            string contentType;
            string fileExtension;

            switch(targetFormat.ToLower())
            {
                case "jpeg":
                case "jpg":
                    await image.SaveAsJpegAsync(output, new JpegEncoder { Quality = 85 });
                    contentType = "image/jpeg";
                    fileExtension = "jpg";
                    break;

                case "png":
                    await image.SaveAsPngAsync(output, new PngEncoder());
                    contentType = "image/png";
                    fileExtension = "png";
                    break;

                case "gif":
                    await image.SaveAsGifAsync(output, new GifEncoder());
                    contentType = "image/gif";
                    fileExtension = "gif";
                    break;

                case "bmp":
                    await image.SaveAsBmpAsync(output, new BmpEncoder());
                    contentType = "image/bmp";
                    fileExtension = "bmp";
                    break;

                case "tiff":
                    await image.SaveAsTiffAsync(output, new TiffEncoder());
                    contentType = "image/tiff";
                    fileExtension = "tiff";
                    break;

                case "webp":
                    await image.SaveAsWebpAsync(output, new WebpEncoder());
                    contentType = "image/webp";
                    fileExtension = "webp";
                    break;

                default:
                    return (null, null, null);
            }

            return (output.ToArray(), contentType, fileExtension);
        }
    }
}
