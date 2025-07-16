# ExportSpecificFolders.ps1
param(
    [string[]]$Folders = @("Domain", "Entities", "Graphics", "Input", "Managers", "States"),
    [string]$OutputFolder = "CodeExport"
)

if (-not (Test-Path $OutputFolder)) {
    New-Item -ItemType Directory -Path $OutputFolder | Out-Null
}

foreach ($folder in $Folders) {
    $files = Get-ChildItem -Path $folder -Filter "*.cs" -ErrorAction SilentlyContinue
    
    if ($files) {
        $outputFile = Join-Path $OutputFolder "$folder.txt"
        
        Add-Content -Path $outputFile -Value "// FOLDER: $folder"
        Add-Content -Path $outputFile -Value "// ========================`n"
        
        foreach ($file in $files) {
            Add-Content -Path $outputFile -Value "`n// FILE: $($file.Name)`n"
            Add-Content -Path $outputFile -Value (Get-Content $file.FullName -Raw)
            Add-Content -Path $outputFile -Value "`n// ========================`n"
        }
        
        Write-Host "Created: $folder.txt" -ForegroundColor Green
    }
}

# Also export root level files
$rootFiles = Get-ChildItem -Filter "*.cs" -File
if ($rootFiles) {
    $outputFile = Join-Path $OutputFolder "Root.txt"
    foreach ($file in $rootFiles) {
        Add-Content -Path $outputFile -Value "`n// FILE: $($file.Name)`n"
        Add-Content -Path $outputFile -Value (Get-Content $file.FullName -Raw)
    }
    Write-Host "Created: Root.txt" -ForegroundColor Green
}