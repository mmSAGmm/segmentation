namespace Segmentation.DomainModels
{
    public class Segment
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Expression { get; set; } = string.Empty;
    }
}
