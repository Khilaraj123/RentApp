using System;
using System.Collections.Generic;
using System.Text;

namespace RentApp.Application.Interfaces.External
{
    public class ImageProcessingResult
    {
        public Stream Stream { get; set; } = Stream.Null;
        public string ContentType { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public long SizeBytes { get; set; }
    }

    public class ImageMetadata
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; } = string.Empty;
    }

    public interface IImageProcessor
    {
        Task<ImageProcessingResult> ResizeAsync(Stream imageStream, int maxWidth, int maxHeight, CancellationToken cancellationToken = default);
        Task<ImageProcessingResult> ConvertToWebPAsync(Stream imageStream, int quality = 80, CancellationToken cancellationToken = default);
        Task<ImageMetadata> GetMetadataAsync(Stream imageStream, CancellationToken cancellationToken = default);
    }
}
