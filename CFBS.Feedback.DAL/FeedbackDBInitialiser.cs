using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CFBS.Feedback.DAL.Entities;
using CFBS.Feedback.DAL.Enums;
using Microsoft.EntityFrameworkCore.Internal;

namespace CFBS.Feedback.DAL
{
    public static class FeedbackDBInitialiser
    {
        public static void SeedTestData(FeedbackContext context,
                                      IServiceProvider services)
        {
            if (context.Questions.Any())
            {
                return;
            }

            List<Image> images = new List<Image>
            {
                new Image
                {
                    ID = 1,
                    Name = "HappyEmoji",
                    Path = "Path/To/Happy/Emoji",
                    CreatedAt = DateTime.Now
                },
                new Image
                {
                    ID = 2,
                    Name = "SadEmoji",
                    Path = "Path/To/Sad/Emoji",
                    CreatedAt = DateTime.Now
                }
            };

            context.Images.AddRange(images);

            List<Location> locations = new List<Location>
            {
                new Location
                {
                    ID = 1,
                    Name = "Stockton-On-Tees",
                    CreatedAt = DateTime.Now
                },
                new Location
                {
                    ID = 2,
                    Name = "Darlington",
                    CreatedAt = DateTime.Now
                },
                new Location
                {
                    ID = 3,
                    Name = "Thornaby",
                    CreatedAt = DateTime.Now
                }
            };

            context.Locations.AddRange(locations);

            List<Question> questions = new List<Question>
            {
                new Question
                {
                    ID = 1,
                    Text = "How was your experience today?",
                    FeedbackType = FeedbackType.Experience,
                    AnswerType = AnswerType.Image,
                    CreatedAt = DateTime.Now
                },
                new Question
                {
                    ID = 2,
                    Text = "Any suggestions on how to improve your experience?",
                    FeedbackType = FeedbackType.Experience,
                    AnswerType = AnswerType.Text,
                    CreatedAt = DateTime.Now
                },
                new Question
                {
                    ID = 3,
                    Text = "How was your experience with our feedback system today?",
                    FeedbackType = FeedbackType.System,
                    AnswerType = AnswerType.Image,
                    CreatedAt = DateTime.Now
                }
            };

            context.Questions.AddRange(questions);

            List<ActiveQuestion> activeQuestions = new List<ActiveQuestion>
            {
                new ActiveQuestion
                {
                    QuestionID = 1,
                    LocationID = 1
                },
                new ActiveQuestion
                {
                    QuestionID = 2,
                    LocationID = 1
                },
                new ActiveQuestion
                {
                    QuestionID = 3,
                    LocationID = 1
                },
                new ActiveQuestion
                {
                    QuestionID = 1,
                    LocationID = 2
                },
                new ActiveQuestion
                {
                    QuestionID = 3,
                    LocationID = 2
                }
            };

            context.ActiveQuestions.AddRange(activeQuestions);

            List<Answer> answers = new List<Answer>
            {
                new Answer
                {
                    ID = 1,
                    QuestionID = 1,
                    AnswerType = AnswerType.Image,
                    CreatedAt = DateTime.Now
                },
                new Answer
                {
                    ID = 2,
                    QuestionID = 1,
                    AnswerType = AnswerType.Image,
                    CreatedAt = DateTime.Now
                },
                new Answer
                {
                    ID = 3,
                    QuestionID = 2,
                    AnswerType = AnswerType.Text,
                    CreatedAt = DateTime.Now
                },
                new Answer
                {
                    ID = 4,
                    QuestionID = 3,
                    AnswerType = AnswerType.Image,
                    CreatedAt = DateTime.Now
                },
                new Answer
                {
                    ID = 5,
                    QuestionID = 3,
                    AnswerType = AnswerType.Image,
                    CreatedAt = DateTime.Now
                },
            };

            context.Answers.AddRange(answers);

            List<ImageAnswer> imageAnswers = new List<ImageAnswer>
            {
                new ImageAnswer
                {
                    AnswerID = 1,
                    ImageID = 1,
                    Text = "Happy",
                    IsActive = true
                },
                new ImageAnswer
                {
                    AnswerID = 2,
                    ImageID = 2,
                    Text = "Sad",
                    IsActive = true
                },
                new ImageAnswer
                {
                    AnswerID = 4,
                    ImageID = 1,
                    Text = "Happy",
                    IsActive = true
                },
                new ImageAnswer
                {
                    AnswerID = 5,
                    ImageID = 2,
                    Text = "Sad",
                    IsActive = true
                },
            };

            context.ImageAnswers.AddRange(imageAnswers);

            List<TextAnswer> textAnswers = new List<TextAnswer>
            {
                new TextAnswer
                {
                    AnswerID = 3,
                    Text = "You could give out stickers!"
                }
            };

            context.TextAnswers.AddRange(textAnswers);

            List<SubmittedAnswer> submittedAnswers = new List<SubmittedAnswer>
            {
                new SubmittedAnswer
                {
                    ID = 1,
                    AnswerID = 1,
                    LocationID = 1,
                    FeedbackSessionNumber = 1,
                    CreatedAt = DateTime.Now
                },
                new SubmittedAnswer
                {
                    ID = 2,
                    AnswerID = 3,
                    LocationID = 1,
                    FeedbackSessionNumber = 1,
                    CreatedAt = DateTime.Now
                },
                new SubmittedAnswer
                {
                    ID = 3,
                    AnswerID = 5,
                    LocationID = 1,
                    FeedbackSessionNumber = 1,
                    CreatedAt = DateTime.Now
                },
                new SubmittedAnswer
                {
                    ID = 4,
                    AnswerID = 2,
                    LocationID = 2,
                    FeedbackSessionNumber = 2,
                    CreatedAt = DateTime.Now
                },
                new SubmittedAnswer
                {
                    ID = 5,
                    AnswerID = 4,
                    LocationID = 2,
                    FeedbackSessionNumber = 2,
                    CreatedAt = DateTime.Now
                },
            };

            context.SubmittedAnswers.AddRange(submittedAnswers);

            context.SaveChanges();
        }
    }
}
