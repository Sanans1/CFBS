namespace CFBS.Feedback.API.REST.Models
{
    public class ImageAnswerDetailsDTO : AnswerDetailsDTO
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public object ImageFile { get; set; } //TODO change to something more relevant.
    }
}
