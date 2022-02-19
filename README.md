# Questionnaire API

Experiments in creating Questionnaire APIs.

## Assumptions and "shortcuts" made

* Types of questionnaires, subjects, questions and answer options has been omitted
    - To simplify the initial domain model, I skipped these, they seem to be consistently the same anyways

## Requirements

To run locally:

* .Net >= 6.0

Or run using Docker:

`docker compose up`

## Running locally

Start up the "Effectory.Questionnaire.API" using `dotnet run` in that directory, or using Visual Studio/Rider.

## Testing

Tests can be run with `dotnet test` in the root directory.
