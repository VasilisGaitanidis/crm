-- add role
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('3e6337f1-8150-4e26-9f90-c2498f33e9e9', 'fa12955e-925b-42fa-a1d5-f0586177d7c8', 'admin', 'admin');
INSERT INTO "AspNetRoles" ("Id", "ConcurrencyStamp", "Name", "NormalizedName")
VALUES ('553a4eb9-3a63-4c72-bde8-d64130826d25', 'cdb206c9-f4f7-4391-a4fb-203c52d6aeba', 'guest', 'guest');

-- add user and claim
INSERT INTO "AspNetUsers" ("Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName")
VALUES ('83756763-d927-46f7-9ede-7acfc821c3d8', 0, '2926f75c-a474-43ee-9c5b-84d2fa560e72', 'admin@nomail.com', TRUE, FALSE, NULL, 'admin@nomail.com', 'ADMIN@NOMAIL.COM', 'AQAAAAEAACcQAAAAEEvgLtEKYkeePCQsD9NoKcjAR0QsJ4UJQg/h6/Xae924TRlk5/NdVD6tnoRHoKQn5w==', NULL, FALSE, 'db142bdd-2723-4f19-b404-cb02078e34df', FALSE, 'admin@nomail.com');

INSERT INTO "AspNetUserClaims" ("Id", "ClaimType", "ClaimValue", "UserId")
VALUES (1, 'email', 'admin@nomail.com', '83756763-d927-46f7-9ede-7acfc821c3d8');
