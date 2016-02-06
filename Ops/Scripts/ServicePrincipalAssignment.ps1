Param(  

    # Azure publish settings file to import
    #$publishSettingsFile = Resolve-Path "$(throw 'Not implemented yet')"
    $publishSettingsFile = (Resolve-Path "C:\DEMOS\21 - CLOUD\AZURE\Azure Keys\stephgou - Internal.publishsettings"),

    # Name of the subscription to use for azure cmdlets
    $subscriptionName = "stephgou - Internal",
    $subscriptionId = "b1256985-d559-406d-a0ca-f47d72fed1e2",
    $tenantId = "72f988bf-86f1-41af-91ab-2d7cd011db47",
    $ServicePrincipalName = "http://stephgou-spn"

    )
#endregion Parameters

Set-PSDebug -Strict

cls
$d = get-date
Write-Host "Starting Deployment $d"

#region Setup

$scriptFolder = Split-Path -Parent $MyInvocation.MyCommand.Definition
Write-Host "scriptFolder" $scriptFolder

set-location $scriptFolder
$resolvePath = "$scriptFolder\$configScript"



#Login-AzureRmAccount -SubscriptionId $subscriptionId -TenantId $tenantId

#
#Import-AzurePublishSettingsFile -PublishSettingsFile $publishSettingsFile
#Select-AzureSubscription -SubscriptionName $subscriptionName

# Trying to change default tenantID without any success
Get-AzureSubscription
$subscription = Get-AzureSubscription | where { $_.SubscriptionId -eq $subscriptionId }
#$subscription.TenantId = $tenantId
Get-AzureSubscription
Select-AzureSubscription -SubscriptionName $subscriptionName
$subscription = Get-AzureSubscription | where { $_.SubscriptionId -eq $subscriptionId }

$tenantId = (Get-AzureAccount).ActiveDirectories.ActiveDirectoryTenantID

New-AzureRmRoleAssignment -ServicePrincipalName $ServicePrincipalName `
                          -RoleDefinitionName Contributor

$d = get-date
Write-Host "Stopping Deployment $d"
