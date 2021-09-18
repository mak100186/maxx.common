
param(
    [int]$desiredCodeCoveragePercent,
    [string] $codeCoverageFilePath
)

Write-Host "Desired Code Coverage Percent is $desiredCodeCoveragePercent"

[xml]$xml = Get-Content $codeCoverageFilePath

$codeCoveragePercent = [double]$xml.coverage.'line-rate' * 100.0

Write-Host "Code Coverage percentage is $codeCoveragePercent"
 
if ($codeCoveragePercent -lt $desiredCodeCoveragePercent) {
        Write-Host "Failing the build as CodeCoverage limit not met"
        exit -1
}

Write-Host "CodeCoverage limit met"