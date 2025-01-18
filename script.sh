set -e
cd Play.Items
docker build -t play-items-app .
cd ../Play.Infra
docker compose up -d