using Azure.Storage.Blobs;

namespace ConsoleUpload
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Upload de arquivos/fotos no Azure blob!");

            BlobConfig blobConfig = new BlobConfig();

            string defaultImage = (@"..\..\..\lua.jpg");

            byte[] bytes = File.ReadAllBytes(defaultImage);
            string base64Image = Convert.ToBase64String(bytes);

            string result = blobConfig.ImageUpload(base64Image);

            Console.WriteLine("Upload ok: " + result);
        }
    }
}