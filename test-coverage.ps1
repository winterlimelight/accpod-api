# This requires OpenCover and ReportGenerator
# https://github.com/opencover/opencover/releases
# https://github.com/danielpalme/ReportGenerator/releases
$openCover = "C:\Code\opencover.4.7.922\OpenCover.Console.exe"
$reportGenerator = "C:\Code\ReportGenerator_4.1.5\netcoreapp2.1\ReportGenerator.dll"

Push-Location

$outputFolder = "$PSScriptRoot/Coverage"
if (Test-Path $outputFolder)
{
    # https://stackoverflow.com/questions/53207678/powershell-remove-item-not-waiting
    Get-ChildItem $outputFolder -Recurse | Remove-Item -Recurse
}
New-Item -Path "$PSScriptRoot" -Name "Coverage" -ItemType "directory"

# Setup filters
$filter = "+[Api]* +[Business]* +[Data]*"

# Integration Test Project
cd $PSScriptRoot/Tests.Integration
dotnet build

$openCoverArgs = `
    "-register:user", `
    "-oldStyle", `
    "-target:C:\Program Files\dotnet\dotnet.exe", `
    "-targetargs:test", `
    "-filter:$filter", `
	"-output:$PSScriptRoot/Coverage/Integration.xml", `
    "-skipautoprops"

& "$openCover" $openCoverArgs

# Generate Report
cd $PSScriptRoot

dotnet "$reportGenerator" `
	"-reports:$PSScriptRoot/Coverage/Integration.xml" `
	"-targetdir:$PSScriptRoot/Coverage"

Pop-Location