#!/bin/zsh

echo "🚀 Starting Render Deployment..."

# Check if git is initialized
if [ ! -d ".git" ]; then
    echo "📦 Initializing Git repository..."
    git init
    git add .
    git commit -m "Initial commit for Render deployment"
fi

# Create GitHub repository and push code
echo "🌐 To deploy to Render, you need to:"
echo "1. Create a GitHub repository at https://github.com/new"
echo "2. Push your code to GitHub:"
echo "   git remote add origin <your-github-repo-url>"
echo "   git push -u origin master"
echo "3. Go to https://dashboard.render.com"
echo "4. Click 'New +' and select 'Web Service'"
echo "5. Connect your GitHub repository"
echo "6. Render will automatically detect your configuration"

echo "✨ Would you like to continue? (y/n)"
read answer

if [[ $answer =~ ^[Yy]$ ]]; then
    echo "Please enter your GitHub repository URL:"
    read repo_url
    
    if [ ! -z "$repo_url" ]; then
        git remote add origin $repo_url
        git push -u origin master
        echo "✅ Code pushed to GitHub!"
        echo "🌐 Now go to https://dashboard.render.com to complete the deployment"
    else
        echo "❌ No repository URL provided"
    fi
fi

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
