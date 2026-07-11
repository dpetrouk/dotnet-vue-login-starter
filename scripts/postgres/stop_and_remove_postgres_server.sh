#!/bin/bash

cd "$(dirname "$0")/../.."
pg_ctl -D pgdata stop
rm -rf pgdata
