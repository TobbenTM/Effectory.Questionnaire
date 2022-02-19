using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Support;
using Effectory.Questionnaire.Domain.Support.AnswerContent;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Effectory.Questionnaire.Infrastructure;

public class QuestionnaireDbContext : DbContext
{
    public DbSet<RootQuestionnaire> Questionnaires => Set<RootQuestionnaire>();

    public DbSet<Subject> Subjects => Set<Subject>();

    public DbSet<Question> Questions => Set<Question>();

    public DbSet<QuestionAnswerOption> QuestionAnswerOptions => Set<QuestionAnswerOption>();

    public DbSet<Answer> Answers => Set<Answer>();

    public QuestionnaireDbContext(DbContextOptions<QuestionnaireDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Two small code smells here:
        // 1. This "hacky" way of storing complex objects in a relational
        //    database might indicate that a noSQL document store is a
        //    better approach, but in the essence of simplicity, I'm doing
        //    it the hacky way for now.
        // 2. I generally prefer using System.Text.Json nowadays, as that's
        //    where most .Net libraries are standardising towards, but there
        //    is one thing missing from that which Newtonsoft.Json does simpler
        //    which is polymorphic ser/de. While not impossible in System.Text.Json,
        //    I'm opting for the minimal approach here.

        modelBuilder.Entity<Answer>()
            .Property(a => a.Content)
            .HasConversion(
                v => JsonConvert.SerializeObject(v,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All}),
                v => JsonConvert.DeserializeObject<IAnswerContent?>(v,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All}));

        modelBuilder.Entity<QuestionAnswerOption>()
            .Property(a => a.Text)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<LocalizedText>(v));

        modelBuilder.Entity<Subject>()
            .Property(a => a.Text)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<LocalizedText>(v));

        modelBuilder.Entity<Question>()
            .Property(a => a.Text)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<LocalizedText>(v));
    }
}
