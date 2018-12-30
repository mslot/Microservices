$secretvalue = ConvertTo-SecureString 'AScriptSecret' -AsPlainText -Force
Set-AzureKeyVaultSecret -VaultName $env:KEYVAULTNAME-$env:ENVIRONMENT -Name 'ScriptSecretv3' -SecretValue $secretvalue