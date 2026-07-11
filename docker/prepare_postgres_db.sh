#!/bin/bash

docker exec -i test-postgres psql -U postgres -d testdb < ../backend/sql/postgres.sql
