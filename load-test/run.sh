#!/usr/bin/env bash

script_dir="$(dirname ${BASH_SOURCE[0]})"
results_dir="$script_dir/results"

rm -rf $results_dir
mkdir -p $results_dir

docker build --tag jadnc-load-test "$script_dir"
docker run --rm --interactive --tty --network host --volume $(cd $results_dir; pwd):/app/results jadnc-load-test
