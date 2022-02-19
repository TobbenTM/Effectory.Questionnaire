using Effectory.Questionnaire.Domain.Repositories;
using Effectory.Questionnaire.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Effectory.Questionnaire.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<QuestionnaireDbContext>(opts =>
        {
            opts.UseNpgsql(configuration.GetConnectionString(nameof(QuestionnaireDbContext)));
        });

        services.AddTransient<IQuestionsRepository, QuestionsRepository>();
        services.AddTransient<IAnswerRepository, AnswerRepository>();

        return services;
    }
}
