using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TripYatri.Core.Base.Providers.Logger;
using TripYatri.Core.Base.Providers.Metrics;

namespace TripYatri.Core.Base.Providers.S3
{
    public class LocalS3Provider : IS3Provider
    {
        private readonly RuntimeEnvironment _runtimeEnvironment;
        private readonly IMetricsProvider _metricsProvider;
        private readonly ILogger _logger;
        private readonly string LOCAL_DRIVE = Path.GetTempPath();

        public LocalS3Provider(
            RuntimeEnvironment runtimeEnvironment,
            IMetricsProvider metricsProvider, ILogger logger)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _metricsProvider = metricsProvider;
            _logger = logger;
        }

        public Task<DeleteObjectsResponse> DeleteObjectsAsync(string bucket, IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public async Task PutObjectAsync(string s3BucketName, string path, string id, object dataObject, bool compress = true,
            S3StorageClass storageClass = null)
        {
            var s3Key = Path.Combine(path ?? "", $"{id}.json").Replace("\\", "/");

            var fileName = $"{LOCAL_DRIVE}\\{s3BucketName}\\{s3Key}";
            Directory.CreateDirectory(Path.GetDirectoryName(fileName) ?? string.Empty);

            using StreamWriter file = File.CreateText(fileName);

            var objJson = JsonConvert.SerializeObject(dataObject, Formatting.None);
            await file.WriteAsync(objJson.ToCharArray());
        }

        public async Task<T> ReadObjectDataAsync<T>(string s3BucketName, string path, string id, bool compress = true)
        {
            var s3Key = Path.Combine(path ?? "", $"{id}.json").Replace("\\", "/");
            var fileName = $"{LOCAL_DRIVE}\\{s3BucketName}\\{s3Key}";

            using var fileReader = new StreamReader(fileName);
            var jsonFileContent = await fileReader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<T>(jsonFileContent);
        }
    }
}
