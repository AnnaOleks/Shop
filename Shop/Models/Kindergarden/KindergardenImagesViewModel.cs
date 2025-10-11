namespace Shop.Models.Kindergarden
{
    public class KindergardenImagesViewModel
    {
        public Guid Id { get; set; }
        public string? ImageTitle { get; set; }
        public byte[]? ImageData { get; set; }
        public string? Image { get; set; }
        public Guid? KindergardenId { get; set; }
    }
}
