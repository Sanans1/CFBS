using System;
using System.Collections.Generic;
using System.Text;

namespace CFBS.Feedback.DAL.Entities
{
    public class TextAnswer
    {
        public int AnswerID { get; set; }
        public string Text { get; set; }

        public virtual Answer Answer { get; set; }
    }
}
