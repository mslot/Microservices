$secretvalue = ConvertTo-SecureString 'AScriptSecret' -AsPlainText -Force
Set-AzureKeyVaultSecret -VaultName $env:KEYVAULT_NAME -Name 'ScriptSecretv3' -SecretValue $secretvalue