#https://github.com/meulta/OfficeTicTacToe/blob/development-0.0.16/azuredeploy.json

Param(  
    #Paramètres du Azure Ressource Group
    $subscriptionName = "stephgou - Internal",
    $subscriptionId = "b1256985-d559-406d-a0ca-f47d72fed1e2",
    $resourceGroupeName = "OfficeTicTacToe-PS-QA",
    $resourceGroupeDeploymentName = "QAOfficeTicTacToeDeployed",
    $resourceLocation = "West Us",
    $publishSettingsFile = (Resolve-Path "C:\DEMOS\21 - CLOUD\AZURE\Azure Keys\stephgou - Internal.publishsettings"),
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

#Login-AzureRmAccount -SubscriptionId $subscriptionId

# Resource groupe create
# Resource groupe create
New-AzureRmResourceGroup `
	-Name $resourceGroupeName `
	-Location $resourceLocation `
    -Tag @{Name=$tagName;Value=$tagValue} `
    -Verbose

# Resource group deploy
New-AzureRmResourceGroupDeployment `
    -Name $resourceGroupeDeploymentName `
	-ResourceGroupName $resourceGroupeName `
	-TemplateFile "$scriptFolder\$templatefolder\$templateFile" `
	-TemplateParameterFile "$scriptFolder\$templatefolder\$templateParameterFile" `
    -Verbose `
    #-StorageAccountName $storageAccountName `

$d = get-date
Write-Host "Stopping Deployment $d"