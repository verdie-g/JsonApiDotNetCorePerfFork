package main

import (
	"context"
	"fmt"
	"io/ioutil"
	"os"
	"path/filepath"
	"strings"

	"github.com/google/go-github/v35/github"
	"golang.org/x/oauth2"
)

func main() {
	content := ""
	println("looking for files")
	files, _ := filepath.Glob("./results/*.txt")

	for _, file := range files {
		println("Reading " + file)
		title := getTitle(file)
		content += ("\n\n" + title + " \n\n")
		content += readFile(file)
	}

	uploadFile(content)
}

func getTitle(fileName string) string {
	removedExtension := fileName[:len(fileName)-4] // remove .txt file extension
	split := strings.Split(removedExtension, "_")

	last := len(split) - 1
	duration := split[last]
	rps := split[last-1]
	rem := strings.Join(split[:last-1], " ")

	title := fmt.Sprintf("%s %s RPS for %s seconds", rem, rps, duration)

	return title
}

func readFile(path string) string {
	fileContent, err := ioutil.ReadFile(path)
	if err != nil {
		fmt.Print(err)
	}

	body := string(fileContent)
	body = strings.Replace(body, "\n", " | \n|", -1)
	body = strings.Replace(body, " [", " | [", -1)
	body = strings.Replace(body, "] ", "] | ", -1)

	header := "| Measure | Key |  Result | \n |---|---|---| \n | "
	body = header + body
	body = removeTrailingPipe(body)
	return body
}

func removeTrailingPipe(str string) string {
	if last := len(str) - 1; last >= 0 && str[last] == '|' {
		str = str[:last]
	}
	return str
}

func uploadFile(body string) {

	ctx := context.Background()
	ts := oauth2.StaticTokenSource(
		&oauth2.Token{AccessToken: os.Getenv("GH_ACCESS_TOKEN")},
	)
	tc := oauth2.NewClient(ctx, ts)
	client := github.NewClient(tc)

	input := &github.IssueRequest{
		Title:  github.String(os.Getenv("GH_ISSUE_TITLE")),
		Body:   github.String(body),
		Labels: &[]string{os.Getenv("GH_LABEL")},
	}

	println("Uploading Issue...")
	_, _, err := client.Issues.Create(ctx, os.Getenv("GH_RESULTS_ORG"), os.Getenv("GH_RESULTS_REPO"), input)
	if err != nil {
		fmt.Print(err)
	}
}
