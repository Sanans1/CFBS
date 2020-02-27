namespace CFBS.Feedback.API.REST.Models
{
    public class ImageAnswerDetailsDTO : AnswerDetailsDTO
    {
        public int ImageID { get; set; }
        public ImageDTO Image { get; set; }
        public object ImageFile { get; set; } //TODO change to something more relevant.
    }
}
