-- Seed: test user (password: password123)
INSERT INTO users ("Email", "PasswordHash")
SELECT 'user@example.com', '$2a$11$sAhnqV/kmez2U5qeh3oexuYYlHwLiJQM/3bqKgJQ8qmkB67B3ewOu'
WHERE NOT EXISTS (SELECT 1 FROM users WHERE "Email" = 'user@example.com');
