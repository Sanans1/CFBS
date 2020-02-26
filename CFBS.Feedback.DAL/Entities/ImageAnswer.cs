using System;
using System.Collections.Generic;
using System.Text;

namespace CFBS.Feedback.DAL.Entities
{
    public class ImageAnswer
    {
        public int AnswerID { get; set; }
        public int ImageID { get; set; }
        public string Text { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Image Image { get; set; }
    }
}
