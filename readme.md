# Commands

Build with jekyll docker container

## Develop

    ./Build.ps1 -Watch

`-Watch` auto-regenerates on changes.

It serves on `http://localhost:4000/`.

## Build for deployment

    ./Build.ps1

Builds into `_site` to publish with `./Update-Homepage.ps1`.
