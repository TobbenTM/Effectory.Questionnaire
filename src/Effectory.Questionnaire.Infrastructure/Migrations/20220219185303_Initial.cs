using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Effectory.Questionnaire.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questionnaires",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionnaires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RootQuestionnaireSubject",
                columns: table => new
                {
                    QuestionnairesId = table.Column<long>(type: "bigint", nullable: false),
                    SubjectsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RootQuestionnaireSubject", x => new { x.QuestionnairesId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_RootQuestionnaireSubject_Questionnaires_QuestionnairesId",
                        column: x => x.QuestionnairesId,
                        principalTable: "Questionnaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RootQuestionnaireSubject_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswerOptions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswerOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    OptionId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    AnsweredByUserId = table.Column<long>(type: "bigint", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_QuestionAnswerOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "QuestionAnswerOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Questionnaires",
                column: "Id",
                value: 1000L);

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "DisplayOrder", "Text" },
                values: new object[,]
                {
                    { 2605515L, 0, "{\"nl-NL\":\"Mijn werk\",\"en-US\":\"My work\"}" },
                    { 2605516L, 1, "{\"nl-NL\":\"Mijn rol\",\"en-US\":\"My role\"}" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "DisplayOrder", "SubjectId", "Text" },
                values: new object[,]
                {
                    { 3807638L, 0, 2605515L, "{\"nl-NL\":\"Ik ben blij met mijn werk\",\"en-US\":\"I am happy with my work\"}" },
                    { 3807643L, 1, 2605515L, "{\"en-US\":\"I enjoy doing my work\",\"nl-NL\":\"Ik doe mijn werk met plezier\"}" },
                    { 3807644L, 4, 2605515L, "{\"nl-NL\":\"Ik heb het gevoel dat ik met mijn werk een bijdrage lever\",\"en-US\":\"I feel I contribute meaningfully through my work\"}" },
                    { 3807701L, 3, 2605515L, "{\"nl-NL\":\"Ik heb voldoende verantwoordelijkheid in mijn werk\",\"en-US\":\"I have sufficient responsibilities in my work\"}" },
                    { 3810105L, 2, 2605516L, "{\"nl-NL\":\"Als je deze organisatie één tip mag geven, wat zou dat dan zijn?\",\"en-US\":\"If you could give this organisation one tip, what would it be?\"}" },
                    { 3851843L, 0, 2605516L, "{\"nl-NL\":\"Ik weet hoe mijn werk bijdraagt aan de visie en doelen van Organisatie\",\"en-US\":\"It is clear to me how my work contributes to the organisation's strategy\"}" },
                    { 3851855L, 2, 2605515L, "{\"nl-NL\":\"Ik vind mijn werk uitdagend\",\"en-US\":\"My work is enjoyably challenging\"}" },
                    { 3851856L, 1, 2605516L, "{\"nl-NL\":\"Ik geef in de dagelijkse praktijk feedback aan de mensen met wie ik samenwerk\",\"en-US\":\"I give feedback to the people I work with in the daily practice of work\"}" }
                });

            migrationBuilder.InsertData(
                table: "QuestionAnswerOptions",
                columns: new[] { "Id", "DisplayOrder", "QuestionId", "Text" },
                values: new object[,]
                {
                    { -1L, 0, 3810105L, null },
                    { 17969120L, 4, 3807638L, "{\"nl-NL\":\"Helemaal mee eens\",\"en-US\":\"Strongly agree\"}" },
                    { 17969121L, 3, 3807638L, "{\"nl-NL\":\"Mee eens\",\"en-US\":\"Agree\"}" },
                    { 17969122L, 2, 3807638L, "{\"nl-NL\":\"Niet mee eens/ niet mee oneens\",\"en-US\":\"Neither agree nor disagree\"}" },
                    { 17969123L, 1, 3807638L, "{\"nl-NL\":\"Mee oneens\",\"en-US\":\"Disagree\"}" },
                    { 17969124L, 0, 3807638L, "{\"nl-NL\":\"Helemaal mee oneens\",\"en-US\":\"Strongly disagree\"}" },
                    { 17969145L, 4, 3807643L, "{\"nl-NL\":\"Helemaal mee eens\",\"en-US\":\"Strongly agree\"}" },
                    { 17969146L, 3, 3807643L, "{\"nl-NL\":\"Mee eens\",\"en-US\":\"Agree\"}" },
                    { 17969147L, 2, 3807643L, "{\"nl-NL\":\"Niet mee eens/ niet mee oneens\",\"en-US\":\"Neither agree nor disagree\"}" },
                    { 17969148L, 1, 3807643L, "{\"nl-NL\":\"Mee oneens\",\"en-US\":\"Disagree\"}" },
                    { 17969149L, 0, 3807643L, "{\"nl-NL\":\"Helemaal mee oneens\",\"en-US\":\"Strongly disagree\"}" },
                    { 17969150L, 4, 3807644L, "{\"nl-NL\":\"Helemaal mee eens\",\"en-US\":\"Strongly agree\"}" },
                    { 17969151L, 3, 3807644L, "{\"nl-NL\":\"Mee eens\",\"en-US\":\"Agree\"}" },
                    { 17969152L, 2, 3807644L, "{\"nl-NL\":\"Niet mee eens/ niet mee oneens\",\"en-US\":\"Neither agree nor disagree\"}" },
                    { 17969153L, 1, 3807644L, "{\"nl-NL\":\"Mee oneens\",\"en-US\":\"Disagree\"}" },
                    { 17969154L, 0, 3807644L, "{\"nl-NL\":\"Helemaal mee oneens\",\"en-US\":\"Strongly disagree\"}" },
                    { 17969411L, 4, 3807701L, "{\"nl-NL\":\"Helemaal mee eens\",\"en-US\":\"Strongly agree\"}" },
                    { 17969412L, 3, 3807701L, "{\"nl-NL\":\"Mee eens\",\"en-US\":\"Agree\"}" },
                    { 17969413L, 2, 3807701L, "{\"nl-NL\":\"Niet mee eens/ niet mee oneens\",\"en-US\":\"Neither agree nor disagree\"}" },
                    { 17969414L, 1, 3807701L, "{\"nl-NL\":\"Mee oneens\",\"en-US\":\"Disagree\"}" },
                    { 17969415L, 0, 3807701L, "{\"nl-NL\":\"Helemaal mee oneens\",\"en-US\":\"Strongly disagree\"}" },
                    { 18166287L, 4, 3851843L, "{\"nl-NL\":\"Helemaal mee eens\",\"en-US\":\"Strongly agree\"}" },
                    { 18166288L, 3, 3851843L, "{\"nl-NL\":\"Mee eens\",\"en-US\":\"Agree\"}" },
                    { 18166289L, 2, 3851843L, "{\"nl-NL\":\"Niet mee eens/ niet mee oneens\",\"en-US\":\"Neither agree nor disagree\"}" },
                    { 18166290L, 1, 3851843L, "{\"nl-NL\":\"Mee oneens\",\"en-US\":\"Disagree\"}" },
                    { 18166291L, 0, 3851843L, "{\"nl-NL\":\"Helemaal mee oneens\",\"en-US\":\"Strongly disagree\"}" },
                    { 18166385L, 4, 3851855L, "{\"nl-NL\":\"Helemaal mee eens\",\"en-US\":\"Strongly agree\"}" },
                    { 18166386L, 3, 3851855L, "{\"nl-NL\":\"Mee eens\",\"en-US\":\"Agree\"}" },
                    { 18166387L, 2, 3851855L, "{\"nl-NL\":\"Niet mee eens/ niet mee oneens\",\"en-US\":\"Neither agree nor disagree\"}" },
                    { 18166388L, 1, 3851855L, "{\"nl-NL\":\"Mee oneens\",\"en-US\":\"Disagree\"}" },
                    { 18166389L, 0, 3851855L, "{\"nl-NL\":\"Helemaal mee oneens\",\"en-US\":\"Strongly disagree\"}" },
                    { 18166390L, 4, 3851856L, "{\"nl-NL\":\"Helemaal mee eens\",\"en-US\":\"Strongly agree\"}" },
                    { 18166391L, 3, 3851856L, "{\"nl-NL\":\"Mee eens\",\"en-US\":\"Agree\"}" },
                    { 18166392L, 2, 3851856L, "{\"nl-NL\":\"Niet mee eens/ niet mee oneens\",\"en-US\":\"Neither agree nor disagree\"}" },
                    { 18166393L, 1, 3851856L, "{\"nl-NL\":\"Mee oneens\",\"en-US\":\"Disagree\"}" },
                    { 18166394L, 0, 3851856L, "{\"nl-NL\":\"Helemaal mee oneens\",\"en-US\":\"Strongly disagree\"}" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_OptionId",
                table: "Answers",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswerOptions_QuestionId",
                table: "QuestionAnswerOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SubjectId",
                table: "Questions",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RootQuestionnaireSubject_SubjectsId",
                table: "RootQuestionnaireSubject",
                column: "SubjectsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "RootQuestionnaireSubject");

            migrationBuilder.DropTable(
                name: "QuestionAnswerOptions");

            migrationBuilder.DropTable(
                name: "Questionnaires");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
