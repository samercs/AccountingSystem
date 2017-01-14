
$rootDirectory = Resolve-Path ..\.
$solutionName = (Get-Item $rootDirectory).Name

$solutionFile = Get-ChildItem -Path $rootDirectory | where {$_.extension -eq ".sln"} | Select-Object -First 1
$sourceFiles = Get-ChildItem -Path $rootDirectory -Include @("*.sln", "*.suo", "*.csproj", "*.nuspec", "*.cs", "*.config", "*.asax", "*.cshtml") -Recurse -Force
$projectFiles = Get-ChildItem -Path $rootDirectory -Include @("*.csproj") -Recurse -Force
$originalNamespace = $solutionFile.BaseName

Write-Host "root $rootDirectory";

# Replace Root Namespace in Files

Write-Host "Updating namespaces..."

foreach ($sourceFile in $sourceFiles)
{
   Write-Host "Updating $sourceFile..."
   (Get-Content $sourceFile) | Foreach-Object { $_ -Replace $originalNamespace, $solutionName } | Set-Content $sourceFile
}

# Rename Project Files

Write-Host "Renaming project files..."

foreach ($projectFile in $projectFiles)
{
    $projectFileName = $projectFile.Name
    $newProjectFileName = $projectFileName.Replace($originalNamespace, $solutionName)

    Write-Host "Renaming file $projectFileName to $newProjectFileName..."

    if ($projectFileName -ne $newProjectFileName) 
    {
        Rename-Item $projectFile $newProjectFileName
    }
}

# Rename Project Directories

Write-Host "Renaming project directories..."

foreach ($projectFile in $projectFiles)
{
    $projectDirectory = $projectFile.Directory
    $projectDirectoryName = $projectFile.Directory.Name
    $newProjectDirectoryName = $projectDirectoryName.Replace($originalNamespace, $solutionName)

    if ($projectDirectoryName -ne $newProjectDirectoryName) 
    {
        Write-Host "Renaming directory $projectDirectoryName to $newProjectDirectoryName..."

        Rename-Item $projectDirectory $newProjectDirectoryName
    }    
}

# Rename Solution File

Write-Host "Renaming solution file..."

$solutionFile | Rename-Item -NewName { $_.Name –replace $originalNamespace, $solutionName }

# Update web.config

#Write-Host "Setting up web.config, please enter the following settings:"

#$appSettings = @{}

#$appSettings.SiteTitle = Read-Host "SiteTitle"
#$appSettings.ProjectKey = Read-Host "ProjectKey"
#$appSettings.ProjectToken = Read-Host "ProjectToken"
#$appSettings.EmailSender = Read-Host "EmailSender"
#$appSettings.AzureCdnHost = Read-Host "AzureCdnHost"
#$appSettings.StorageAccountName = Read-Host "StorageAccountName"
#$appSettings.StorageAccountKey = Read-Host "StorageAccountKey"

#$webConfigPath = Get-ChildItem -Path $rootDirectory -Recurse -Force | where {$_.Name -eq "web.config"} | Select-Object -First 1

#$doc = (Get-Content $webConfigPath.FullName) -as [Xml]
#$obj = $doc.configuration.appSettings.add | where {$_.Key -eq 'SiteTitle'}
#$obj.value = ""+$appSettings.SiteTitle

#$obj = $doc.configuration.appSettings.add | where {$_.Key -eq 'OrangeJetpack.Services:Project.Key'}
#$obj.value = ""+$appSettings.ProjectKey

#$obj = $doc.configuration.appSettings.add | where {$_.Key -eq 'OrangeJetpack.Services:Project.Token'}
#$obj.value = ""+$appSettings.ProjectToken

#$obj = $doc.configuration.appSettings.add | where {$_.Key -eq 'OrangeJetpack.Services:Email.Sender'}
#$obj.value = ""+$appSettings.EmailSender

#$obj = $doc.configuration.appSettings.add | where {$_.Key -eq 'AzureCdnUrlHost'}
#$obj.value = ""+$appSettings.AzureCdnHost

#$obj = $doc.configuration.appSettings.add | where {$_.Key -eq 'StorageConnection'}
#$obj.value = "DefaultEndpointsProtocol=https;AccountName="+ $appSettings.StorageAccountName +";AccountKey=" + $appSettings.StorageAccountKey

#$doc.Save($webConfigPath.FullName)

# Delete Git Folder

Write-Host "Deleting .git folder..."

$gitFolder = Get-ChildItem $rootDirectory -Force | where {$_.Name -eq ".git"}
Remove-Item  "$rootDirectory\$gitFolder" -Force -Recurse

Delete Setup Folder

Write-Host "Deleting setup folder..."

$setupFolder = Get-ChildItem $rootDirectory -Force | where {$_.Name -eq "setup"}
Remove-Item "$rootDirectory\$setupFolder" -Force -Recurse

# Build Project

# TODO rebuild project...

# Launch VS Solution

# TODO launch solution...