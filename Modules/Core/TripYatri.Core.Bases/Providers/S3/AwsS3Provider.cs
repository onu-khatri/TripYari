using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using TripYatri.Core.Base.Providers.Logger;
using TripYatri.Core.Base.Providers.Metrics;

namespace TripYatri.Core.Base.Providers.S3
{
    public class AwsS3Provider : IS3Provider
    {
        private readonly RuntimeEnvironment _runtimeEnvironment;
        private readonly IAmazonS3 _s3Client;
        private readonly IMetricsProvider _metricsProvider;
        private readonly ILogger _logger;

        public AwsS3Provider(RuntimeEnvironment runtimeEnvironment, IAmazonS3 s3Client, IMetricsProvider metricsProvider, ILogger logger)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _s3Client = s3Client;
            _metricsProvider = metricsProvider;
            _logger = logger;
        }

        public async Task PutObjectAsync(string s3BucketName, string path, string id, object dataObject, bool compress = true,
            S3StorageClass storageClass = null)
        {
            using (_metricsProvider.BeginTiming(this))
            {
                var s3Key = Path.Combine(path ?? "", $"{id}.json").Replace("\\", "/");

                using (var writeMemoryStream = new MemoryStream())
                {
                    try
                    {
                        var objJson = JsonConvert.SerializeObject(dataObject, Formatting.None);

                        if (compress)
                        {
                            s3Key += ".gz";
                            using var gzipStream = new GZipStream(writeMemoryStream, CompressionLevel.Fastest);
                            using var streamWriter = new StreamWriter(gzipStream);
                            await streamWriter.WriteAsync(objJson);
                        }
                        else
                        {
                            using var streamWriter = new StreamWriter(writeMemoryStream);
                            await streamWriter.WriteAsync(objJson);
                        }

                        using var readMemoryStream = new MemoryStream(writeMemoryStream.ToArray());
                        var putObjectRequest = new PutObjectRequest()
                        {
                            BucketName = s3BucketName,
                            Key = s3Key,
                            InputStream = readMemoryStream,
                            StorageClass = storageClass ?? S3StorageClass.Standard
                        };

                        _logger.LogInfo($"Uploading Object File s3://{s3BucketName}/{s3Key}");
                        await _s3Client.PutObjectAsync(putObjectRequest);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Failed to upload S3 object s3://{s3BucketName}/{s3Key}", ex);
                        throw;
                    }
                }
            }
        }

        public async Task<T> ReadObjectDataAsync<T>(string s3BucketName, string path, string id, bool compress = true)
        {
            var result = "";
            using (_metricsProvider.BeginTiming(this))
            {
                var s3Key = Path.Combine(path ?? "", $"({id}.json").Replace("\\", "/");
                var request = new GetObjectRequest
                {
                    BucketName = s3BucketName,
                    Key = s3Key
                };

                using (var response = await _s3Client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                {
                    if (compress)
                    {
                        using var decompressionStream =
                            new GZipStream(responseStream, CompressionMode.Decompress);
                        var sb = new StringBuilder();
                        await decompressionStream.CopyToAsync(decompressionStream);
                    }
                    else
                    {
                        using var reader = new StreamReader(responseStream);
                        result = await reader.ReadToEndAsync();
                    }
                }
                return JsonConvert.DeserializeObject<T>(result);
            }
        }
        public  async Task<DeleteObjectsResponse> DeleteObjectsAsync(string bucket, IEnumerable<string> keys)
        {
            using (_metricsProvider.BeginTiming(this))
            {
                

                    var req = new DeleteObjectsRequest()
                    {
                        BucketName = bucket
                    };

                    foreach (var key in keys)
                        req.AddKey(key);

                  return  await _s3Client.DeleteObjectsAsync(req);
               
                
            }
        }
    }
}
