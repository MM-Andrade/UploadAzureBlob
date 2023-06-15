using Azure.Storage.Blobs;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ConsoleUpload
{
    public class BlobConfig
    {
        //string de conexão do blob storage
        public string connectionString = "";
        //nome do container
        public string containerName = "";


        public string ImageUpload(string base64Image)
        {
            var fileName = Guid.NewGuid().ToString() + ".jpg";

            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, "");

            byte[] imageBytes = Convert.FromBase64String(data);


            var blobClient = new BlobClient(connectionString, containerName, fileName);

            using(var stream = new MemoryStream(imageBytes))
            {
                blobClient.Upload(stream);
            }

            return blobClient.Uri.AbsoluteUri;

        }
    }
}
