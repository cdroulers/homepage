[CmdletBinding()]
PARAM(
  [switch]
  $Watch
)

$params = @();
if ($Watch) {
  $params += "serve";
  $params += "--watch";
  $params += "--force_polling";
  $params += "--incremental";
} else {
  $params += "build";
}

docker run --rm --volume="$($PSScriptRoot):/srv/jekyll" -p 4000:4000 -it jekyll/jekyll:3 jekyll @params;