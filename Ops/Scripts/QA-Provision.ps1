﻿#https://github.com/meulta/OfficeTicTacToe/blob/development-0.0.16/azuredeploy.json

Param(  
    #Paramètres du Azure Ressource Group
    $resourceGroupeName = "QAOfficeTicTacToe",
    $resourceGroupeDeploymentName = "QAOfficeTicTacToeDeployed",
    $resourceLocation = "West Us",
    $publishSettingsFile = (Resolve-Path "C:\DEMOS\21 - CLOUD\AZURE\Azure Keys\stephgou - Internal.publishsettings"),
    $subscriptionName = "stephgou - Internal",
    $templateFile = "azuredeploy.json",
    $templateParameterFile = "qa-azuredeploy-parameters.json",
    $templateFolder = "..\templates",
    $tagName = "OfficeTicTacToe_RG",
    $tagValue = "QA"
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


$d = get-date
Write-Host "Stopping Deployment $d"