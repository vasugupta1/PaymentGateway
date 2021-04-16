#GET SDK FROM DOCKER HUB
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0
 
COPY src/PaymentGateway.API/PaymentGateway.API.csproj /build/PaymentGateway.API/
COPY src/PaymentGateway.Common/PaymentGateway.Common.csproj /build/PaymentGateway.Common/
COPY src/PaymentGateway.Services/PaymentGateway.Services.csproj /build/PaymentGateway.Services/

RUN dotnet restore ./build/PaymentGateway.API/PaymentGateway.API.csproj 
RUN dotnet restore ./build/PaymentGateway.Common/PaymentGateway.Common.csproj 
RUN dotnet restore ./build/PaymentGateway.Services/PaymentGateway.Services.csproj 

COPY . ./build/
WORKDIR /build/

RUN dotnet build ./PaymentGateway.API/PaymentGateway.API.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

RUN dotnet publish ./PaymentGateway.API/PaymentGateway.API.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/runtime:3.1

WORKDIR /app
COPY --from=build /build/out .


ENTRYPOINT [ "dotnet", "PaymentGateway.API.dll" ]


