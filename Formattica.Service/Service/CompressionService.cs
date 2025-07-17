using Formattica.Models;
using Formattica.Service.IService;
using Ghostscript.NET;
using Ghostscript.NET.Processor;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System.IO.Compression;

namespace Formattica.Service.Service
{
    public class CompressionService : ICompressionService
    {


        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".zip" };

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
        }


        public async Task<byte[]> CompressPdf(IFormFile file)
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
        }
    }
}
