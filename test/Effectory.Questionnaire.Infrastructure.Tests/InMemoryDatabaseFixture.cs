using System;
using Microsoft.EntityFrameworkCore;

namespace Effectory.Questionnaire.Infrastructure.Tests;

public class InMemoryDatabaseFixture
{
    public DbContextOptions<QuestionnaireDbContext> CreateDbContextOptions()
        => new DbContextOptionsBuilder<QuestionnaireDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

    public QuestionnaireDbContext CreateDatabase(DbContextOptions<QuestionnaireDbContext> opts)
        => new QuestionnaireDbContext(opts);
}
