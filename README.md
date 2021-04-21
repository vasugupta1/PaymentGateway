# PaymentGateway

This api offers 2 endpoints that can be used by merchants :
1) Payment Processing Endpoint which is used to validated and process card details of the user
2) Payment Retrieval Endpoint which is used to retrieve previous payments.

# Prequisties
* .Net Core SDK 3.1
* Docker 
* Docker Compose
# How to run
Use command 'docker compose up --build' at the root of the directory to start up the containers

# Authentication
* Username : admin , Password : adminpassword
* Username : user1 , Password : password

# Swagger
* Payment Gateway : http://localhost:8080/swagger/index.html

# Requests

## Make payment ##
Request :
curl -X 'POST' \
  'https://localhost:8080/api/v1/PaymentProcessor' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "currencyCode": "str",
  "expiryMonth": 12,
  "expiryYear": 2999,
  "cvv": 999,
  "cardNumber": "string",
  "amount": 200
}'
Response : 
{
  "paymentId": "c22805f0-17df-4de0-9e8b-2c15e6492ba4",
  "success": true,
  "message": "The payment has been accepted by the bank"
}


## Retrieve payment ##
Request:
curl -X 'GET' \
  'https://localhost:8080/api/v1/PaymentRetrieval/payment-retrieval/pamentid' \
  -H 'accept: text/plain'
Response:
{
  "currencyCode": "GBP",
  "expiryMonth": 12,
  "expiryYear": 2999,
  "cvv": 999,
  "cardNumber": "123123123",
  "amount": 200
}

# Validation Rules 
* Currency Code : must be length 3
* Expiry Month : must be in range of 1 -> 12
* Expiry Year : must be in range of 2021 -> 2999
* CVV : must be in range of 1 -> 999
* Card Number : must have min length of 1 and max length of 19
* Amount : must be in range of 0.0 -> 1.79769313486232E+308

* This validation can be found under PaymentGateway/src/PaymentGateway.API/Models/Validators/PaymentProcessingRequestValidator.cs 


# Logging 
Kibana : http://localhost:5601/app/kibana
Elasticsearch : http://localhost:9200

# CI 
https://github.com/vasugupta1/PaymentGateway/actions

# Storage 
Redis : https://hub.docker.com/r/bitnami/redis/

# WebApi
https://hub.docker.com/repository/docker/vasugupta1/webapi

# Built With
* .Net Core SDK
* Swagger
* Refit
* FluentValidations.AspNetCore
* OneOf
* StackExchange.Redis
* Serilog.Sinks.Elasticsearch
* Microsoft.AspNetCore.Mvc.Versioning
* System.Text.Json
* AutoFixture
* FluentAssertions
* Moq
* Nunit

# Assumptions 
* The fake bank api is created in .netcore and has a docker file which can be used to build 
* I wasn't able to host the bank api in the same docker compose hence I had to mock the reponse of the api 
* All of the payment processing requests are stored in redis and can be connected via localhost:6379

# ToDo
* Find a way to host the two apis within the same docker-compose.
* Add data encryption, usign RSA or other encryption schemes
* Implement a better user service which will contain all the clients usernames and passwords which should have access to this api
* Add Grafana support for application metrics
* Intergrate Auth0 for JWT authentication 

