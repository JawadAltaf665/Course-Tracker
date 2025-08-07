FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/CourseTracker.Web.Host/CourseTracker.Web.Host.csproj", "src/CourseTracker.Web.Host/"]
COPY ["src/CourseTracker.Web.Core/CourseTracker.Web.Core.csproj", "src/CourseTracker.Web.Core/"]
COPY ["src/CourseTracker.Application/CourseTracker.Application.csproj", "src/CourseTracker.Application/"]
COPY ["src/CourseTracker.Core/CourseTracker.Core.csproj", "src/CourseTracker.Core/"]
COPY ["src/CourseTracker.EntityFrameworkCore/CourseTracker.EntityFrameworkCore.csproj", "src/CourseTracker.EntityFrameworkCore/"]
WORKDIR "/src/src/CourseTracker.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/CourseTracker.Web.Host", "src/CourseTracker.Web.Host"]
COPY ["src/CourseTracker.Web.Core", "src/CourseTracker.Web.Core"]
COPY ["src/CourseTracker.Application", "src/CourseTracker.Application"]
COPY ["src/CourseTracker.Core", "src/CourseTracker.Core"]
COPY ["src/CourseTracker.EntityFrameworkCore", "src/CourseTracker.EntityFrameworkCore"]
WORKDIR "/src/src/CourseTracker.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "CourseTracker.Web.Host.dll"]
