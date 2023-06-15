namespace BlazorUpload.Shared.Model
{
    public class File : IEntityBase
    {
        public Guid Id { get; set; } = new Guid();

        public byte[]? FileToUpload { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
    }
}
