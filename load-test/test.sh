echo "Waiting an arbitrary amount of time to ensure the database is ready to accept connections"
sleep 5

echo "Checking to see if webserver is ready"
i=0

while [ "$(curl --write-out %{http_code} --silent --output /dev/null http://app/articles)" != "200" ]; do 
    echo "Webserver not up yet."
    sleep 1
done

attack() {
    file_name="$i-$1_$2_$3.txt"
    file_path="./results/$file_name"

    echo "Running $file_name"
    echo "GET http://app:80/articles" | \
        vegeta attack -rate=$2 -duration=$3s | \
        tee results.bin | \
        vegeta report > $file_path
    
    i=$((i+1)) 
}

attack_post() {
    file_name="$i-$1_$2_$3.txt"
    file_path="./results/$file_name"

    echo "Running $file_name"
    echo "POST http://app:80/articles \\nContent-Type: application/vnd.api+json \\nAccept: application/vnd.api+json" | \
        vegeta attack -rate=$2 -duration=$3s -body $4 | \
        tee results.bin | \
        vegeta report > $file_path

    i=$((i+1)) 
}

warmup() {
    echo "Warmup phase started"
    attack "Warmup_GET_Empty" 1 10
    sleep 1
    attack "Warmup_GET_Empty" 5 10
    sleep 1
    attack "Warmup_GET_Empty" 10 10
    sleep 1
    echo "Warmup phase completed"
}

get_articles_empty() {
    attack "GET_Articles_Empty" $1 $2
}

create_articles() {
    attack_post "POST_Articles" $1 $2 "article.json"
}

get_articles_all() {
    attack "GET_Articles_All" $1 $2
}

warmup
get_articles_empty 20 10
get_articles_empty 50 10
create_articles 20 10
create_articles 50 10
get_articles_all 1 10

if [ "$UPLOAD_RESULTS" = "true" ]; then
    echo "Test complete. Uploading results."
    go run ./upload_results.go
else
    echo "Skipping results upload."
fi