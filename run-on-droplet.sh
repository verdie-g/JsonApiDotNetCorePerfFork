case ${DIGITAL_OCEAN_TOKEN-} in '') echo "$0: Environment variable DIGITAL_OCEAN_TOKEN is not set" >&2; exit 1;; esac

export UPLOAD_RESULTS="true"
docker-machine create --driver digitalocean --digitalocean-access-token $DIGITAL_OCEAN_TOKEN docker-sandbox

docker-machine ls

# connect to the machine
docker-machine env docker-sandbox
eval $(docker-machine env docker-sandbox)

# verify we are connected (ACTIVE should have an *)
docker-machine ls

docker-compose -f stack.yml build
docker-compose -f stack.yml up

