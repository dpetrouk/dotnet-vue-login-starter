#!/bin/bash

cd "$(dirname "$0")/../.."
initdb -D pgdata --username=postgres --pwfile=<(echo postgres) -c unix_socket_directories=''
pg_ctl -D pgdata -l pgdata/logfile start
createdb -h localhost -U postgres testdb
