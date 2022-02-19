using System;
using System.Linq;
using Effectory.Questionnaire.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Effectory.Questionnaire.Tests;

internal class QuestionnaireWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _fixtureId;

    public QuestionnaireWebApplicationFactory()
    {
        _fixtureId = Guid.NewGuid().ToString();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // We want to work with an in-memory database for all runs,
            // so let's remove the old options and add the in-memory one
            var dbDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<QuestionnaireDbContext>));
            services.Remove(dbDescriptor);

            services.AddDbContext<QuestionnaireDbContext>(opts =>
            {
                opts.UseInMemoryDatabase(_fixtureId);
            });
        });

        return base.CreateHost(builder);
    }
}
