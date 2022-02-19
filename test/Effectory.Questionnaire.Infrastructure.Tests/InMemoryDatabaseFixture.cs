using System;
using Microsoft.EntityFrameworkCore;

namespace Effectory.Questionnaire.Infrastructure.Tests;

// Note: this doesn't technically need to be a class fixture, as
// it doesn't actually share state across test suites, but I
// just wanted to access it in a consistent way. A better test
// suite base class is probably the better option.
public class InMemoryDatabaseFixture
{
    public DbContextOptions<QuestionnaireDbContext> CreateDbContextOptions()
        => new DbContextOptionsBuilder<QuestionnaireDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

    public QuestionnaireDbContext CreateDatabase(DbContextOptions<QuestionnaireDbContext> opts)
        => new(opts);
}
