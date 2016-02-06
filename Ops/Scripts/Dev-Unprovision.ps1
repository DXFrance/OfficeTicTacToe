Param(  
    # Name of the subscription to use for azure cmdlets
    $subscriptionName = "stephgou - Internal",
    $subscriptionId = "b1256985-d559-406d-a0ca-f47d72fed1e2",
    #Paramètres du Azure Ressource Group
    $resourceGroupeName = "OfficeTicTacToe-RM-Dev"
    )

#region init
Set-PSDebug -Strict

cls
$d = get-date
Write-Host "Starting Unprovisioning $d"

$scriptFolder = Split-Path -Parent $MyInvocation.MyCommand.Definition
Write-Host "scriptFolder" $scriptFolder

set-location $scriptFolder
#endregion init

#Login-AzureRmAccount -SubscriptionId $subscriptionId

# Resource groupe create
Remove-AzureRmResourceGroup -Name $resourceGroupeName

$d = get-date
Write-Host "Stopping Unprovisioning $d"


