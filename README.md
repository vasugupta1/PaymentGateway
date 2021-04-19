# PaymentGateway

API which offers 2 endpoints that can be used an merchants :
1) Payment Processing Endpoint which is used to validated and process card details of the user
2) Payment Retrieval Endpoint which is used to retrieve previous payments.

# Prequisties
* .Net Core SDK 3.1
* Docker 

# Authentication
Username : admin , Password : adminpassword

# Swagger
* Payment Gateway : https://localhost:44396/swagger

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
  "amount": 0
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
  "amount": 0
}

# Validation Rules 
* Currency Code : must be length 3
* Expiry Month : must be in range of 1 -> 12
* Expiry Year : must be in range of 2021 -> 2999
* CVV : must be in range of 1 -> 999
* Card Number : must have min length of 1 and max length of 19
* Amount : must be in range of 0.0 -> 1.79769313486232E+308

# Logging 
Kibana : http://localhost:5601/app/kibana
Elasticsearch : http://localhost:9200

# CI 
https://github.com/vasugupta1/PaymentGateway/actions

# Storage 
Redis : https://hub.docker.com/r/bitnami/redis/
Connection String : 127.0.0.1:6379

  
