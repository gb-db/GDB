using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Users.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<int>(nullable: false),
                    Qualifications = table.Column<int>(nullable: false),
                    UserCode = table.Column<string>(maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserCode = table.Column<string>(maxLength: 8, nullable: true),
                    status = table.Column<bool>(nullable: false),
                    country = table.Column<int>(nullable: false),
                    province = table.Column<int>(nullable: false),
                    church = table.Column<int>(nullable: false),
                    person = table.Column<string>(maxLength: 50, nullable: true),
                    fName = table.Column<string>(maxLength: 50, nullable: true),
                    lName = table.Column<string>(maxLength: 50, nullable: true),
                    description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    ShortName = table.Column<string>(maxLength: 10, nullable: true),
                    imgPath = table.Column<string>(maxLength: 150, nullable: true),
                    isActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "HeratigeUsers",
                columns: table => new
                {
                    HeratigeUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserCode = table.Column<string>(maxLength: 8, nullable: true),
                    user_id = table.Column<int>(nullable: false),
                    lName = table.Column<string>(maxLength: 50, nullable: true),
                    fName = table.Column<string>(maxLength: 50, nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeratigeUsers", x => x.HeratigeUserId);
                });

            migrationBuilder.CreateTable(
                name: "NewsData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserCode = table.Column<string>(maxLength: 8, nullable: true),
                    user_id = table.Column<int>(nullable: false),
                    Path = table.Column<string>(maxLength: 1500, nullable: true),
                    status = table.Column<bool>(nullable: false),
                    country = table.Column<int>(nullable: false),
                    province = table.Column<int>(nullable: false),
                    church = table.Column<int>(nullable: false),
                    person = table.Column<string>(maxLength: 50, nullable: true),
                    fName = table.Column<string>(maxLength: 50, nullable: true),
                    lName = table.Column<string>(maxLength: 50, nullable: true),
                    description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    provinceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    ShortName = table.Column<string>(maxLength: 10, nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    countryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.provinceId);
                    table.ForeignKey(
                        name: "FK_Province_Country_countryId",
                        column: x => x.countryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HeratigePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserCode = table.Column<string>(maxLength: 8, nullable: true),
                    user_id = table.Column<int>(nullable: false),
                    isReadable = table.Column<bool>(nullable: false),
                    isWritable = table.Column<bool>(nullable: false),
                    isRoot = table.Column<bool>(nullable: false),
                    permit = table.Column<bool>(nullable: false),
                    dateTime = table.Column<DateTime>(nullable: false),
                    HeratigeUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeratigePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeratigePermissions_HeratigeUsers_HeratigeUserId",
                        column: x => x.HeratigeUserId,
                        principalTable: "HeratigeUsers",
                        principalColumn: "HeratigeUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HerInfoSrcs",
                columns: table => new
                {
                    HerInfoSrcId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(nullable: false),
                    parent_level = table.Column<int>(nullable: false),
                    parent_number = table.Column<int>(nullable: false),
                    level = table.Column<int>(nullable: false),
                    number = table.Column<int>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    desc = table.Column<string>(nullable: true),
                    HeratigeUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HerInfoSrcs", x => x.HerInfoSrcId);
                    table.ForeignKey(
                        name: "FK_HerInfoSrcs_HeratigeUsers_HeratigeUserId",
                        column: x => x.HeratigeUserId,
                        principalTable: "HeratigeUsers",
                        principalColumn: "HeratigeUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    cityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    ShortName = table.Column<string>(maxLength: 10, nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    provinceId = table.Column<int>(nullable: true),
                    countryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.cityId);
                    table.ForeignKey(
                        name: "FK_City_Country_countryId",
                        column: x => x.countryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_City_Province_provinceId",
                        column: x => x.provinceId,
                        principalTable: "Province",
                        principalColumn: "provinceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Info_Srcs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    number = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 10000, nullable: true),
                    src = table.Column<string>(maxLength: 1500, nullable: true),
                    tt_width = table.Column<int>(nullable: false),
                    tt_height = table.Column<int>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    HerInfoSrcId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Info_Srcs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Info_Srcs_HerInfoSrcs_HerInfoSrcId",
                        column: x => x.HerInfoSrcId,
                        principalTable: "HerInfoSrcs",
                        principalColumn: "HerInfoSrcId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Church",
                columns: table => new
                {
                    churchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 150, nullable: true),
                    ShortName = table.Column<string>(maxLength: 10, nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    countryId = table.Column<int>(nullable: true),
                    provinceId = table.Column<int>(nullable: true),
                    cityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Church", x => x.churchId);
                    table.ForeignKey(
                        name: "FK_Church_City_cityId",
                        column: x => x.cityId,
                        principalTable: "City",
                        principalColumn: "cityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Church_Country_countryId",
                        column: x => x.countryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Church_Province_provinceId",
                        column: x => x.provinceId,
                        principalTable: "Province",
                        principalColumn: "provinceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HeratigeDatas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserCode = table.Column<string>(maxLength: 8, nullable: true),
                    user_id = table.Column<int>(nullable: false),
                    parent_level = table.Column<int>(nullable: false),
                    parent_number = table.Column<int>(nullable: false),
                    level = table.Column<int>(nullable: false),
                    number = table.Column<int>(nullable: false),
                    last_child = table.Column<int>(nullable: false),
                    tt_width = table.Column<int>(nullable: false),
                    tt_height = table.Column<int>(nullable: false),
                    DOB = table.Column<DateTime>(nullable: true),
                    PWD = table.Column<DateTime>(nullable: true),
                    ImagePath = table.Column<string>(maxLength: 1500, nullable: true),
                    ImageFolderPath = table.Column<string>(maxLength: 1500, nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    isEditable = table.Column<bool>(nullable: false),
                    isClosed = table.Column<bool>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    provinceId = table.Column<int>(nullable: true),
                    cityId = table.Column<int>(nullable: true),
                    churchId = table.Column<int>(nullable: true),
                    CountryBPId = table.Column<int>(nullable: true),
                    provinceBPId = table.Column<int>(nullable: true),
                    cityBPId = table.Column<int>(nullable: true),
                    person = table.Column<string>(maxLength: 50, nullable: true),
                    fName = table.Column<string>(maxLength: 50, nullable: true),
                    lName = table.Column<string>(maxLength: 50, nullable: true),
                    information = table.Column<string>(nullable: true),
                    fNameParentF = table.Column<string>(maxLength: 50, nullable: true),
                    lNameParentF = table.Column<string>(maxLength: 50, nullable: true),
                    fNameParentM = table.Column<string>(maxLength: 50, nullable: true),
                    lNameParentM = table.Column<string>(maxLength: 50, nullable: true),
                    pattern = table.Column<string>(maxLength: 5000, nullable: true),
                    dateTime = table.Column<DateTime>(nullable: false),
                    HeratigeUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeratigeDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeratigeDatas_Country_CountryBPId",
                        column: x => x.CountryBPId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HeratigeDatas_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeratigeDatas_HeratigeUsers_HeratigeUserId",
                        column: x => x.HeratigeUserId,
                        principalTable: "HeratigeUsers",
                        principalColumn: "HeratigeUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeratigeDatas_Church_churchId",
                        column: x => x.churchId,
                        principalTable: "Church",
                        principalColumn: "churchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HeratigeDatas_City_cityBPId",
                        column: x => x.cityBPId,
                        principalTable: "City",
                        principalColumn: "cityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HeratigeDatas_City_cityId",
                        column: x => x.cityId,
                        principalTable: "City",
                        principalColumn: "cityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HeratigeDatas_Province_provinceBPId",
                        column: x => x.provinceBPId,
                        principalTable: "Province",
                        principalColumn: "provinceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HeratigeDatas_Province_provinceId",
                        column: x => x.provinceId,
                        principalTable: "Province",
                        principalColumn: "provinceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Church_cityId",
                table: "Church",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_Church_countryId",
                table: "Church",
                column: "countryId");

            migrationBuilder.CreateIndex(
                name: "IX_Church_provinceId",
                table: "Church",
                column: "provinceId");

            migrationBuilder.CreateIndex(
                name: "IX_City_countryId",
                table: "City",
                column: "countryId");

            migrationBuilder.CreateIndex(
                name: "IX_City_provinceId",
                table: "City",
                column: "provinceId");

            migrationBuilder.CreateIndex(
                name: "IX_HeratigeDatas_CountryBPId",
                table: "HeratigeDatas",
                column: "CountryBPId");

            migrationBuilder.CreateIndex(
                name: "IX_HeratigeDatas_CountryId",
                table: "HeratigeDatas",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_HeratigeDatas_HeratigeUserId",
                table: "HeratigeDatas",
                column: "HeratigeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HeratigeDatas_churchId",
                table: "HeratigeDatas",
                column: "churchId");

            migrationBuilder.CreateIndex(
                name: "IX_HeratigeDatas_cityBPId",
                table: "HeratigeDatas",
                column: "cityBPId");

            migrationBuilder.CreateIndex(
                name: "IX_HeratigeDatas_cityId",
                table: "HeratigeDatas",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_HeratigeDatas_provinceBPId",
                table: "HeratigeDatas",
                column: "provinceBPId");

            migrationBuilder.CreateIndex(
                name: "IX_HeratigeDatas_provinceId",
                table: "HeratigeDatas",
                column: "provinceId");

            migrationBuilder.CreateIndex(
                name: "IX_HeratigePermissions_HeratigeUserId",
                table: "HeratigePermissions",
                column: "HeratigeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HerInfoSrcs_HeratigeUserId",
                table: "HerInfoSrcs",
                column: "HeratigeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Info_Srcs_HerInfoSrcId",
                table: "Info_Srcs",
                column: "HerInfoSrcId");

            migrationBuilder.CreateIndex(
                name: "IX_Province_countryId",
                table: "Province",
                column: "countryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "HeratigeDatas");

            migrationBuilder.DropTable(
                name: "HeratigePermissions");

            migrationBuilder.DropTable(
                name: "Info_Srcs");

            migrationBuilder.DropTable(
                name: "NewsData");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Church");

            migrationBuilder.DropTable(
                name: "HerInfoSrcs");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "HeratigeUsers");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
