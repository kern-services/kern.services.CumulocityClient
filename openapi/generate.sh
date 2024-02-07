# Get directory of this script
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"

# Get parent directory
PARENT_DIR="$(dirname "$DIR")"

docker run --rm \
    -v $PARENT_DIR:/local openapitools/openapi-generator-cli generate \
    -i /local/openapi/c8y-oas-10.15.0.json \
    -g csharp \
    --additional-properties=netCoreProjectFile=true,packageName=kern.services.CumulocityClient,packageVersion=10.15.1,targetFramework=net8.0 \
    -o /local/
