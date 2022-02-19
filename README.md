# Questionnaire API

Experiments in creating Questionnaire APIs.

## Assumptions and "shortcuts" made

* Types/Categories of questionnaires, subjects, questions and answer options has been omitted
    - To simplify the initial domain model, I skipped these, they seem to be consistently the same anyways (with one exception)
* Min/max/average on these questions that from the initial model doesn't seem to necessarily have a numeric range has been approximated using the available options
    - Had I understood the domain a bit better I might've modelled it to better represent numeric ranges, but didn't have time for that now
    - I interpreted minimum to mean the lowest answer given based on the order number of the option, likewise with the maximum

## Scalability and alternative approaches

As is, the statistics part of the application is calculating all numbers on-demand.
In the long term, this is probably going to be the least scalable part of the process.

Other ways to deal with this can include asynchronously processing new answers, continuously updating a materialized view of current statistics.
This is one place where event-driven architectures can be especially helpful, as we could offload the running calculations to other services.
Of course, this does make the application as a whole eventually consistent, which may be undesirable depending on requirements.

The API layer itself is stateless, and so it should be fairly scalable, either by migrating to a serverless approach, or by scaling out horizontally using containers, VMs, or App Services.

## Requirements

To run locally:

* .Net >= 6.0
* PostgreSQL running on localhost
    - Can be started with `docker compose up -d`
    - PostgreSQL available on localhost:15432
    - PGAdmin available on localhost:15050, credentials in `docker-compose.yml`

## Running locally

Start up the "Effectory.Questionnaire.API" using `dotnet run` in that directory, or using Visual Studio/Rider.

On first boot, the API will migrate the database, adding necessary tables and seed data.

## Testing

Tests can be run with `dotnet test` in the root directory.

All tests are using an in-memory database, so you don't need to run the infrastructure to run the tests.
