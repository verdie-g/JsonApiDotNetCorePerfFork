FROM golang:latest AS go
WORKDIR /app

RUN go get -u github.com/tsenart/vegeta
RUN go get "golang.org/x/oauth2"
RUN go get "github.com/google/go-github/github"

COPY test.sh .
COPY upload_results.go .
COPY article.json .

ENTRYPOINT ["sh", "./test.sh"]
