using System;
using System.Collections.Generic;
using System.Text;

namespace RentApp.Application.Interfaces.External
{
    public interface IFileStorage
    {
        Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default);
        Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken = default);
        Task DeleteAsync(string path, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default);
    }
}
