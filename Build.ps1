[CmdletBinding()]
PARAM(
  [switch]
  $Watch
)

$params = @();
if ($Watch) {
  $params += "--watch";
  $params += "--force_polling";
  $params += "--incremental";
}

docker run --rm --volume="$($PSScriptRoot):/srv/jekyll" -it jekyll/jekyll:3 jekyll build @params;