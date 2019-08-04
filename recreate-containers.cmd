@echo off
@echo Removing containers...
docker-compose rm -v -f

@echo Restarting containers...
docker-compose up

@echo done!