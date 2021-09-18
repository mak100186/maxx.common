# This code is based on an article at https://blogs.msdn.microsoft.com/tfssetup/2015/11/06/controlling-build-result-with-code-coverage-percentage-using-build-vnext/

param(
    [int]$desiredCodeCoveragePercent,
    [string] $username,
    [string] $password
)

Write-Host "Desired Code Coverage Percent is $desiredCodeCoveragePercent"
 
# Setting a few values
[int]$coveredBlocks = 0
[int]$skippedBlocks = 0
[int]$totalBlocks = 0
[decimal]$codeCoveragePercent = 0
[int]$numberOfTries = 10;
[int]$waitAfterEachTry = 2;

# Getting a few environment variables we need
[String]$buildID = "$env:BUILD_BUILDID"
[String]$project = "$env:SYSTEM_TEAMPROJECT"
 
$basicAuth = ("{0}:{1}" -f $username, $password)
$basicAuth = [System.Text.Encoding]::UTF8.GetBytes($basicAuth)
$basicAuth = [System.Convert]::ToBase64String($basicAuth)
$headers = @{Authorization = ("Basic {0}" -f $basicAuth)}
 
$url = "https://dev.azure.com/makofficial/" + $project + "/_apis/test/codeCoverage?buildId=" + $buildID + "&flags=1&api-version=2.0-preview"
Write-Host $url
for ($i = 0; $i -lt $numberOfTries; $i++) {
    

    $responseBuild = (Invoke-RestMethod -Uri $url -headers $headers -Method Get).value | Select-Object modules
 
    foreach ($module in $responseBuild.modules) {
        $coveredBlocks += $module.statistics[0].blocksCovered
        $skippedBlocks += $module.statistics[0].blocksNotCovered
    }
 
    $totalBlocks = $coveredBlocks + $skippedBlocks;
    if ($totalBlocks -eq 0) {
        $codeCoveragePercent = 0
        Write-Host "did not find any blocks. Retrying ..."
        if ($i -lt $numberOfTries - 1) {
            Start-Sleep -Seconds $waitAfterEachTry
            continue
        }
        
        Write-Host "Reached the maximum number of tries. Giving up."
        exit -1
    }
 
    $codeCoveragePercent = $coveredBlocks * 100.0 / $totalBlocks
    Write-Host "Code Coverage percentage is $codeCoveragePercent"
 
    if ($codeCoveragePercent -lt $desiredCodeCoveragePercent) {
        Write-Host "Failing the build as CodeCoverage limit not met"
        exit -1
    }

    break
}

Write-Host "CodeCoverage limit met"