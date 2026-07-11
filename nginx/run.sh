#!/bin/bash
cd "$(dirname "$0")"
echo "http://localhost:8080/"
exec nginx -c "$PWD/nginx.conf" -p "$PWD" -g 'daemon off;'
