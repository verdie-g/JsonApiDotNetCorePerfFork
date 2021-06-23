#!/usr/bin/env sh

docker-machine stop docker-sandbox
docker-machine rm docker-sandbox -y

# unset docker-machine env vars
eval $(docker-machine env -u)
