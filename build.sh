workspace=`pwd`
echo 'workspace: ${workspace}'

echo "start build asp.net core"
docker run --rm \
-v ~/.dotnet:/root/.dotnet \
-v ~/.nuget:/root/.nuget  \
-v ${workspace}:/src \
--workdir /src dukecheng/aspnetcore:aspnetcore-sdk-2.2.100 bash -c "dotnet restore ./niusys_mock_server.sln && rm -rf ./MockServer/obj/Docker/publish && dotnet publish ./MockServer/MockServer.csproj -c Release -o ./obj/Docker/publish"

echo "current dir: `pwd`"
mkdir -p ./buildreport
env >  ./buildreport/env.txt
echo "Image Version: ${imagesNames[number]}:${bulldversion}
    GIT COMMIT: $GIT_COMMIT
    GIT_PREVIOUS_COMMIT:$GIT_PREVIOUS_COMMIT
    GIT_PREVIOUS_SUCCESSFUL_COMMIT:$GIT_PREVIOUS_SUCCESSFUL_COMMIT
    GIT_BRANCH:$GIT_BRANCH
    GIT_LOCAL_BRANCH:$GIT_LOCAL_BRANCH
    GIT_URL:$GIT_URL
    GIT_COMMITTER_NAME:$GIT_COMMITTER_NAME
    GIT_AUTHOR_NAME:$GIT_AUTHOR_NAME
    GIT_COMMITTER_EMAIL:$GIT_COMMITTER_EMAIL
    GIT_AUTHOR_EMAIL:$GIT_AUTHOR_EMAIL
    " > ./buildreport/buildversion.txt
