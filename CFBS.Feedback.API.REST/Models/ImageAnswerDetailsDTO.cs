using System;
using System.Collections.Generic;
using System.Text;
using CFBS.Feedback.DAL.Enums;

namespace CFBS.Feedback.DAL.Entities
{
    public class ImageAnswerDetailsDTO : AnswerDetailsDTO
    {
        public string Name;
        public string Path;
        public object ImageFile; //TODO change to something more relevant.
    }
}
