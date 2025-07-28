using Microsoft.AspNetCore.Mvc;
using PdfDocument = IronPdf.PdfDocument;

namespace Formattica.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpPost("pdf-to-word")]
        public async Task<IActionResult> ConvertPdfToWord([FromBody] IFormFile pdfFile)
        {
            if(pdfFile == null || pdfFile.Length == 0)
                return BadRequest("Please upload a valid PDF file.");

            // Step 1: Save the uploaded PDF to a temporary file
            var tempPdfPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pdf");

            await using(var stream = new FileStream(tempPdfPath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            try
            {
                // Step 2: Load the PDF and convert it to Word
                var pdfDoc = PdfDocument.FromFile(tempPdfPath);
                var tempDocxPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".docx");

                pdfDoc.SaveAs(tempDocxPath);

                if(!System.IO.File.Exists(tempDocxPath))
                    return StatusCode(500, "Conversion failed. Word file not found.");

                // Step 3: Read the Word file and return it as a response
                var wordBytes = await System.IO.File.ReadAllBytesAsync(tempDocxPath);
                var fileName = "Converted_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".docx";

                return File(wordBytes,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    fileName);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Conversion error: {ex.Message}");
            }
            finally
            {
                // Step 4: Clean up temp files
                if(System.IO.File.Exists(tempPdfPath))
                    System.IO.File.Delete(tempPdfPath);
            }
        }
































        /*private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

        private readonly Dictionary<string, int> _imageQualityPresets = new(StringComparer.OrdinalIgnoreCase)
        {
            { "low", 25 },     // 25% quality
            { "medium", 50 },  // 50% quality
            { "high", 75 }     // 75% quality
        };

        [HttpPost("compress")]
        public async Task<IActionResult> Compress(IFormFile file, string quality)
        {
            if(file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            try
            {
                if(ext == ".pdf")
                {
                    return BadRequest("PDF compression is not implemented.");
                }

                var result = await CompressFile(file, quality);
                return File(result.CompressedBytes, result.ContentType, result.FileName);
            }
            catch(NotSupportedException ex)
            {
                return BadRequest($"Unsupported file type: {ext}. {ex.Message}");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the file. " + ex.Message);
            }
        }

        private async Task<(byte[] CompressedBytes, string ContentType, string FileName)> CompressFile(IFormFile file, string qualityKey)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if(!_allowedImageExtensions.Contains(ext))
                throw new NotSupportedException("Only JPG, PNG, and WEBP formats are supported.");

            float resizeFactor = 1f;
            int jpegWebpQuality = 90;
            PngCompressionLevel pngCompression = PngCompressionLevel.Level2;

            switch(qualityKey.ToLower())
            {
                case "low":
                    resizeFactor = 0.80f;
                    jpegWebpQuality = 50;
                    pngCompression = PngCompressionLevel.Level9; // most compressed
                    break;

                case "medium":
                    resizeFactor = 0.90f;
                    jpegWebpQuality = 75;
                    pngCompression = PngCompressionLevel.Level6; // medium compression
                    break;

                case "high":
                default:
                    resizeFactor = 1f;
                    jpegWebpQuality = 90;
                    pngCompression = PngCompressionLevel.Level2; // low compression
                    break;
            }

            await using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            using var image = await Image.LoadAsync(inputStream);

            // Resize if needed
            if(resizeFactor < 1f)
            {
                int newWidth = (int)(image.Width * resizeFactor);
                int newHeight = (int)(image.Height * resizeFactor);
                image.Mutate(x => x.Resize(newWidth, newHeight));
            }

            byte[] compressedBytes;
            string contentType;
            string downloadFileName;

            using var outputStream = new MemoryStream();

            if(ext == ".png")
            {
                await image.SaveAsPngAsync(outputStream, new PngEncoder
                {
                    CompressionLevel = pngCompression,
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

            compressedBytes = outputStream.ToArray();
            downloadFileName = $"compressed_{Path.GetFileName(file.FileName)}";

            return (compressedBytes, contentType, downloadFileName);
        }*/




        /*private async Task<(byte[] CompressedBytes, string ContentType, string FileName)> CompressFile(IFormFile file, string qualityKey)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if(!_allowedImageExtensions.Contains(ext))
                throw new NotSupportedException("Only JPG, PNG, and WEBP formats are supported.");

            // Quality values
            float resizeFactor;
            PngCompressionLevel compressionLevel;

            switch(qualityKey.ToLower())
            {
                case "low":
                    resizeFactor = 0.90f; // small resize only
                    compressionLevel = PngCompressionLevel.Level2; // low compression
                    break;

                case "medium":
                    resizeFactor = 0.92f;                           // Keep full resolution
                    compressionLevel = PngCompressionLevel.Level6; // Medium compression
                    break;

                case "high":
                default:
                    resizeFactor = 1f; // full size
                    compressionLevel = PngCompressionLevel.Level9; // maximum compression
                    break;
            }




            await using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            using var image = await Image.LoadAsync(inputStream);

            if(resizeFactor < 1f)
            {
                int newWidth = (int)(image.Width * resizeFactor);
                int newHeight = (int)(image.Height * resizeFactor);
                image.Mutate(x => x.Resize(newWidth, newHeight));
            }

            byte[] compressedBytes;
            string contentType;
            string downloadFileName;

            using var outputStream = new MemoryStream();

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
                int jpegQuality = qualityKey.ToLower() switch
                {
                    "low" => 50,
                    "medium" => 75,
                    "high" => 90,
                    _ => 75
                };

                await image.SaveAsJpegAsync(outputStream, new JpegEncoder
                {
                    Quality = jpegQuality
                });

                contentType = "image/jpeg";
            }
            else if(ext == ".webp")
            {
                int webpQuality = qualityKey.ToLower() switch
                {
                    "low" => 50,
                    "medium" => 75,
                    "high" => 90,
                    _ => 75
                };

                await image.SaveAsWebpAsync(outputStream, new WebpEncoder
                {
                    Quality = webpQuality
                });

                contentType = "image/webp";
            }
            else
            {
                throw new NotSupportedException("Unsupported image format.");
            }

            compressedBytes = outputStream.ToArray();
            downloadFileName = $"compressed_{Path.GetFileName(file.FileName)}";

            return (compressedBytes, contentType, downloadFileName);
        }*/



    }
}
