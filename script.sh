set -e
cd Play.Items
docker build -t play-items-app .
cd ../Play.ApiGateway/src
docker build -t play-apigateway-app .
cd ../../Play.Infra
docker compose up -d