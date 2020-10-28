using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace wdhrtosis.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WD_Certification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Certification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_Compensation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TotalBasePayAnnualizedAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    EffectiveDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Compensation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_Contact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_EmploymentProfile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(nullable: true),
                    Campus = table.Column<string>(nullable: true),
                    TotalFTE = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
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
                    WorkerPrimaryPositionType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_EmploymentProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_EthnicCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EthnicCategory = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_EthnicCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_LifeEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BirthDate = table.Column<DateTimeOffset>(nullable: true),
                    BirthCountry = table.Column<string>(nullable: true),
                    BirthCountryCode = table.Column<string>(nullable: true),
                    BirthStateProvince = table.Column<string>(nullable: true),
                    BirthStateProvinceCode = table.Column<string>(nullable: true),
                    BirthPlace = table.Column<string>(nullable: true),
                    DeathDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_LifeEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_MaritalStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MaritalStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_MaritalStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_Memberships",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Memberships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_Name",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                    Pronunciation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Name", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_Nationality",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NationalityCountry = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Nationality", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_Photos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Photos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_UsCitizenship",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UsCitizen = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_UsCitizenship", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WD_CertificationAchievement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CertificationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_CertificationAchievement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_CertificationAchievement_WD_Certification_CertificationId",
                        column: x => x.CertificationId,
                        principalTable: "WD_Certification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_ExamHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExamDate = table.Column<DateTimeOffset>(nullable: true),
                    ExamScore = table.Column<int>(nullable: true),
                    ExamActive = table.Column<bool>(nullable: true),
                    CertificationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_ExamHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_ExamHistory_WD_Certification_CertificationId",
                        column: x => x.CertificationId,
                        principalTable: "WD_Certification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_BasePayData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CompensationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_BasePayData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_BasePayData_WD_Compensation_CompensationId",
                        column: x => x.CompensationId,
                        principalTable: "WD_Compensation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                    UsageData = table.Column<string>(nullable: true),
                    ContactId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Address_WD_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "WD_Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Email",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmailAddress = table.Column<string>(nullable: true),
                    EmailComment = table.Column<string>(nullable: true),
                    EmailType = table.Column<string>(nullable: true),
                    PrimaryIndicator = table.Column<bool>(nullable: true),
                    PublicIndicator = table.Column<bool>(nullable: true),
                    ContactId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Email", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Email_WD_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "WD_Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_PhoneNumber",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryAccessCode = table.Column<string>(nullable: true),
                    AreaCityCode = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    DeviceType = table.Column<string>(nullable: true),
                    PhoneType = table.Column<string>(nullable: true),
                    PhoneNumberFormatted = table.Column<string>(nullable: true),
                    PrimaryIndicator = table.Column<bool>(nullable: true),
                    PublicIndicator = table.Column<bool>(nullable: true),
                    ContactId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_PhoneNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_PhoneNumber_WD_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "WD_Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_SocialMedia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SocialMediaConnector = table.Column<string>(nullable: true),
                    SocialMediaCategory = table.Column<string>(nullable: true),
                    SocialMediaType = table.Column<string>(nullable: true),
                    ContactId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_SocialMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_SocialMedia_WD_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "WD_Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_WebUrl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true),
                    ContactId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_WebUrl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_WebUrl_WD_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "WD_Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_EmploymentPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PrimaryJobIndicator = table.Column<int>(nullable: true),
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
                    JobFTE = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PayGroup = table.Column<string>(nullable: true),
                    TerminationReason = table.Column<string>(nullable: true),
                    AnnualWorkPeriodStartDate = table.Column<DateTimeOffset>(nullable: true),
                    AnnualWorkPeriodEndDate = table.Column<DateTimeOffset>(nullable: true),
                    WorkPeriodPercentOfYear = table.Column<int>(nullable: true),
                    DisbursementPlanPeriodStartDate = table.Column<DateTimeOffset>(nullable: true),
                    DisbursementPlanPeriodEndDate = table.Column<DateTimeOffset>(nullable: true),
                    PayType = table.Column<string>(nullable: true),
                    EmploymentProfileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_EmploymentPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_EmploymentPosition_WD_EmploymentProfile_EmploymentProfileId",
                        column: x => x.EmploymentProfileId,
                        principalTable: "WD_EmploymentProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Biographic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LifeEventsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Biographic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Biographic_WD_LifeEvents_LifeEventsId",
                        column: x => x.LifeEventsId,
                        principalTable: "WD_LifeEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Affiliation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Association = table.Column<string>(nullable: true),
                    Participation = table.Column<string>(nullable: true),
                    MembershipsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Affiliation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Affiliation_WD_Memberships_MembershipsId",
                        column: x => x.MembershipsId,
                        principalTable: "WD_Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_SourcePhotograph",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageIdentifier = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    PhotoType = table.Column<string>(nullable: true),
                    PhotoUsage = table.Column<string>(nullable: true),
                    PhotoUsageRelease = table.Column<string>(nullable: true),
                    PhotosId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_SourcePhotograph", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_SourcePhotograph_WD_Photos_PhotosId",
                        column: x => x.PhotosId,
                        principalTable: "WD_Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Demographic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MaritalStatusesId = table.Column<int>(nullable: true),
                    UsCitizenshipId = table.Column<int>(nullable: true),
                    EthnicCategoryId = table.Column<int>(nullable: true),
                    NationalityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Demographic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Demographic_WD_EthnicCategories_EthnicCategoryId",
                        column: x => x.EthnicCategoryId,
                        principalTable: "WD_EthnicCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Demographic_WD_MaritalStatuses_MaritalStatusesId",
                        column: x => x.MaritalStatusesId,
                        principalTable: "WD_MaritalStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Demographic_WD_Nationality_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "WD_Nationality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Demographic_WD_UsCitizenship_UsCitizenshipId",
                        column: x => x.UsCitizenshipId,
                        principalTable: "WD_UsCitizenship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_CertificationData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CertificationId = table.Column<string>(nullable: true),
                    CredentialType = table.Column<string>(nullable: true),
                    Issuer = table.Column<string>(nullable: true),
                    IssuingCountry = table.Column<string>(nullable: true),
                    RestrictToCountryRegion = table.Column<string>(nullable: true),
                    CredentialIssueDate = table.Column<DateTimeOffset>(nullable: true),
                    CredentialExpirationDate = table.Column<DateTimeOffset>(nullable: true),
                    RenewalRequired = table.Column<bool>(nullable: true),
                    RenewalPeriod = table.Column<string>(nullable: true),
                    CertificationAchievementId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_CertificationData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_CertificationData_WD_CertificationAchievement_CertificationAchievementId",
                        column: x => x.CertificationAchievementId,
                        principalTable: "WD_CertificationAchievement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Organization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationReferenceID = table.Column<string>(nullable: true),
                    OrganizationName = table.Column<string>(nullable: true),
                    OrganizationCode = table.Column<string>(nullable: true),
                    OrganizationType = table.Column<string>(nullable: true),
                    EmploymentPositionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Organization_WD_EmploymentPosition_EmploymentPositionId",
                        column: x => x.EmploymentPositionId,
                        principalTable: "WD_EmploymentPosition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Disabilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Disability = table.Column<bool>(nullable: false),
                    BiographicId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Disabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Disabilities_WD_Biographic_BiographicId",
                        column: x => x.BiographicId,
                        principalTable: "WD_Biographic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Gender",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GenderType = table.Column<string>(nullable: true),
                    GenderCategory = table.Column<string>(nullable: true),
                    BiographicId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Gender", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Gender_WD_Biographic_BiographicId",
                        column: x => x.BiographicId,
                        principalTable: "WD_Biographic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Citizenship",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CitizenshipStatus = table.Column<string>(nullable: true),
                    CitizenshipCountry = table.Column<string>(nullable: true),
                    CitizenshipCountryCode = table.Column<string>(nullable: true),
                    DemographicId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Citizenship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Citizenship_WD_Demographic_DemographicId",
                        column: x => x.DemographicId,
                        principalTable: "WD_Demographic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Ethnicity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ethnicities = table.Column<string>(nullable: true),
                    DemographicId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Ethnicity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Ethnicity_WD_Demographic_DemographicId",
                        column: x => x.DemographicId,
                        principalTable: "WD_Demographic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_MilitaryService",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MilitaryServiceStatus = table.Column<string>(nullable: true),
                    MilitaryDischargeDate = table.Column<DateTimeOffset>(nullable: true),
                    MilitaryServiceDischargeType = table.Column<string>(nullable: true),
                    DemographicId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_MilitaryService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_MilitaryService_WD_Demographic_DemographicId",
                        column: x => x.DemographicId,
                        principalTable: "WD_Demographic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_VeteranStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Veteran = table.Column<bool>(nullable: false),
                    DemographicId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_VeteranStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_VeteranStatus_WD_Demographic_DemographicId",
                        column: x => x.DemographicId,
                        principalTable: "WD_Demographic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Workers",
                columns: table => new
                {
                    UniversalId = table.Column<string>(nullable: false),
                    WorkerId = table.Column<string>(nullable: false),
                    LegacyEmployeeId = table.Column<string>(nullable: true),
                    NameId = table.Column<int>(nullable: true),
                    BiographicId = table.Column<int>(nullable: true),
                    ContactId = table.Column<int>(nullable: true),
                    DemographicId = table.Column<int>(nullable: true),
                    PhotosId = table.Column<int>(nullable: true),
                    EmploymentProfileId = table.Column<int>(nullable: true),
                    CertificationId = table.Column<int>(nullable: true),
                    CompensationId = table.Column<int>(nullable: true),
                    MembershipsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Workers", x => x.UniversalId);
                    table.ForeignKey(
                        name: "FK_WD_Workers_WD_Biographic_BiographicId",
                        column: x => x.BiographicId,
                        principalTable: "WD_Biographic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Workers_WD_Certification_CertificationId",
                        column: x => x.CertificationId,
                        principalTable: "WD_Certification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Workers_WD_Compensation_CompensationId",
                        column: x => x.CompensationId,
                        principalTable: "WD_Compensation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Workers_WD_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "WD_Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Workers_WD_Demographic_DemographicId",
                        column: x => x.DemographicId,
                        principalTable: "WD_Demographic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Workers_WD_EmploymentProfile_EmploymentProfileId",
                        column: x => x.EmploymentProfileId,
                        principalTable: "WD_EmploymentProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Workers_WD_Memberships_MembershipsId",
                        column: x => x.MembershipsId,
                        principalTable: "WD_Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Workers_WD_Name_NameId",
                        column: x => x.NameId,
                        principalTable: "WD_Name",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WD_Workers_WD_Photos_PhotosId",
                        column: x => x.PhotosId,
                        principalTable: "WD_Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Education",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WorkerUniversalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Education_WD_Workers_WorkerUniversalId",
                        column: x => x.WorkerUniversalId,
                        principalTable: "WD_Workers",
                        principalColumn: "UniversalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_ExternalJobHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WorkerUniversalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_ExternalJobHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_ExternalJobHistory_WD_Workers_WorkerUniversalId",
                        column: x => x.WorkerUniversalId,
                        principalTable: "WD_Workers",
                        principalColumn: "UniversalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_GovernmentId",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdNumber = table.Column<string>(nullable: true),
                    IdType = table.Column<string>(nullable: true),
                    IssuingCountry = table.Column<string>(nullable: true),
                    IssuedDate = table.Column<DateTimeOffset>(nullable: true),
                    ExpiryDate = table.Column<DateTimeOffset>(nullable: true),
                    WorkerUniversalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_GovernmentId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_GovernmentId_WD_Workers_WorkerUniversalId",
                        column: x => x.WorkerUniversalId,
                        principalTable: "WD_Workers",
                        principalColumn: "UniversalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Role = table.Column<string>(nullable: true),
                    PrimaryRoleIndicator = table.Column<bool>(nullable: true),
                    WorkerUniversalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_UserRole_WD_Workers_WorkerUniversalId",
                        column: x => x.WorkerUniversalId,
                        principalTable: "WD_Workers",
                        principalColumn: "UniversalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_Visa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                    WorkerUniversalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_Visa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_Visa_WD_Workers_WorkerUniversalId",
                        column: x => x.WorkerUniversalId,
                        principalTable: "WD_Workers",
                        principalColumn: "UniversalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_EducationData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryOfEducation = table.Column<string>(nullable: true),
                    StateProvinceOfEducation = table.Column<string>(nullable: true),
                    StateCode = table.Column<string>(nullable: true),
                    School = table.Column<string>(nullable: true),
                    SchoolLocation = table.Column<string>(nullable: true),
                    EducationalInstitution = table.Column<string>(nullable: true),
                    EducationalInstitutionType = table.Column<string>(nullable: true),
                    CurrentlyEnrolledIndicator = table.Column<bool>(nullable: true),
                    YearEnrolled = table.Column<string>(nullable: true),
                    EducationalDegree = table.Column<string>(nullable: true),
                    EducationalDegreeCompleted = table.Column<string>(nullable: true),
                    HighestDegree = table.Column<bool>(nullable: true),
                    YearOfGraduation = table.Column<int>(nullable: true),
                    EducationalMajorFieldOfStudy = table.Column<string>(nullable: true),
                    GradePointAverage = table.Column<string>(nullable: true),
                    EducationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_EducationData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_EducationData_WD_Education_EducationId",
                        column: x => x.EducationId,
                        principalTable: "WD_Education",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WD_JobHistoryData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExternalEmploymentId = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTimeOffset>(nullable: true),
                    EndDate = table.Column<DateTimeOffset>(nullable: true),
                    ResponsibilitiesAndAchievements = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    JobReference = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    ExternalJobHistoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WD_JobHistoryData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WD_JobHistoryData_WD_ExternalJobHistory_ExternalJobHistoryId",
                        column: x => x.ExternalJobHistoryId,
                        principalTable: "WD_ExternalJobHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WD_Address_ContactId",
                table: "WD_Address",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Affiliation_MembershipsId",
                table: "WD_Affiliation",
                column: "MembershipsId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_BasePayData_CompensationId",
                table: "WD_BasePayData",
                column: "CompensationId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Biographic_LifeEventsId",
                table: "WD_Biographic",
                column: "LifeEventsId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_CertificationAchievement_CertificationId",
                table: "WD_CertificationAchievement",
                column: "CertificationId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_CertificationData_CertificationAchievementId",
                table: "WD_CertificationData",
                column: "CertificationAchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Citizenship_DemographicId",
                table: "WD_Citizenship",
                column: "DemographicId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Demographic_EthnicCategoryId",
                table: "WD_Demographic",
                column: "EthnicCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Demographic_MaritalStatusesId",
                table: "WD_Demographic",
                column: "MaritalStatusesId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Demographic_NationalityId",
                table: "WD_Demographic",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Demographic_UsCitizenshipId",
                table: "WD_Demographic",
                column: "UsCitizenshipId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Disabilities_BiographicId",
                table: "WD_Disabilities",
                column: "BiographicId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Education_WorkerUniversalId",
                table: "WD_Education",
                column: "WorkerUniversalId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_EducationData_EducationId",
                table: "WD_EducationData",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Email_ContactId",
                table: "WD_Email",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_EmploymentPosition_EmploymentProfileId",
                table: "WD_EmploymentPosition",
                column: "EmploymentProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Ethnicity_DemographicId",
                table: "WD_Ethnicity",
                column: "DemographicId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_ExamHistory_CertificationId",
                table: "WD_ExamHistory",
                column: "CertificationId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_ExternalJobHistory_WorkerUniversalId",
                table: "WD_ExternalJobHistory",
                column: "WorkerUniversalId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Gender_BiographicId",
                table: "WD_Gender",
                column: "BiographicId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_GovernmentId_WorkerUniversalId",
                table: "WD_GovernmentId",
                column: "WorkerUniversalId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_JobHistoryData_ExternalJobHistoryId",
                table: "WD_JobHistoryData",
                column: "ExternalJobHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_MilitaryService_DemographicId",
                table: "WD_MilitaryService",
                column: "DemographicId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Organization_EmploymentPositionId",
                table: "WD_Organization",
                column: "EmploymentPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_PhoneNumber_ContactId",
                table: "WD_PhoneNumber",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_SocialMedia_ContactId",
                table: "WD_SocialMedia",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_SourcePhotograph_PhotosId",
                table: "WD_SourcePhotograph",
                column: "PhotosId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_UserRole_WorkerUniversalId",
                table: "WD_UserRole",
                column: "WorkerUniversalId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_VeteranStatus_DemographicId",
                table: "WD_VeteranStatus",
                column: "DemographicId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Visa_WorkerUniversalId",
                table: "WD_Visa",
                column: "WorkerUniversalId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_WebUrl_ContactId",
                table: "WD_WebUrl",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Workers_BiographicId",
                table: "WD_Workers",
                column: "BiographicId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Workers_CertificationId",
                table: "WD_Workers",
                column: "CertificationId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Workers_CompensationId",
                table: "WD_Workers",
                column: "CompensationId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Workers_ContactId",
                table: "WD_Workers",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Workers_DemographicId",
                table: "WD_Workers",
                column: "DemographicId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Workers_EmploymentProfileId",
                table: "WD_Workers",
                column: "EmploymentProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Workers_MembershipsId",
                table: "WD_Workers",
                column: "MembershipsId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Workers_NameId",
                table: "WD_Workers",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_WD_Workers_PhotosId",
                table: "WD_Workers",
                column: "PhotosId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WD_Address");

            migrationBuilder.DropTable(
                name: "WD_Affiliation");

            migrationBuilder.DropTable(
                name: "WD_BasePayData");

            migrationBuilder.DropTable(
                name: "WD_CertificationData");

            migrationBuilder.DropTable(
                name: "WD_Citizenship");

            migrationBuilder.DropTable(
                name: "WD_Disabilities");

            migrationBuilder.DropTable(
                name: "WD_EducationData");

            migrationBuilder.DropTable(
                name: "WD_Email");

            migrationBuilder.DropTable(
                name: "WD_Ethnicity");

            migrationBuilder.DropTable(
                name: "WD_ExamHistory");

            migrationBuilder.DropTable(
                name: "WD_Gender");

            migrationBuilder.DropTable(
                name: "WD_GovernmentId");

            migrationBuilder.DropTable(
                name: "WD_JobHistoryData");

            migrationBuilder.DropTable(
                name: "WD_MilitaryService");

            migrationBuilder.DropTable(
                name: "WD_Organization");

            migrationBuilder.DropTable(
                name: "WD_PhoneNumber");

            migrationBuilder.DropTable(
                name: "WD_SocialMedia");

            migrationBuilder.DropTable(
                name: "WD_SourcePhotograph");

            migrationBuilder.DropTable(
                name: "WD_UserRole");

            migrationBuilder.DropTable(
                name: "WD_VeteranStatus");

            migrationBuilder.DropTable(
                name: "WD_Visa");

            migrationBuilder.DropTable(
                name: "WD_WebUrl");

            migrationBuilder.DropTable(
                name: "WD_CertificationAchievement");

            migrationBuilder.DropTable(
                name: "WD_Education");

            migrationBuilder.DropTable(
                name: "WD_ExternalJobHistory");

            migrationBuilder.DropTable(
                name: "WD_EmploymentPosition");

            migrationBuilder.DropTable(
                name: "WD_Workers");

            migrationBuilder.DropTable(
                name: "WD_Biographic");

            migrationBuilder.DropTable(
                name: "WD_Certification");

            migrationBuilder.DropTable(
                name: "WD_Compensation");

            migrationBuilder.DropTable(
                name: "WD_Contact");

            migrationBuilder.DropTable(
                name: "WD_Demographic");

            migrationBuilder.DropTable(
                name: "WD_EmploymentProfile");

            migrationBuilder.DropTable(
                name: "WD_Memberships");

            migrationBuilder.DropTable(
                name: "WD_Name");

            migrationBuilder.DropTable(
                name: "WD_Photos");

            migrationBuilder.DropTable(
                name: "WD_LifeEvents");

            migrationBuilder.DropTable(
                name: "WD_EthnicCategories");

            migrationBuilder.DropTable(
                name: "WD_MaritalStatuses");

            migrationBuilder.DropTable(
                name: "WD_Nationality");

            migrationBuilder.DropTable(
                name: "WD_UsCitizenship");
        }
    }
}
