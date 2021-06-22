# Builder where we create our own Go library and Vegeta dependency.
FROM golang:latest AS builder
WORKDIR /app

COPY go.mod .
COPY summarize_upload_results.go .

# Replace Github dependency with latest version.
RUN go mod tidy

# Build our results uploader.
RUN CGO_ENABLED=0 GOOS=linux GOARCH=amd64 go build -o summarize_upload_results

# Build vegeta.
RUN CGO_ENABLED=0 GOOS=linux go get -u github.com/tsenart/vegeta

# The image we actually run
FROM alpine:latest  
RUN apk --no-cache add ca-certificates curl
WORKDIR /root/

# Copy in requirements
COPY --from=builder /app/summarize_upload_results /bin/summarize_upload_results
COPY --from=builder /go/bin/vegeta /bin/vegeta

COPY tag.json .
COPY test.sh .

ENTRYPOINT ["sh", "./test.sh"]
