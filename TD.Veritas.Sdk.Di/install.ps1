param($installPath, $toolsPath, $package, $project)

# $project = Get-Project

$project

$unity = $project.ProjectItems.Item("Unity")
$xml = $unity.ProjectItems.Item("unity.sdk.di.xml")
$xml.Properties.Item("BuildAction").Value = 0
$xml.Properties.Item("CopyToOutputDirectory").Value = 1
