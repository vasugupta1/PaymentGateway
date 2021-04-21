#GET SDK FROM DOCKER HUB
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

EXPOSE 80

COPY src/PaymentGateway.API/PaymentGateway.API.csproj /build/PaymentGateway.API/
COPY src/PaymentGateway.Common/PaymentGateway.Common.csproj /build/PaymentGateway.Common/
COPY src/PaymentGateway.Services/PaymentGateway.Services.csproj /build/PaymentGateway.Services/

RUN dotnet restore ./build/PaymentGateway.API/PaymentGateway.API.csproj 
RUN dotnet restore ./build/PaymentGateway.Common/PaymentGateway.Common.csproj 
RUN dotnet restore ./build/PaymentGateway.Services/PaymentGateway.Services.csproj 

COPY src/PaymentGateway.API/. ./build/PaymentGateway.API/
COPY src/PaymentGateway.Common/. ./build/PaymentGateway.Common/
COPY src/PaymentGateway.Services/. ./build/PaymentGateway.Services/

WORKDIR /build/

RUN dotnet build ./PaymentGateway.API/PaymentGateway.API.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

RUN dotnet publish ./PaymentGateway.API/PaymentGateway.API.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime

WORKDIR /app
COPY --from=build /build/out .


ENTRYPOINT [ "dotnet", "PaymentGateway.API.dll" ]


