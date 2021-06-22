#!/usr/bin/env sh

echo "Waiting an arbitrary amount of time to ensure the database is ready to accept connections"
sleep 30

echo "Checking to see if webserver is ready"
i=001

while [ "$(curl --write-out %{http_code} --silent --output /dev/null http://app/todoItems)" != "200" ]; do
    echo "Webserver not up yet."
    sleep 1
done

results_dir="./results"

attack_get() {
    prefix=$(printf "%03d" $i)
    file_name="$prefix-GET_$1_$3_$4.txt"
    file_path="$results_dir/$file_name"
    url="http://app$2"

    echo "Running $file_name"
    echo "GET $url" | \
        vegeta attack -rate=$3 -duration=$4s | \
        tee results.bin | \
        vegeta report > $file_path

    chmod go+w $file_path
    i=$((i+1))
}

attack_post() {
    prefix=$(printf "%03d" $i)
    file_name="$prefix-POST_$1_$3_$4.txt"
    file_path="$results_dir/$file_name"
    url="http://app$2"

    echo "Running $file_name"
    echo "POST $url \\nContent-Type: application/vnd.api+json" | \
        vegeta attack -rate=$3 -duration=$4s -body $5 | \
        tee results.bin | \
        vegeta report > $file_path

    chmod go+w $file_path
    i=$((i+1))
}

get_todoItems() {
    attack_get "Warmup" "/todoItems" $1 $2
}

get_todoItems_sort() {
    attack_get "TodoItems_Sort" "/todoItems?sort=-id" $1 $2
}

get_todoItem_includes() {
    attack_get "TodoItem_Includes" "/todoItems/1?include=owner,assignee,tags" $1 $2
}

get_todoItems_pagination() {
    attack_get "TodoItems_Pagination" "/todoItems?page%5Bsize%5D=5&page%5Bnumber%5D=8" $1 $2
}

get_todoItems_filter() {
    attack_get "TodoItems_Filter" "/todoItems?filter=and(equals(priority,%27High%27),equals(owner.firstName,%27Debbi%27))" $1 $2
}

get_todoItems_fields() {
    attack_get "TodoItems_Fields" "/todoItems?fields%5BtodoItems%5D=description,priority" $1 $2
}

get_todoItem_tags() {
    attack_get "TodoItem_Tags" "/todoItems/1/tags" $1 $2
}

post_tag() {
    attack_post "Tag" "/tags" $1 $2 "tag.json"
}

warmup() {
    echo "Warmup phase started"
    get_todoItems 1 10
    sleep 1
    get_todoItems 10 10
    sleep 1
    get_todoItems 50 10
    sleep 1
    echo "Warmup phase completed"
}

warmup
get_todoItems_sort 50 10
get_todoItem_includes 50 10
get_todoItems_pagination 50 10
get_todoItems_filter 50 10
get_todoItems_fields 50 10
get_todoItem_tags 50 10
post_tag 50 10

if [ "$UPLOAD_RESULTS" = "true" ]; then
    echo "Test complete. Writing summary and uploading results."
else
    echo "Test complete. Writing summary and skipping results upload."
fi
summarize_upload_results
chmod go+w "$results_dir/summary.md"

echo "Signaling test completion."
touch "$results_dir/test-completed-signal"
chmod go+w "$results_dir/test-completed-signal"
