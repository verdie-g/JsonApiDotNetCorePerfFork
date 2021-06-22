#!/usr/bin/env sh

results_dir="./load-test/results"

rm -rf $results_dir
mkdir -p $results_dir

export UPLOAD_RESULTS="false"

docker-compose -f stack.yml build
docker-compose -f stack.yml up &

while [ ! -f "$results_dir/test-completed-signal" ]; do sleep 5; done

echo "Received test completion signal, stopping containers."
rm -f "$results_dir/test-completed-signal"

docker-compose -f stack.yml down
