namespace Shop.Models.RealEstate
{
    public class RealEstateDetailsViewModel
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string? Location { get; set; }
        public int? RoomNumber { get; set; }
        public string? BuildingType { get; set; }
        public List<RealEstateImagesViewModel> Image { get; set; }
            = new List<RealEstateImagesViewModel>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
