using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace MockAws
{
    public class TestService
    {
        private readonly IAmazonS3 _s3Client;

        public TestService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<List<string>> ListImages(string prefix)
        {
            var imageUrls = new List<string>();

            var request = new ListObjectsRequest
            {
                BucketName = "your-bucket-name",
                Prefix = prefix
            };

            var response = await _s3Client.ListObjectsAsync(request);

            foreach (var obj in response.S3Objects)
            {
                var urlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = "your-bucket-name",
                    Key = obj.Key,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                var url = _s3Client.GetPreSignedURL(urlRequest);
                imageUrls.Add(url);

                if (imageUrls.Count == 2)
                {
                    break;
                }
            }

            return imageUrls;
        }
    }
}
