namespace BlazorUpload.Shared.Model
{
    public class FileToUpload : IEntityBase
    {
        public Guid Id { get; set; } = new Guid();

        public byte[]? FileName { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
    }
}
