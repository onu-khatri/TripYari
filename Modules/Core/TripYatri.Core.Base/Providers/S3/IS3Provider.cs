﻿using Amazon.S3;
using Amazon.S3.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TripYatri.Core.Base.Providers.S3
{
    public interface IS3Provider
    {
        Task PutObjectAsync(string s3BucketName, string path, string id, object dataObject, bool compress = true, S3StorageClass storageClass = null);
        Task<T> ReadObjectDataAsync<T>(string s3BucketName, string path, string id, bool compress = true);
        Task<DeleteObjectsResponse> DeleteObjectsAsync(string bucket, IEnumerable<string> keys);
    }
}