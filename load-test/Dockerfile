# Builder where we create our own Go library and Vegeta dependency.
FROM golang:latest AS builder
WORKDIR /app

COPY go.mod .
COPY go.sum .
COPY upload_results.go .

# Build our own uploader.
RUN CGO_ENABLED=0 GOOS=linux GOARCH=amd64 go build -o upload_results

# Build vegeta.
RUN CGO_ENABLED=0 GOOS=linux go get -u github.com/tsenart/vegeta

# The image we actually run
FROM alpine:latest  
RUN apk --no-cache add ca-certificates curl
WORKDIR /root/

# Copy in requirements
COPY --from=builder /app/upload_results /bin/upload_results
COPY --from=builder /go/bin/vegeta /bin/vegeta

COPY tag.json .
COPY test.sh .

ENTRYPOINT ["sh", "./test.sh"]
