#GET SDK FROM DOCKER HUB
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

EXPOSE 90

COPY Bank/Bank.csproj /build/Bank/

RUN dotnet restore ./build/Bank/Bank.csproj 

COPY Bank/. ./build/Bank/

WORKDIR /build/

RUN dotnet build ./Bank/Bank.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

RUN dotnet publish ./Bank/Bank.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime

WORKDIR /app
COPY --from=build /build/out .


ENTRYPOINT [ "dotnet", "Bank.dll" ]


