CREATE TABLE IF NOT EXISTS users (
    "Id"            INTEGER NOT NULL CONSTRAINT "PK_users" PRIMARY KEY AUTOINCREMENT,
    "Email"         TEXT NOT NULL,
    "PasswordHash"  TEXT NOT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_users_Email" ON users ("Email");

CREATE TABLE IF NOT EXISTS user_profiles (
    "Id"        INTEGER NOT NULL CONSTRAINT "PK_user_profiles" PRIMARY KEY AUTOINCREMENT,
    "UserId"    INTEGER NOT NULL,
    "FullName"  TEXT NOT NULL,
    "City"      TEXT NOT NULL,
    "Street"    TEXT NOT NULL,
    "Building"  TEXT NOT NULL,
    "ZipCode"   TEXT NOT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_user_profiles_UserId" ON user_profiles ("UserId");

INSERT OR IGNORE INTO users ("Id", "Email", "PasswordHash")
VALUES (1, 'user@example.com', 'AQAAAAIAAYagAAAAEDJiv4TCfjhTrXw6+imdSbRu2h9aN6HJzf7ZsK3Hoq6CUrurFiZWjhDhW87Nbpj3dg==');

INSERT OR IGNORE INTO user_profiles ("Id", "UserId", "FullName", "City", "Street", "Building", "ZipCode")
VALUES (1, 1, 'Иванов Иван Иванович', 'Москва', 'Тверская', '1', '125009');
