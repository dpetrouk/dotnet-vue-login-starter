#!/usr/bin/env bash

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
if docker ps --format '{{.Names}}' 2>/dev/null | grep -q '^test-postgres$'; then
    echo "==> PostgreSQL already running."
else
    echo "==> Starting PostgreSQL container..."
    docker compose -f docker/docker-compose.yml up -d || {
        echo "FAILED to start PostgreSQL. Is Docker running?"
        exit 1
    }
    echo "==> Waiting for PostgreSQL..."
    until docker exec test-postgres pg_isready -q 2>/dev/null; do
        sleep 1
    done
    echo "    PostgreSQL ready."
fi

# ── Backend ───────────────────────────────────────────────
echo "==> Starting backend..."
cd backend
dotnet run &
BACKEND_PID=$!
cd "$ROOT_DIR"

# ── Frontend ──────────────────────────────────────────────
echo "==> Setting up frontend..."
cd frontend || {
    echo "FAILED: frontend/ directory not found"
    exit 1
}

if [ ! -d node_modules ]; then
    echo "==> Installing dependencies..."
    npm install || {
        echo "FAILED: npm install"
        exit 1
    }
fi

echo ""
echo "    Open http://localhost:5173"
echo "    Login: user@example.com / password123"
echo ""
npm run dev
