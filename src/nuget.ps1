param(
    [string]
    [Parameter(Mandatory = $true)]
    $file,
    
    [string]
    [Parameter(Mandatory = $true)]
    $username,
    
    [string]
    [Parameter(Mandatory = $true)]
    $password,
    
    [string]
    [Parameter(Mandatory = $true)]
    $feedName
    )

process{
$xml = [xml](Get-Content $file)

# create the username node and set the attributes
$userNameNode = $xml.CreateElement("add")
$userNameNode.SetAttribute("key", "Username")
$userNameNode.SetAttribute("value", $username)

# create the password node and set the attributes
$passwordNode = $xml.CreateElement("add")
$passwordNode.SetAttribute("key", "ClearTextPassword")
$passwordNode.SetAttribute("value", $password)

# create the feedName node and attach the username and password nodes
$feedNameNode = $xml.CreateElement($feedName)
[void] $feedNameNode.AppendChild($userNameNode)
[void] $feedNameNode.AppendChild($passwordNode)

# create the packageSourceCredentials node and append the feedName node
$credentialsNode = $xml.GetElementsByTagName("packageSourceCredentials")
if($credentialsNode.Count -gt 0)
{ 
    Write-Host "removing credential node"
    $xml.configuration.RemoveChild($credentialsNode[0]);
}else{
    Write-Host "no credential node found "
}

$credentialsNode = $xml.CreateElement("packageSourceCredentials")

[void] $credentialsNode.AppendChild($feedNameNode);

# add the packageSourceCredentials node to the document's configuration node
$xml.configuration.AppendChild($credentialsNode);

Write-Host "Saving..."
# save the file to the same location
$xml.Save($file)

Write-Host "Done!"
}