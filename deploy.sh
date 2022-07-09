#!/bin/bash
set -e

echo "Fazendo deploy ..."

docker-compose build
docker-compose up --no-deps -d

echo "🚀 API subiu. FINALMENTE!"
