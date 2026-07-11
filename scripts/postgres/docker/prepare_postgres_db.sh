#!/bin/bash

cd "$(dirname "$0")/../../.."
docker exec -i test-postgres psql -U postgres -d testdb < backend/sql/postgres.sql
