param location string = resourceGroup().location
param clusterName string = 'auctionhouseAksCluster'
param nodeCount int = 2
param nodeSize string = 'Standard_DS2_v2'

resource aksCluster 'Microsoft.ContainerService/managedClusters@2023-03-01' = {
  name: clusterName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  sku: {
    name: 'Base'
    tier: 'Free'
  }
  properties: {
    agentPoolProfiles: [
      {
        name: 'nodepool1'
        count: nodeCount
        vmSize: nodeSize
        osType: 'Linux'
        mode: 'System'
      }
    ]
    dnsPrefix: clusterName
    enableRBAC: true
  }
}

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-01-01-preview' = {
  name: 'nikolaiauctionhouseprojectregistry'
  location: location
  sku: {
    name: 'Basic' // Options: Basic, Standard, Premium
  }
  properties: {
    adminUserEnabled: false // Recommended for security
  }
}

resource aksAcrRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(aksCluster.id, 'AcrPull') // Unique name for the role assignment
  scope: containerRegistry
  properties: {
    principalId: aksCluster.identity.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d') // AcrPull role
  }
}

output acrLoginServer string = containerRegistry.properties.loginServer
output aksClusterIdentity string = aksCluster.identity.principalId
