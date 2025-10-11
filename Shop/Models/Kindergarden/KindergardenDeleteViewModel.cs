namespace Shop.Models.Kindergarden
{
    public class KindergardenDeleteViewModel
    {
        public Guid? Id { get; set; }
        public string? GroupName { get; set; }
        public string? KindergardenName { get; set; }
        public int? ChildrenCount { get; set; }
        public string? TeacherName { get; set; }
        public List<KindergardenImagesViewModel> Image { get; set; }
            = new List<KindergardenImagesViewModel>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
