﻿properties { 
  $zipFileName = "Json40r2.zip"
  $majorVersion = "4.0.2"
  $version = GetVersion $majorVersion
  $signAssemblies = $false
  $signKeyPath = "D:\Development\Releases\newtonsoft.snk"
  $buildDocumentation = $true
  $buildNuGet = $true
  
  $baseDir  = resolve-path ..
  $buildDir = "$baseDir\Build"
  $sourceDir = "$baseDir\Src"
  $toolsDir = "$baseDir\Tools"
  $docDir = "$baseDir\Doc"
  $releaseDir = "$baseDir\Release"
  $workingDir = "$baseDir\Working"
  $builds = @(
    @{Name = "Newtonsoft.Json"; TestsName = "Newtonsoft.Json.Tests"; Constants=""; FinalDir="Net"; NuGetDir = "net40-client,net40-full"; Framework="net-4.0"},
    @{Name = "Newtonsoft.Json.WindowsPhone"; TestsName = $null; Constants="SILVERLIGHT;WINDOWS_PHONE"; FinalDir="WindowsPhone"; NuGetDir = "sl3-wp"; Framework="net-4.0"},
    @{Name = "Newtonsoft.Json.Silverlight"; TestsName = "Newtonsoft.Json.Tests.Silverlight"; Constants="SILVERLIGHT"; FinalDir="Silverlight"; NuGetDir = "sl4"; Framework="net-4.0"},
    @{Name = "Newtonsoft.Json.Net35"; TestsName = "Newtonsoft.Json.Tests.Net35"; Constants="NET35"; FinalDir="Net35"; NuGetDir = "net35-client,net35-full"; Framework="net-2.0"},
    @{Name = "Newtonsoft.Json.Net20"; TestsName = "Newtonsoft.Json.Tests.Net20"; Constants="NET20"; FinalDir="Net20"; NuGetDir = "net20"; Framework="net-2.0"}
  )
}

$framework = '4.0x86'

task default -depends Test

# Ensure a clean working directory
task Clean {
  Set-Location $baseDir
  
  if (Test-Path -path $workingDir)
  {
    Write-Output "Deleting Working Directory"
    
    del $workingDir -Recurse -Force
  }
  
  New-Item -Path $workingDir -ItemType Directory
}

# Build each solution, optionally signed
task Build -depends Clean { 
  Write-Host -ForegroundColor Green "Updating assembly version"
  Write-Host
  Update-AssemblyInfoFiles $sourceDir ($majorVersion + '.0') $version

  foreach ($build in $builds)
  {
    $name = $build.Name
    $finalDir = $build.FinalDir

    Write-Host -ForegroundColor Green "Building " $name
    Write-Host
    exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release /p:OutputPath=bin\Release\$finalDir\ /p:AssemblyOriginatorKeyFile=$signKeyPath "/p:SignAssembly=$signAssemblies" (GetConstants $build.Constants $signAssemblies) ".\Src\$name.sln" } "Error building $name"
  }
}

# Merge LinqBridge into .NET 2.0 build
task Merge -depends Build {
  $binaryDir = "$sourceDir\Newtonsoft.Json\bin\Release\Net20"
  MergeAssembly "$binaryDir\Newtonsoft.Json.Net20.dll" $signKeyPath "$binaryDir\LinqBridge.dll"
  del $binaryDir\LinqBridge.dll

  $binaryDir = "$sourceDir\Newtonsoft.Json.Tests\bin\Release\Net20"
  MergeAssembly "$binaryDir\Newtonsoft.Json.Tests.Net20.dll" $signKeyPath "$binaryDir\LinqBridge.dll"
  MergeAssembly "$binaryDir\Newtonsoft.Json.Net20.dll" $signKeyPath "$binaryDir\LinqBridge.dll"
  del $binaryDir\LinqBridge.dll
}

# Optional build documentation, add files to final zip
task Package -depends Merge {
  foreach ($build in $builds)
  {
    $name = $build.TestsName
    $finalDir = $build.FinalDir
    
    Copy-Item -Path "$sourceDir\Newtonsoft.Json\bin\Release\$finalDir" -Destination $workingDir\Package\Bin\$finalDir -recurse
  }
  
  if ($buildNuGet)
  {
    New-Item -Path $workingDir\NuGet -ItemType Directory
    Copy-Item -Path "$buildDir\Newtonsoft.Json.nuspec" -Destination $workingDir\NuGet\Newtonsoft.Json.nuspec -recurse
    
    foreach ($build in $builds)
    {
        $name = $build.TestsName
        $finalDir = $build.FinalDir
        $frameworkDirs = $build.NuGetDir.Split(",")
        
        foreach ($frameworkDir in $frameworkDirs)
        {
            Copy-Item -Path "$sourceDir\Newtonsoft.Json\bin\Release\$finalDir" -Destination $workingDir\NuGet\lib\$frameworkDir -recurse
        }
    }
  
    exec { .\Tools\NuGet\NuGet.exe pack $workingDir\NuGet\Newtonsoft.Json.nuspec }
    move -Path .\*.nupkg -Destination $workingDir\NuGet
  }
  
  if ($buildDocumentation)
  {
    # Sandcastle has issues when compiling with .NET 4 MSBuild - http://shfb.codeplex.com/Thread/View.aspx?ThreadId=50652
    exec { .$env:windir\Microsoft.NET\Framework\v3.5\MSBuild.exe "/t:Clean;Rebuild" /p:Configuration=Release "/p:DocumentationSourcePath=$workingDir\Package\Bin\Net" $docDir\doc.shfbproj } "Error building documentation. Check that you have Sandcastle, Sandcastle Help File Builder and HTML Help Workshop installed."
    
    move -Path $workingDir\Documentation\Documentation.chm -Destination $workingDir\Package\Documentation.chm
    move -Path $workingDir\Documentation\LastBuild.log -Destination $workingDir\Documentation.log
  }
  
  Copy-Item -Path $docDir\readme.txt -Destination $workingDir\Package\
  Copy-Item -Path $docDir\versions.txt -Destination $workingDir\Package\Bin\

  robocopy $sourceDir $workingDir\Package\Source\Src /MIR /NP /XD .svn bin obj /XF *.suo *.user
  robocopy $buildDir $workingDir\Package\Source\Build /MIR /NP /XD .svn
  robocopy $docDir $workingDir\Package\Source\Doc /MIR /NP /XD .svn
  robocopy $toolsDir $workingDir\Package\Source\Tools /MIR /NP /XD .svn
  
  exec { .\Tools\7-zip\7za.exe a -tzip $workingDir\$zipFileName $workingDir\Package\* } "Error zipping"
}

