# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/RelinkMulti/*" -Force -Recurse
dotnet publish "./RelinkMulti.csproj" -c Release -o "$env:RELOADEDIIMODS/RelinkMulti" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location