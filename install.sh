#!/usr/bin/env sh

# Install Docker Compose
sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose
docker-compose --version

# Install Docker Machine
curl -L "https://github.com/docker/machine/releases/latest/download/docker-machine-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-machine
chmod +x /usr/local/bin/docker-machine
docker-machine version

# Use default settings
cp ./variables.env.sample ./variables.env
