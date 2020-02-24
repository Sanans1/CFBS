namespace CFBS.Feedback.API.REST.Models
{
    public class ImageAnswerDetailsDTO : AnswerDetailsDTO
    {
        public string Name;
        public string Path;
        public object ImageFile; //TODO change to something more relevant.
    }
}
