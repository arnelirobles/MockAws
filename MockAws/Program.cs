using Amazon.S3;
using Amazon.S3.Model;
using Moq;

namespace MockAws
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mockS3Client = new Mock<IAmazonS3>();

            var s3Objects = new List<S3Object>
            {
                new S3Object { Key = "image1.jpg" },
                new S3Object { Key = "image2.jpg" }
            };

            var listObjectsResponse = new ListObjectsResponse { S3Objects = s3Objects };

            mockS3Client
                .Setup(client => client.ListObjectsAsync(It.IsAny<ListObjectsRequest>(), default))
                .ReturnsAsync(listObjectsResponse);

            mockS3Client
                .Setup(client => client.GetPreSignedURL(It.IsAny<GetPreSignedUrlRequest>()))
                .Returns("http://fake-s3-url.com/image.jpg");

            var service = new TestService(mockS3Client.Object);
            var images = service.ListImages("prefix").GetAwaiter().GetResult();

            foreach (var image in images)
            {
                Console.WriteLine($"ImageURL: {image}");
            }
        }
    }
}
