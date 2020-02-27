using System;
using System.Collections.Generic;
using System.Text;
using CFBS.Feedback.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CFBS.Feedback.DAL
{
    public class FeedbackContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<ActiveQuestion> ActiveQuestions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<TextAnswer> TextAnswers { get; set; }
        public DbSet<ImageAnswer> ImageAnswers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<SubmittedAnswer> SubmittedAnswers { get; set; }
        public DbSet<Location> Locations { get; set; }

        public FeedbackContext()
        {

        }

        public FeedbackContext(DbContextOptions<FeedbackContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Question>(questionEntities =>
            {
                questionEntities.HasKey(question => question.ID);
                questionEntities.Property(question => question.ID).ValueGeneratedOnAdd();

                questionEntities.Property(question => question.Text).IsRequired();
                questionEntities.Property(question => question.FeedbackType).IsRequired().HasConversion<string>();
                questionEntities.Property(question => question.AnswerType).IsRequired().HasConversion<string>();
                questionEntities.Property(question => question.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<Location>(locationEntities =>
            {
                locationEntities.HasKey(location => location.ID);
                locationEntities.Property(location => location.ID).ValueGeneratedOnAdd();

                locationEntities.Property(location => location.Name).IsRequired();
                locationEntities.Property(location => location.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<ActiveQuestion>(activeQuestionEntities =>
            {
                activeQuestionEntities.HasKey(activeQuestion => new { activeQuestion.LocationID, activeQuestion.QuestionID });

                activeQuestionEntities.HasOne(activeQuestion => activeQuestion.Question)
                                      .WithMany()
                                      .HasForeignKey(activeQuestion => activeQuestion.QuestionID);
                activeQuestionEntities.HasOne(activeQuestion => activeQuestion.Location)
                                      .WithMany()
                                      .HasForeignKey(activeQuestion => activeQuestion.LocationID);
            });

            modelBuilder.Entity<Image>(imageEntities =>
            {
                imageEntities.HasKey(image => image.ID);
                imageEntities.Property(image => image.ID).ValueGeneratedOnAdd();

                imageEntities.Property(image => image.Name).IsRequired();
                imageEntities.Property(image => image.Path).IsRequired();
                imageEntities.Property(image => image.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<Answer>(answerEntities =>
            {
                answerEntities.HasKey(answer => answer.ID);
                answerEntities.Property(answer => answer.ID).ValueGeneratedOnAdd();

                answerEntities.HasOne(answer => answer.Question)
                              .WithMany()
                              .HasForeignKey(answer => answer.QuestionID);

                answerEntities.Property(answer => answer.AnswerType).IsRequired().HasConversion<string>();
                answerEntities.Property(answer => answer.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<ImageAnswer>(imageAnswerEntities =>
            {
                imageAnswerEntities.HasKey(imageAnswer => imageAnswer.AnswerID);

                imageAnswerEntities.HasOne(imageAnswer => imageAnswer.Answer)
                                   .WithOne()
                                   .HasForeignKey<ImageAnswer>(imageAnswer => imageAnswer.AnswerID);
                imageAnswerEntities.HasOne(imageAnswer => imageAnswer.Image)
                                   .WithMany()
                                   .HasForeignKey(imageAnswer => imageAnswer.ImageID);

                imageAnswerEntities.Property(imageAnswer => imageAnswer.Text).IsRequired();
            });

            modelBuilder.Entity<TextAnswer>(textAnswerEntities =>
            {
                textAnswerEntities.HasKey(textAnswer => textAnswer.AnswerID);

                textAnswerEntities.HasOne(textAnswer => textAnswer.Answer)
                    .WithOne()
                    .HasForeignKey<TextAnswer>(textAnswer => textAnswer.AnswerID);

                textAnswerEntities.Property(textAnswer => textAnswer.Text).IsRequired();
            });

            modelBuilder.Entity<SubmittedAnswer>(submittedAnswerEntities =>
            {
                submittedAnswerEntities.HasKey(submittedAnswer => submittedAnswer.ID);
                submittedAnswerEntities.Property(submittedAnswer => submittedAnswer.ID).ValueGeneratedOnAdd();

                submittedAnswerEntities.HasOne(submittedAnswer => submittedAnswer.Answer)
                                       .WithMany()
                                       .HasForeignKey(submittedAnswer => submittedAnswer.AnswerID);
                submittedAnswerEntities.HasOne(submittedAnswer => submittedAnswer.Location)
                                       .WithMany()
                                       .HasForeignKey(submittedAnswer => submittedAnswer.LocationID);

                submittedAnswerEntities.Property(submittedAnswer => submittedAnswer.FeedbackSessionNumber).IsRequired();
                submittedAnswerEntities.Property(submittedAnswer => submittedAnswer.CreatedAt).IsRequired();
            });
        }
    }
}
