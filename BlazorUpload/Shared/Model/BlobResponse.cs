namespace BlazorUpload.Shared.Model
{
    public class BlobResponse
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobDTO Blob { get; set; }

        public BlobResponse()
        {
            Blob = new BlobDTO();
        }
    }
}
