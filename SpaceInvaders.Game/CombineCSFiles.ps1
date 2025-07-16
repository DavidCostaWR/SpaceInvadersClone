# CombineCSFiles.ps1
# PowerShell script to combine all C# files in a project

param(
    [string]$Path = ".",
    [string]$OutputFile = "AllCSFiles.txt",
    [string[]]$Exclude = @("bin", "obj", ".vs", "packages"),
    [switch]$IncludeTests = $false
)

# Clear output file if it exists
if (Test-Path $OutputFile) {
    Remove-Item $OutputFile
}

Write-Host "Combining C# files from: $Path" -ForegroundColor Green
Write-Host "Output file: $OutputFile" -ForegroundColor Green

$fileCount = 0
$totalLines = 0

# Build exclude filter
$excludeFilter = $Exclude | ForEach-Object { "*\$_\*" }
if (-not $IncludeTests) {
    $excludeFilter += "*Test*"
}

# Get all .cs files
$files = Get-ChildItem -Path $Path -Filter "*.cs" -Recurse | 
    Where-Object { 
        $file = $_
        $include = $true
        foreach ($filter in $excludeFilter) {
            if ($file.FullName -like $filter) {
                $include = $false
                break
            }
        }
        $include
    } |
    Sort-Object DirectoryName, Name

# Process each file
foreach ($file in $files) {
    $relativePath = $file.FullName.Replace((Get-Location).Path, "").TrimStart("\")
    $content = Get-Content $file.FullName -Raw
    $lineCount = ($content -split "`n").Count
    
    # Add file header
    Add-Content -Path $OutputFile -Value "`n// ============================================"
    Add-Content -Path $OutputFile -Value "// FILE: $relativePath"
    Add-Content -Path $OutputFile -Value "// Lines: $lineCount"
    Add-Content -Path $OutputFile -Value "// ============================================`n"
    
    # Add file content
    Add-Content -Path $OutputFile -Value $content
    
    $fileCount++
    $totalLines += $lineCount
    
    Write-Host "Added: $relativePath ($lineCount lines)" -ForegroundColor Cyan
}

# Summary
Write-Host "`nSummary:" -ForegroundColor Yellow
Write-Host "Total files: $fileCount" -ForegroundColor Green
Write-Host "Total lines: $totalLines" -ForegroundColor Green
Write-Host "Output saved to: $OutputFile" -ForegroundColor Green

# Show file size
$fileSize = (Get-Item $OutputFile).Length
$fileSizeMB = [math]::Round($fileSize / 1MB, 2)
Write-Host "Output file size: $fileSizeMB MB" -ForegroundColor Green