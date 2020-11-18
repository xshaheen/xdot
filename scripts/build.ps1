. $PSScriptRoot\common.ps1

if (Test-Path $artifactsDir) { Remove-Item $artifactsDir -Force -Recurse }

Write-Host ""
Write-Host "#####################"
Write-Host "###   Clean Sln   ###"
Write-Host "#####################"
exec { & dotnet clean -c Release }

Write-Host ""
Write-Host "#####################"
Write-Host "###   Build Sln   ###"
Write-Host "#####################"
exec { & dotnet build -c Release }

Write-Host ""
Write-Host "#####################"
Write-Host "###      Pack     ###"
Write-Host "#####################"
exec { & dotnet pack -c Release -o $artifactsDir --no-build }
