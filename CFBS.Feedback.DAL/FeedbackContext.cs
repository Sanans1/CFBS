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
        public DbSet<Image> Image { get; set; }
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

                
            });
        }
    }
}
