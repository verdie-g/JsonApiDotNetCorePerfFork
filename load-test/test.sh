#!/usr/bin/env bash

set -e

BASE_URL='http://localhost:5000'

results_dir="./results"

attack() {
    prefix=$(printf "%03d" $i)
    file_name="$prefix-$1_$3_$4.txt"
    file_path="$results_dir/$file_name"

    echo "Running $file_name"
    echo "$2" | \
        vegeta attack -format=json -rate=$3 -duration=$4m | \
        tee results.bin | \
        vegeta report > $file_path

    i=$((i+1))
}

get_todoItems() {
    attack "TodoItems" "{\"method\": \"GET\", \"url\": \"$BASE_URL/todoItems\"}" $1 $2
}

get_todoItems_sort() {
    attack "TodoItems_Sort" "{\"method\": \"GET\", \"url\": \"$BASE_URL/todoItems?sort=-id\"}" $1 $2
}

get_todoItem_includes() {
    attack "TodoItem_Includes" "{\"method\": \"GET\", \"url\": \"$BASE_URL/todoItems/1?include=owner,assignee\"}" $1 $2
}

get_todoItems_pagination() {
    attack "TodoItems_Pagination" "{\"method\": \"GET\", \"url\": \"$BASE_URL/todoItems?page%5Bsize%5D=5&page%5Bnumber%5D=8\"}" $1 $2
}

get_todoItems_filter() {
    attack "TodoItems_Filter" "{\"method\": \"GET\", \"url\": \"$BASE_URL/todoItems?filter=and(equals(priority,%27High%27),equals(owner.firstName,%27Debbi%27))\"}" $1 $2
}

get_todoItems_fields() {
    attack "TodoItems_Fields" "{\"method\": \"GET\", \"url\": \"$BASE_URL/todoItems?fields%5BtodoItems%5D=description,priority\"}" $1 $2
}

get_todoItem_tags() {
    attack "TodoItem_Tags" "{\"method\": \"GET\", \"url\": \"$BASE_URL/todoItems/1/tags\"}" $1 $2
}

post_tag() {
    attack "Tag" "{\"method\": \"POST\", \"url\": \"$BASE_URL/tags\", \"headers\": {\"Content-Type\": \"application/vnd.api+json\"}, \"body\": \"eyJkYXRhIjp7InR5cGUiOiJ0YWdzIiwiYXR0cmlidXRlcyI6eyJuYW1lIjogIlRoaXMgaXMgYW4gZXhhbXBsZSB0YWcifX19\"}" $1 $2
}

get_todoItem_includes 500 5
get_todoItems 100 5
get_todoItems_sort 100 5
get_todoItems_pagination 100 5
get_todoItems_filter 100 5
get_todoItems_fields 100 5
get_todoItem_tags 100 5
post_tag 100 5
