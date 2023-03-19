param rgLocation string = resourceGroup().location


//https://learn.microsoft.com/en-us/azure/templates/microsoft.operationalinsights/workspaces

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
    name: 'polling-log-analytics'
    location: rgLocation
    properties: {
        retentionInDays: 30
        sku: {
            name: 'PerGB2018'
        }
    }
}

// https://learn.microsoft.com/en-us/azure/templates/microsoft.insights/components

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
    name: 'polling-appInsights'
    location: rgLocation
    kind: 'web'
    properties: {
        Application_Type: 'web'
        WorkspaceResourceId: logAnalytics.id
    }
}

// https://learn.microsoft.com/en-us/azure/templates/microsoft.storage/storageaccounts

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
    name: 'pollingstorage'
    location: rgLocation
    kind: 'StorageV2'
    sku: {
        name: 'Standard_LRS'
    }
}

// https://medium.com/codex/publish-azure-static-web-apps-using-a-bicep-template-ca315a825b74
// https://learn.microsoft.com/en-us/azure/templates/microsoft.web/staticsites

resource staticWebApp 'Microsoft.Web/staticSites@2022-03-01' existing = {
    name: 'StaticWebAppBlazorPolling'
}

// https://learn.microsoft.com/en-us/azure/templates/microsoft.web/staticsites/config-appsettings

resource staticWebAppConfig 'Microsoft.Web/staticSites/config@2022-03-01' = {
    name: 'appsettings'
    parent: staticWebApp
    properties: {
        AzureWebJobsStorage: storageAccount.listKeys().keys[0].value
    }
}