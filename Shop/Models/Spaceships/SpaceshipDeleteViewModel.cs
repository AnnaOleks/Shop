namespace Shop.Models.Spaceships
{
    public class SpaceshipDeleteViewModel
    {
        public Guid? ID { get; set; }
        public string? Name { get; set; }
        public string? TypeName { get; set; }
        public DateTime? BuiltDate { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }
        public int? Passengers { get; set; }
        public int? InnerVolume { get; set; }
        public List<ImagesViewModel> Image { get; set; }
            = new List<ImagesViewModel>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
