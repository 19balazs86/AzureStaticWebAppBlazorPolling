# Install the Azure PowerShell module
# https://docs.microsoft.com/en-us/powershell/azure/install-az-ps

# Sign in
# Connect-AzAccount

param(
    [string] $resGroupName = "StaticWebAppBlazorPolling-ResGroup",
    [string] $location     = "North Europe"
)

# --> Create: ResourceGroup
Get-AzResourceGroup -Name $resGroupName -ErrorVariable notPresent -ErrorAction SilentlyContinue | Out-Null

if ($notPresent)
{
    Write-Host "Creating resource group..."

    New-AzResourceGroup -Name $resGroupName -Location $location
}
else
{
    Write-Host "Already exists:" $resGroupName
}