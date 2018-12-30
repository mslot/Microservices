$webapp=Set-AzureRmWebApp -AssignIdentity $true -Name microservices-webapi-$env:ENVIRONMENT -ResourceGroupName microservices-$env:ENVIRONMENT-rg

Set-AzureRmKeyVaultAccessPolicy -VaultName $env:KEYVAULTNAME-$env:ENVIRONMENT -ObjectId $webapp.Identity.PrincipalId -PermissionsToKeys get,list -PermissionsToSecrets get,list

#This is the same as above, just with the az command
#$json=az webapp identity assign --name "microservices-webapi-$env:ENVIRONMENT" --resource-group "microservices-$env:ENVIRONMENT-rg"
#$parsedObject=$json | ConvertFrom-Json
#az keyvault set-policy --name '$env:KEYVAULTNAME-$env:ENVIRONMENT' --object-id $parsedObject.principalId --secret-permissions get list