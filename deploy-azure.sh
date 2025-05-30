#!/bin/zsh

# Variables
resourceGroup="PlayerManagementRG"
location="eastus"
appName="player-management-api"

# Colors for output
GREEN='\033[0;32m'
NC='\033[0m'

echo "${GREEN}1. Creating Azure Resources...${NC}"
# Create Resource Group
az group create --name $resourceGroup --location $location

# Create App Service Plan
echo "${GREEN}2. Creating App Service Plan...${NC}"
az appservice plan create --name "$appName-plan" \
    --resource-group $resourceGroup \
    --sku FREE \
    --is-linux

# Create Web App
echo "${GREEN}3. Creating Web App...${NC}"
az webapp create --name $appName \
    --resource-group $resourceGroup \
    --plan "$appName-plan" \
    --runtime "DOTNET:7.0"

# Build and Publish
echo "${GREEN}4. Building and Publishing the API...${NC}"
cd /Volumes/Workspace/API/PlayerManagementAPI
dotnet publish -c Release

# Create zip file
echo "${GREEN}5. Creating deployment package...${NC}"
cd bin/Release/net9.0/publish
zip -r publish.zip .

# Deploy to Azure
echo "${GREEN}6. Deploying to Azure...${NC}"
az webapp deployment source config-zip --src publish.zip \
    --resource-group $resourceGroup \
    --name $appName

echo "${GREEN}Deployment Complete!${NC}"
echo "Your API is now available at: https://$appName.azurewebsites.net"
