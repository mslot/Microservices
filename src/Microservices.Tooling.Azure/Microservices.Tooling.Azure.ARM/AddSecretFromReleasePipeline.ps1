$secretvalue = ConvertTo-SecureString 'AScriptSecret' -AsPlainText -Force
Set-AzureKeyVaultSecret -VaultName $env:KEYVAULTNAME -Name 'ScriptSecretv3' -SecretValue $secretvalue