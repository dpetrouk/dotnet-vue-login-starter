#!/bin/bash

cd "$(dirname "$0")/../.."
psql -h localhost -U postgres -d testdb < backend/sql/postgres.sql
