using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace wdhrtosis.Migrations.PersonImport
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<string>(nullable: true),
                    UniversalId = table.Column<string>(nullable: true),
                    AddressType = table.Column<string>(nullable: true),
                    AddressId = table.Column<string>(nullable: true),
                    AddressEffectiveDate = table.Column<DateTimeOffset>(nullable: true),
                    DefaultedBusinessAddress = table.Column<bool>(nullable: true),
                    PrimaryIndicator = table.Column<bool>(nullable: true),
                    PublicIndicator = table.Column<bool>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AddressLine3 = table.Column<string>(nullable: true),
                    AddressLine4 = table.Column<string>(nullable: true),
                    AddressLine5 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    StateProvince = table.Column<string>(nullable: true),
                    StateProvinceCode = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    ImportCreatedDate = table.Column<DateTime>(nullable: false),
                    ImportLastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ImportIsActiveRecord = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonContact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<string>(nullable: true),
                    UniversalId = table.Column<string>(nullable: true),
                    CountryAccessCode = table.Column<string>(nullable: true),
                    AreaCityCode = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    DeviceType = table.Column<string>(nullable: true),
                    PhoneType = table.Column<string>(nullable: true),
                    PhoneNumberFormatted = table.Column<string>(nullable: true),
                    PrimaryIndicator = table.Column<bool>(nullable: true),
                    PublicIndicator = table.Column<bool>(nullable: true),
                    ImportCreatedDate = table.Column<DateTime>(nullable: false),
                    ImportLastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ImportIsActiveRecord = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonContact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonEmailAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<string>(nullable: true),
                    UniversalId = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    EmailComment = table.Column<string>(nullable: true),
                    EmailType = table.Column<string>(nullable: true),
                    PrimaryIndicator = table.Column<bool>(nullable: true),
                    PublicIndicator = table.Column<bool>(nullable: true),
                    ImportCreatedDate = table.Column<DateTime>(nullable: false),
                    ImportLastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ImportIsActiveRecord = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonEmailAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonEmploymentPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<string>(nullable: true),
                    UniversalId = table.Column<string>(nullable: true),
                    PrimaryJobIndicator = table.Column<int>(nullable: false),
                    EmployeeType = table.Column<string>(nullable: true),
                    JobCode = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    JobClassificationGroup = table.Column<string>(nullable: true),
                    BusinessTitle = table.Column<string>(nullable: true),
                    JobActiveIndicator = table.Column<string>(nullable: true),
                    JobStatus = table.Column<string>(nullable: true),
                    JobBenefitsActiveIndicator = table.Column<string>(nullable: true),
                    Manager = table.Column<string>(nullable: true),
                    JobEffectiveDate = table.Column<DateTimeOffset>(nullable: true),
                    JobEndDate = table.Column<DateTimeOffset>(nullable: true),
                    BuildingNumber = table.Column<string>(nullable: true),
                    BuildingName = table.Column<string>(nullable: true),
                    RoomNumber = table.Column<string>(nullable: true),
                    FloorNumber = table.Column<string>(nullable: true),
                    TimeType = table.Column<string>(nullable: true),
                    JobFTE = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    PayGroup = table.Column<string>(nullable: true),
                    TerminationReason = table.Column<string>(nullable: true),
                    AnnualWorkPeriodStartDate = table.Column<DateTimeOffset>(nullable: true),
                    AnnualWorkPeriodEndDate = table.Column<DateTimeOffset>(nullable: true),
                    WorkPeriodPercentOfYear = table.Column<int>(nullable: false),
                    DisbursementPlanPeriodStartDate = table.Column<DateTimeOffset>(nullable: true),
                    DisbursementPlanPeriodEndDate = table.Column<DateTimeOffset>(nullable: true),
                    PayType = table.Column<string>(nullable: true),
                    ImportCorrelationId = table.Column<string>(nullable: true),
                    ImportCreatedDate = table.Column<DateTime>(nullable: false),
                    ImportLastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ImportIsActiveRecord = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonEmploymentPosition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonEmploymentProfile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<string>(nullable: true),
                    UniversalId = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Campus = table.Column<string>(nullable: true),
                    TotalFTE = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    ContinuousServiceDate = table.Column<DateTimeOffset>(nullable: true),
                    OptOutWURecord = table.Column<string>(nullable: true),
                    LocationHierarchy = table.Column<string>(nullable: true),
                    OptOutTotalDirectoryInfo = table.Column<string>(nullable: true),
                    OrigHireDate = table.Column<DateTimeOffset>(nullable: true),
                    TerminationDate = table.Column<DateTimeOffset>(nullable: true),
                    WorkerType = table.Column<string>(nullable: true),
                    CampusMailStop = table.Column<string>(nullable: true),
                    HireDate = table.Column<DateTimeOffset>(nullable: true),
                    RetireeIndicator = table.Column<int>(nullable: true),
                    ImportCorrelationId = table.Column<string>(nullable: true),
                    ImportCreatedDate = table.Column<DateTime>(nullable: false),
                    ImportLastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ImportIsActiveRecord = table.Column<bool>(nullable: false),
                    WorkerPrimaryPositionType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonEmploymentProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CorrelationId = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    ErrorMessage = table.Column<string>(nullable: true),
                    Success = table.Column<bool>(nullable: false),
                    LastRun = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonImportProcessSummary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastRun = table.Column<DateTime>(nullable: false),
                    ObjectProcessed = table.Column<string>(nullable: true),
                    ObjectCount = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonImportProcessSummary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonName",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<string>(nullable: true),
                    UniversalId = table.Column<string>(nullable: true),
                    NationalId = table.Column<string>(nullable: true),
                    Prefix = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Suffix = table.Column<string>(nullable: true),
                    PreferredPrefix = table.Column<string>(nullable: true),
                    PreferredFirstName = table.Column<string>(nullable: true),
                    PreferredMiddleName = table.Column<string>(nullable: true),
                    PreferredLastName = table.Column<string>(nullable: true),
                    PreferredSuffix = table.Column<string>(nullable: true),
                    ReportingName = table.Column<string>(nullable: true),
                    Pronunciation = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    DateofBirth = table.Column<DateTimeOffset>(nullable: true),
                    ImportCreatedDate = table.Column<DateTime>(nullable: false),
                    ImportLastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ImportIsActiveRecord = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonVisa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<string>(nullable: true),
                    UniversalId = table.Column<string>(nullable: true),
                    VisaNumber = table.Column<string>(nullable: true),
                    VisaType = table.Column<string>(nullable: true),
                    VisaCountry = table.Column<string>(nullable: true),
                    VisaPermitStatus = table.Column<string>(nullable: true),
                    VisaPermitDuration = table.Column<string>(nullable: true),
                    VisaPermitDurationType = table.Column<string>(nullable: true),
                    VisaIssuingAuthority = table.Column<string>(nullable: true),
                    VisaIssueDate = table.Column<DateTimeOffset>(nullable: true),
                    VisaExpiryDate = table.Column<DateTimeOffset>(nullable: true),
                    VisaVerificationDate = table.Column<DateTimeOffset>(nullable: true),
                    ImportCreatedDate = table.Column<DateTime>(nullable: false),
                    ImportLastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ImportIsActiveRecord = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonVisa", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PersonHistory",
                columns: new[] { "Id", "CorrelationId", "ErrorMessage", "LastRun", "Message", "Success" },
                values: new object[] { 1, "First 50 days", null, new DateTimeOffset(new DateTime(2020, 1, 8, 17, 9, 21, 583, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Initial Seed", true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonAddress");

            migrationBuilder.DropTable(
                name: "PersonContact");

            migrationBuilder.DropTable(
                name: "PersonEmailAddress");

            migrationBuilder.DropTable(
                name: "PersonEmploymentPosition");

            migrationBuilder.DropTable(
                name: "PersonEmploymentProfile");

            migrationBuilder.DropTable(
                name: "PersonHistory");

            migrationBuilder.DropTable(
                name: "PersonImportProcessSummary");

            migrationBuilder.DropTable(
                name: "PersonName");

            migrationBuilder.DropTable(
                name: "PersonVisa");
        }
    }
}
