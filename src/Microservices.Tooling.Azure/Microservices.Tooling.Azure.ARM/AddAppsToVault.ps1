$webapp=Set-AzWebApp -AssignIdentity $true -Name microservices-webapi-$env:ENVIRONMENT -ResourceGroupName microservices-$env:ENVIRONMENT-rg

Set-AzureRmKeyVaultAccessPolicy -VaultName $env:KEYVAULTNAME-$env:ENVIRONMENT -ServicePrincipalName $webapp.Identity.PrincipalId -PermissionsToKeys get,list


#$json=az webapp identity assign --name "microservices-webapi-$env:ENVIRONMENT" --resource-group "microservices-$env:ENVIRONMENT-rg"
#$parsedObject=$json | ConvertFrom-Json
#az keyvault set-policy --name '$env:KEYVAULTNAME-$env:ENVIRONMENT' --object-id $parsedObject.principalId --secret-permissions get list