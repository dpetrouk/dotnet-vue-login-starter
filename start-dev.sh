#!/usr/bin/env bash
set -e

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$ROOT_DIR"

cleanup() {
    echo ""
    echo "Shutting down..."
    [ -n "$BACKEND_PID" ] && kill "$BACKEND_PID" 2>/dev/null
    exit 0
}
trap cleanup SIGINT SIGTERM

# ── PostgreSQL ────────────────────────────────────────────
if ! docker ps --format '{{.Names}}' | grep -q '^test-postgres$'; then
    echo "==> Starting PostgreSQL container..."
    docker compose -f docker/docker-compose.yml up -d

    echo "==> Waiting for PostgreSQL to be ready..."
    until docker exec test-postgres pg_isready -q 2>/dev/null; do
        sleep 1
    done
    echo "    PostgreSQL ready."
else
    echo "==> PostgreSQL already running."
fi

# ── Backend ───────────────────────────────────────────────
echo "==> Starting backend..."
cd backend
dotnet run &
BACKEND_PID=$!
cd "$ROOT_DIR"

# ── Frontend ──────────────────────────────────────────────
echo "==> Installing frontend dependencies..."
cd frontend
npm install --silent 2>/dev/null

echo "==> Starting frontend dev server..."
echo ""
echo "    Open http://localhost:5173"
echo "    Login: user@example.com / password123"
echo ""
npm run dev
