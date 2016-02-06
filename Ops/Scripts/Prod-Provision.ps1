#https://github.com/meulta/OfficeTicTacToe/blob/development-0.0.16/azuredeploy.json

Param(  
    #Paramètres du Azure Ressource Group
    $resourceGroupeName = "ProdOfficeTicTacToe",
    $resourceGroupeDeploymentName = "ProdOfficeTicTacToeDeployed",
    $resourceLocation = "West Us",
    $publishSettingsFile = (Resolve-Path "C:\DEMOS\21 - CLOUD\AZURE\Azure Keys\stephgou - Internal.publishsettings"),
    $subscriptionName = "stephgou - Internal",
    $templateFile = "azuredeploy.json",
    $templateParameterFile = "prod-azuredeploy-parameters.json",
    $templateFolder = "..\templates",
    $tagName = "OfficeTicTacToe_RG",
    $tagValue = "Prod"
    )

#region init
Set-PSDebug -Strict

cls
$d = get-date
Write-Host "Starting Deployment $d"

$scriptFolder = Split-Path -Parent $MyInvocation.MyCommand.Definition
Write-Host "scriptFolder" $scriptFolder

set-location $scriptFolder
#endregion init

#region Souscription
#Remove-AzureAccount 
#Add-AzureAccount
#Get-AzureAccount
Select-AzureSubscription -SubscriptionName $subscriptionName
#endregion Souscription

Switch-AzureMode -Name AzureResourceManager

# Resource groupe create
New-AzureResourceGroup `
	-Name $resourceGroupeName `
	-Location $resourceLocation `
    -Tag @{Name=$tagName;Value=$tagValue} `
    -Verbose

# Resource group deploy
New-AzureResourceGroupDeployment `
    -Name $resourceGroupeDeploymentName `
	-ResourceGroupName $resourceGroupeName `
	-TemplateFile "$scriptFolder\$templatefolder\$templateFile" `
	-TemplateParameterFile "$scriptFolder\$templatefolder\$templateParameterFile" `
    -Verbose `
    #-StorageAccountName $storageAccountName `

$siteName = "prod-OfficeTicTacToe"
$testSlotName = "preview"

#Définition d'une règle de Ramp Up
$rule1 = New-Object Microsoft.WindowsAzure.Commands.Utilities.Websites.Services.WebEntities.RampUpRule
$rule1.ActionHostName = $testSlotName
$rule1.ReroutePercentage = 10;
$rule1.Name = "preview"

$rule1.ChangeIntervalInMinutes = 10;
$rule1.ChangeStep = 5;
$rule1.MinReroutePercentage = 5;
$rule1.MaxReroutePercentage = 50;

#Application de la règle de Ramp Up
Set-AzureWebsite $siteName -Slot Production -RoutingRules $rule1
$d = get-date
Write-Host "Stopping Deployment $d"