using Effectory.Questionnaire.Domain.Entities;
using Effectory.Questionnaire.Domain.Support;
using Effectory.Questionnaire.Domain.Support.AnswerContent;
using Effectory.Questionnaire.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        // Some small code smells here:
        // 1. This "hacky" way of storing complex objects in a relational
        //    database might indicate that a noSQL document store is a
        //    better approach, but in the essence of simplicity, I'm doing
        //    it the hacky way for now.
        // 2. I generally prefer using System.Text.Json nowadays, as that's
        //    where most .Net libraries are standardising towards, but there
        //    is one thing missing from that which Newtonsoft.Json does simpler
        //    which is polymorphic ser/de. While not impossible in System.Text.Json,
        //    I'm opting for the minimal approach here.
        // 3. The relationships became a bit complicated, had I had time,
        //    I would love to refactor this to be have a better relationship graph.

        modelBuilder.Entity<Answer>()
            .Property(a => a.Content)
            .HasConversion(
                v => JsonConvert.SerializeObject(v,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All}),
                v => JsonConvert.DeserializeObject<IAnswerContent?>(v,
                    new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All}));

        // modelBuilder.Entity<Answer>()
        //     .HasOne(a => a.Option)
        //     .WithMany(o => o.Answers)
        //     .HasForeignKey(a => new {a.QuestionId, a.OptionId});

        var localizedTextComparer = new ValueComparer<LocalizedText>(
            (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => new LocalizedText(c.ToDictionary(kv => kv.Key, kv => kv.Value)));

        // modelBuilder.Entity<QuestionAnswerOption>()
        //     .HasKey(o => new {AnswerId = o.OptionId, o.QuestionId });
        modelBuilder.Entity<QuestionAnswerOption>()
            .Property(a => a.Text)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<LocalizedText>(v),
                localizedTextComparer);

        modelBuilder.Entity<Subject>()
            .Property(a => a.Text)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<LocalizedText>(v),
                localizedTextComparer);

        modelBuilder.Entity<Question>()
            .Property(a => a.Text)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<LocalizedText>(v),
                localizedTextComparer);

        // Add seed data based on initial JSON payload given
        var seed = SeedData.Load().Result;

        modelBuilder.Entity<RootQuestionnaire>()
            .HasData(seed.Root);
        modelBuilder.Entity<Subject>()
            .HasData(seed.Subjects);
        modelBuilder.Entity<Question>()
            .HasData(seed.Questions);
        modelBuilder.Entity<QuestionAnswerOption>()
            .HasData(seed.QuestionAnswerOptions);
    }
}
