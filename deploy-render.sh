#!/bin/zsh

# Colors for output
GREEN='\033[0;32m'
NC='\033[0m'

echo "${GREEN}1. Building the application...${NC}"
dotnet publish -c Release

echo "${GREEN}2. Deploying to Render...${NC}"
if ! command -v render &> /dev/null; then
    echo "Installing Render CLI..."
    brew tap render-oss/render
    brew install render
fi

echo "${GREEN}3. Logging in to Render...${NC}"
render login

echo "${GREEN}4. Deploying the application...${NC}"
render deploy

echo "${GREEN}Deployment Complete!${NC}"
echo "Your API will be available at the URL shown above once the deployment is finished."
