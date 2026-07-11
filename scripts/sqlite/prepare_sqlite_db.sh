#!/bin/bash

cd "$(dirname "$0")/../.."
sqlite3 backend/profiles.db < backend/sql/sqlite.sql
