$json=az webapp identity assign --name "microservices-webapi-$env:ENVIRONMENT" --resource-group "microservices-$env:ENVIRONMENT-rg"
$parsedObject=$json | ConvertFrom-Json
az keyvault set-policy --name '$env:KEYVAULTNAME-$env:ENVIRONMENT' --object-id $parsedObject.principalId --secret-permissions get list