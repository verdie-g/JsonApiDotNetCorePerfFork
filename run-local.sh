#!/usr/bin/env sh

export UPLOAD_RESULTS="false"

docker-compose -f stack.yml build
docker-compose -f stack.yml up
