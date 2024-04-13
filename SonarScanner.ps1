dotnet sonarscanner begin /k:"ReferenceApi" /d:sonar.login="sqp_6ae70378a9e5ee382a658c5cd89e0ad86ae2f8bd" /d:sonar.cs.opencover.reportsPaths="./Reference.Api.Test/coverage.xml"

dotnet build --no-incremental

coverlet ./Reference.Api.Test/bin/Debug/net8.0/Reference.Api.Test.dll  --target "dotnet" --targetargs "test --no-build" --format opencover --output "./Reference.Api.Test/coverage.xml"

dotnet sonarscanner end /d:sonar.login="sqp_6ae70378a9e5ee382a658c5cd89e0ad86ae2f8bd"
