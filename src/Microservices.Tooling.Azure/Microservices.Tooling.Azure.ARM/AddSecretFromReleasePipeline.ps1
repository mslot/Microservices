$secretvalue = ConvertTo-SecureString 'AScriptSecret' -AsPlainText -Force
Set-AzureKeyVaultSecret -VaultName 'microservices-vault-test' -Name 'ScriptSecretv3' -SecretValue $secretvalue