# Unzip package to a location
task Deploy -depends Package {
  exec { .\Tools\7-zip\7za.exe x -y "-o$workingDir\Deployed" $workingDir\$zipFileName } "Error unzipping"
}

# Run tests on deployed files
task Test -depends Deploy {
  foreach ($build in $builds)
  {
    $name = $build.TestsName
    if ($name -ne $null)
    {
        $finalDir = $build.FinalDir
        $framework = $build.Framework
        
        Write-Host -ForegroundColor Green "Copying test assembly $name to deployed directory"
        Write-Host
        robocopy ".\Src\Newtonsoft.Json.Tests\bin\Release\$finalDir" $workingDir\Deployed\Bin\$finalDir /NP /XO /XF LinqBridge.dll
        
        Copy-Item -Path ".\Src\Newtonsoft.Json.Tests\bin\Release\$finalDir\$name.dll" -Destination $workingDir\Deployed\Bin\$finalDir\

        Write-Host -ForegroundColor Green "Running tests " $name
        Write-Host
        exec { .\Tools\NUnit\nunit-console.exe "$workingDir\Deployed\Bin\$finalDir\$name.dll" /framework=$framework /xml:$workingDir\$name.xml } "Error running $name tests"
    }
  }
}

function MergeAssembly($dllPrimaryAssembly, $signKey, [string[]]$mergedAssemlies)
{
  $mergeAssemblyPaths = [String]::Join(" ", $mergedAssemlies)
  
  $primary = Get-Item $dllPrimaryAssembly
  $mergedAssemblyName = $primary.Name
  $temporaryDir = $primary.DirectoryName + "\" + [Guid]::NewGuid().ToString()
  New-Item $temporaryDir -ItemType Directory
  
  $ilMergeKeyFile = switch($signAssemblies) { $true { "/keyfile:$signKeyPath" } default { "" } }
  
  try
  {
    exec { .\Tools\ILMerge\ilmerge.exe "/internalize" "/closed" "/log:$workingDir\$mergedAssemblyName.MergeLog.txt" $ilMergeKeyFile "/out:$temporaryDir\$mergedAssemblyName" $dllPrimaryAssembly $mergeAssemblyPaths } "Error executing ILMerge"
    Copy-Item -Path $temporaryDir\$mergedAssemblyName -Destination $dllPrimaryAssembly -Force
  }
  finally
  {
    Remove-Item $temporaryDir -Recurse -Force
  }
}

function GetConstants($constants, $includeSigned)
{
  $signed = switch($includeSigned) { $true { ";SIGNED" } default { "" } }

  return "/p:DefineConstants=`"TRACE;$constants$signed`""
}

function GetVersion($majorVersion)
{
    $now = [DateTime]::Now
    
    $year = $now.Year - 2000
    $month = $now.Month
    $totalMonthsSince2000 = ($year * 12) + $month
    $day = $now.Day
    $minor = "{0}{1:00}" -f $totalMonthsSince2000, $day
    
    $hour = $now.Hour
    $minute = $now.Minute
    $revision = "{0:00}{1:00}" -f $hour, $minute
    
    return $majorVersion + "." + $minor
}

function Update-AssemblyInfoFiles ([string] $sourceDir, [string] $assemblyVersionNumber, [string] $fileVersionNumber)
{
    $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $assemblyVersion = 'AssemblyVersion("' + $assemblyVersionNumber + '")';
    $fileVersion = 'AssemblyFileVersion("' + $fileVersionNumber + '")';
    
    Get-ChildItem -Path $sourceDir -r -filter AssemblyInfo.cs | ForEach-Object {
        
        $filename = $_.Directory.ToString() + '\' + $_.Name
        Write-Host $filename
        $filename + ' -> ' + $version
    
        (Get-Content $filename) | ForEach-Object {
            % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
            % {$_ -replace $fileVersionPattern, $fileVersion }
        } | Set-Content $filename
    }
}