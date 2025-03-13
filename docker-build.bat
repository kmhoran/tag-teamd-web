@echo off

setlocal EnableDelayedExpansion

dotnet restore .\WebApi\
dotnet build .\WebApi\

docker buildx build --push --platform linux/amd64 --tag kmhoran/tag-teamd-web .

@REM -- UNCOMMENT TO DEBUG BUILD --
@REM docker build --tag "docker-file-debug_20240511" --progress plain --no-cache .

EXIT
