using Formattica.Service.IService;
using Ghostscript.NET;
using Ghostscript.NET.Processor;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Formattica.Service.Service
{
    public class CompressionService : ICompressionService
    {
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

/*        private readonly Dictionary<string, int> _imageQualityPresets = new(StringComparer.OrdinalIgnoreCase)
        {
           { "extremelow", 10 },
           { "verylow", 30 },
           { "low", 50 },
           { "medium", 70 },
           { "high", 85 },
           { "veryhigh", 95 }
        };*/

        private readonly Dictionary<string, int> _imageQualityPresets = new(StringComparer.OrdinalIgnoreCase)
        {
            { "extreme", 5 },  // --verylow
            { "minimum", 5 },

            { "extremelow", 10 },
            { "ultralow", 15 },   // --low
            { "verylow", 30 },
            { "low", 50 },

            { "medium", 70 },
            { "normal", 70 },    //Moderate     
            { "default", 75 },      

            { "high", 85 },      // --medium

            { "veryhigh", 95 },
            { "ultrahigh", 97 },  //--high

            { "max", 100 },
            { "maximum", 100 }    // --veryhigh
        };

        private readonly Dictionary<string, (string PdfSetting, int Dpi)> _pdfQualityPresets = new(StringComparer.OrdinalIgnoreCase)
        {
            { "veryextreme", ("screen", 36) }, // --verylow
            { "minimum", ("screen", 36) },

            { "extreme", ("screen", 50) },     // --low
            { "extremelow", ("screen", 50) },

            { "ultralow", ("screen", 60) },

            { "verylow", ("screen", 72) },     //Moderate
            { "low", ("screen", 72) },

            { "medium", ("ebook", 100) },      // --medium
            { "normal", ("ebook", 100) },
            { "default", ("ebook", 100) },
            { "high", ("printer", 150) },
            { "veryhigh", ("prepress", 300) },  //--high
            { "ultrahigh", ("prepress", 350) },

            { "max", ("prepress", 400) },       // --veryhigh
            { "maximum", ("prepress", 400) }
        };


        public async Task<(byte[] CompressedBytes, string ContentType, string FileName)> CompressFile(IFormFile file, string qualityKey)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if(!_allowedImageExtensions.Contains(ext))
                throw new NotSupportedException("Only JPG, PNG, and WEBP formats are supported.");

            float resizeFactor;
            int jpegWebpQuality;
            PngCompressionLevel compressionLevel;

            switch(qualityKey.Trim().ToLower())
            {
                case "low":
                    resizeFactor = 0.90f;
                    compressionLevel = PngCompressionLevel.Level2;
                    jpegWebpQuality = 50;
                    break;

                case "medium":
                    resizeFactor = 0.95f; // medium is slightly better than low
                    compressionLevel = PngCompressionLevel.Level6;
                    jpegWebpQuality = 75;
                    break;

                case "high":
                default:
                    resizeFactor = 1f;
                    compressionLevel = PngCompressionLevel.Level9;
                    jpegWebpQuality = 90;
                    break;
            }

            await using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            using var image = await Image.LoadAsync(inputStream);

            int originalWidth = image.Width;
            int originalHeight = image.Height;

            int newWidth = (int)(originalWidth * resizeFactor);
            int newHeight = (int)(originalHeight * resizeFactor);

            // Resize only if needed
            if(newWidth != originalWidth || newHeight != originalHeight)
            {
                image.Mutate(x => x.Resize(newWidth, newHeight));
            }

            await using var outputStream = new MemoryStream();

            string contentType;

            if(ext == ".png")
            {
                await image.SaveAsPngAsync(outputStream, new PngEncoder
                {
                    CompressionLevel = compressionLevel,
                    ColorType = PngColorType.Rgb
                });

                contentType = "image/png";
            }
            else if(ext == ".jpg" || ext == ".jpeg")
            {
                await image.SaveAsJpegAsync(outputStream, new JpegEncoder
                {
                    Quality = jpegWebpQuality
                });

                contentType = "image/jpeg";
            }
            else if(ext == ".webp")
            {
                await image.SaveAsWebpAsync(outputStream, new WebpEncoder
                {
                    Quality = jpegWebpQuality
                });

                contentType = "image/webp";
            }
            else
            {
                throw new NotSupportedException("Unsupported image format.");
            }

            return (outputStream.ToArray(), contentType, $"compressed_{Path.GetFileName(file.FileName)}");
        }


        public async Task<byte[]> CompressPdf(IFormFile file, string quality)
        {
            if(file == null || file.Length == 0)
                return null!;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if(ext != ".pdf")
                return null!;

            var tempPath = Path.GetTempPath();
            var inputFile = Path.Combine(tempPath, $"{Guid.NewGuid()}_{file.FileName}");
            var outputFile = Path.Combine(tempPath, $"compressed_{Guid.NewGuid()}_{file.FileName}");

            try
            {
                await using(var stream = new FileStream(inputFile, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Apply quality preset
                var preset = _pdfQualityPresets.TryGetValue(quality, out var profile)
                    ? profile
                    : ("screen", 72); // default fallback

                string pdfSetting = profile.PdfSetting;
                int dpi = profile.Dpi;

/*                switch(quality.ToLower())
                {
                    case "low":
                        pdfSetting = "screen";
                        dpi = 72;
                        break;
                    case "medium":
                        pdfSetting = "ebook";
                        dpi = 100;
                        break;
                    case "high":
                        pdfSetting = "printer";
                        dpi = 150;
                        break;
                    case "veryhigh":
                        pdfSetting = "prepress";
                        dpi = 300;
                        break;
                    case "extreme":
                        pdfSetting = "screen";
                        dpi = 50;
                        break;
                    case "veryextreme":
                        pdfSetting = "screen";
                        dpi = 36;
                        break;
                    default:
                        pdfSetting = "screen";
                        dpi = 72;
                        break;
                }*/

                var gsDllPath = Path.Combine(Directory.GetCurrentDirectory(), "ghostscript", "gsdll64.dll");
                var gsVersion = new GhostscriptVersionInfo(gsDllPath);

                using var processor = new GhostscriptProcessor(gsVersion, true);

                var switches = new List<string>
                {
                    "-dNOPAUSE",
                    "-dBATCH",
                    "-sDEVICE=pdfwrite",
                    "-dCompatibilityLevel=1.4",
                    $"-dPDFSETTINGS=/{pdfSetting}",
                    "-dDownsampleColorImages=true",
                    "-dColorImageDownsampleType=/Average",
                    $"-dColorImageResolution={dpi}",
                    "-dDownsampleGrayImages=true",
                    "-dGrayImageDownsampleType=/Average",
                    $"-dGrayImageResolution={dpi}",
                    "-dDownsampleMonoImages=true",
                    "-dMonoImageDownsampleType=/Subsample",
                    $"-dMonoImageResolution={dpi}",
                    $"-sOutputFile={outputFile}",
                    inputFile
                };

                processor.StartProcessing(switches.ToArray(), null);

                return await File.ReadAllBytesAsync(outputFile);
            }
            finally
            {
                if(File.Exists(inputFile)) File.Delete(inputFile);
                if(File.Exists(outputFile)) File.Delete(outputFile);
            }
        }


        #region OldCode
        /*public async Task<byte[]> CompressPdf(IFormFile file)
{
    if(file == null || file.Length == 0)
        return null!;

    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
    if(ext != ".pdf")
        return null!;

    var tempPath = Path.GetTempPath();
    var inputFile = Path.Combine(tempPath, $"{Guid.NewGuid()}_{file.FileName}");
    var outputFile = Path.Combine(tempPath, $"compressed_{Guid.NewGuid()}_{file.FileName}");

    try
    {
        // Save uploaded file to temp
        using(var stream = new FileStream(inputFile, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Ghostscript DLL path (you must include gsdll64.dll in project under ghostscript folder)
        var gsDllPath = Path.Combine(Directory.GetCurrentDirectory(), "ghostscript", "gsdll64.dll");
        var gsVersion = new GhostscriptVersionInfo(gsDllPath);

        using(var processor = new GhostscriptProcessor(gsVersion, true))
        {
            var switches = new List<string>
        {
            "-dNOPAUSE",
            "-dBATCH",
            "-sDEVICE=pdfwrite",
            "-dCompatibilityLevel=1.4",
            "-dPDFSETTINGS=/screen",
            "-dDownsampleColorImages=true",
            "-dColorImageDownsampleType=/Average",
            "-dColorImageResolution=72",
            "-dDownsampleGrayImages=true",
            "-dGrayImageDownsampleType=/Average",
            "-dGrayImageResolution=72",
            "-dDownsampleMonoImages=true",
            "-dMonoImageDownsampleType=/Subsample",
            "-dMonoImageResolution=72",
            $"-sOutputFile={outputFile}",
            inputFile
        };

            processor.StartProcessing(switches.ToArray(), null);
        }

        return await File.ReadAllBytesAsync(outputFile);
    }
    finally
    {
        if(File.Exists(inputFile))
            File.Delete(inputFile);

        if(File.Exists(outputFile))
            File.Delete(outputFile);
    }
}*/
        #endregion

        #region Old Code
        /*public async Task<(byte[] CompressedBytes, string ContentType, string FileName)> CompressFile(IFormFile file, string qualitykey)
{
    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
    //int quality = int.TryParse(intquality, out var parsedQuality) ? parsedQuality : 75;

    if(!_allowedImageExtensions.Contains(ext))
        throw new NotSupportedException("Only JPG, PNG, and WEBP formats are supported.");

    int quality = _imageQualityPresets.TryGetValue(qualitykey, out var mappedQuality) ? mappedQuality : 75;

    await using var inputStream = new MemoryStream();
    await file.CopyToAsync(inputStream);
    inputStream.Position = 0;

    byte[] compressedBytes;
    string contentType;
    string downloadFileName;

    // Step 1: Load original image
    using var originalImage = await Image.LoadAsync(inputStream);

    // Step 2: Save image as JPEG into temp stream to compress it
    using var jpegTempStream = new MemoryStream();
    await originalImage.SaveAsJpegAsync(jpegTempStream, new JpegEncoder
    {
        Quality = quality
    });
    jpegTempStream.Position = 0;

    // Step 3: Load compressed JPEG image
    using var compressedJpegImage = await Image.LoadAsync(jpegTempStream);

    // Step 4: Convert back to original format
    using var outputStream = new MemoryStream();

    switch(ext)
    {
        case ".jpg":
        case ".jpeg":
            await compressedJpegImage.SaveAsJpegAsync(outputStream, new JpegEncoder { Quality = quality });
            contentType = "image/jpeg";
            break;

        case ".png":
            var clamped = Math.Clamp(quality / 10, 1, 9);
            await compressedJpegImage.SaveAsPngAsync(outputStream, new PngEncoder
            {
                CompressionLevel = (PngCompressionLevel)clamped // You can adjust this or make it dynamic
            });
            contentType = "image/png";
            break;

        case ".webp":
            await compressedJpegImage.SaveAsWebpAsync(outputStream, new WebpEncoder { Quality = quality });
            contentType = "image/webp";
            break;

        default:
            throw new NotSupportedException("Unsupported image format.");
    }

    compressedBytes = outputStream.ToArray();
    downloadFileName = $"compressed_{Path.GetFileName(file.FileName)}";

    return (compressedBytes, contentType, downloadFileName);
}*/


        /*private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        public async Task<(byte[] CompressedBytes, string ContentType, string FileName)> CompressFile(IFormFile file, string intquality)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            int quality = int.TryParse(intquality, out var parsedQuality) ? parsedQuality : 75;

            if(!_allowedImageExtensions.Contains(ext))
                throw new NotSupportedException("Only JPG, PNG, and WEBP formats are supported.");

            await using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            byte[] compressedBytes;
            string contentType;
            string downloadFileName;

            using var image = await Image.LoadAsync(inputStream);
            using var outputStream = new MemoryStream();

            switch(ext)
            {
                case ".jpg":
                case ".jpeg":
                    await image.SaveAsJpegAsync(outputStream, new JpegEncoder
                    {
                        Quality = quality // 1–100 (higher = better quality/larger size)
                    });
                    contentType = "image/jpeg";
                    break;

                case ".png":
                    await image.SaveAsPngAsync(outputStream, new PngEncoder
                    {
                        CompressionLevel = (PngCompressionLevel)quality // You can also use Level1–9 or Default
                    });
                    contentType = "image/png";
                    break;

                case ".webp":
                    await image.SaveAsWebpAsync(outputStream, new WebpEncoder
                    {
                        Quality = quality
                    });
                    contentType = "image/webp";
                    break;

                default:
                    throw new NotSupportedException("Unsupported image format.");
            }

            compressedBytes = outputStream.ToArray();
            downloadFileName = $"compressed_{Path.GetFileName(file.FileName)}";

            return (compressedBytes, contentType, downloadFileName);
        }*/


        /*private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".zip" };

        public async Task<(byte[] CompressedBytes, string ContentType, string FileName)> CompressFile(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if(!_allowedExtensions.Contains(ext))
                throw new NotSupportedException("Unsupported file type.");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            byte[] compressedBytes;
            string contentType;
            string downloadFileName;

            if(ext == ".zip")
            {
                using var output = new MemoryStream();
                using(var archive = new ZipArchive(output, ZipArchiveMode.Create, true))
                {
                    var entry = archive.CreateEntry(Path.GetFileName(file.FileName), CompressionLevel.SmallestSize);
                    using var entryStream = entry.Open();
                    memoryStream.CopyTo(entryStream);
                }

                compressedBytes = output.ToArray();
                contentType = "application/zip";
            }
            else
            {
                using var image = await Image.LoadAsync(memoryStream);
                using var output = new MemoryStream();

                if(ext == ".jpg" || ext == ".jpeg")
                {
                    var encoder = new JpegEncoder { Quality = 75 };
                    await image.SaveAsJpegAsync(output, encoder);
                    contentType = "image/jpeg";
                }
                else if(ext == ".png")
                {
                    var encoder = new PngEncoder { CompressionLevel = PngCompressionLevel.Level6 };
                    await image.SaveAsPngAsync(output, encoder);
                    contentType = "image/png";
                }
                else
                {
                    contentType = "application/octet-stream";
                }

                compressedBytes = output.ToArray();
            }

            downloadFileName = $"compressed_{Path.GetFileName(file.FileName)}";
            return (compressedBytes, contentType, downloadFileName);
        }*/
        #endregion
    }
}
