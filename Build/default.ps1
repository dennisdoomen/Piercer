properties { 
	$BaseDirectory = Resolve-Path .. 
    
    $ProjectName = "Piercer"
    
	$NugetOutputDir = "$BaseDirectory\Artifacts"
	$SrcDir = "$BaseDirectory\Src"
    $ReportsDir = "$BaseDirectory\Artifacts"
	$SolutionFilePath = "$BaseDirectory\$ProjectName.sln"
	$AssemblyInfoFilePath = "$SrcDir\Piercer.Middleware\Properties\AssemblyInfo.cs"
    $ilMergeModule.ilMergePath = "$BaseDirectory\packages\ilmerge.2.14.1208\tools\ILMerge.exe"
    
    $NugetExe = "$BaseDirectory\Lib\nuget.exe"
    $GitVersionExe = "$BaseDirectory\Lib\GitVersion.exe"
}

task default -depends Clean, RestoreNugetPackages, ExtractVersionsFromGit, ApplyAssemblyVersioning, Compile, RunTests, CreateNuGetPackages

task Clean -Description "Cleaning solution." {
	Remove-Item $NugetOutputDir/* -Force -Recurse -ErrorAction SilentlyContinue
	exec { msbuild /nologo /verbosity:minimal $SolutionFilePath /t:Clean  }
    
    if (!(Test-Path -Path $NugetOutputDir)) {
        New-Item -ItemType Directory -Force -Path $NugetOutputDir
    }
}

task RestoreNugetPackages {
    $packageConfigs = Get-ChildItem $BaseDirectory -Recurse | where{$_.Name -eq "packages.config"}

    foreach($packageConfig in $packageConfigs){
    	Write-Host "Restoring" $packageConfig.FullName 
    	exec { 
            . "$NugetExe" install $packageConfig.FullName -OutputDirectory "$BaseDirectory\Packages" -ConfigFile "$BaseDirectory\NuGet.Config"
        }
    }
}

task ExtractVersionsFromGit {
    
        $json = . "$GitVersionExe" 
        
        if ($LASTEXITCODE -eq 0) {
            $version = (ConvertFrom-Json ($json -join "`n"));
          
            TeamCity-SetBuildNumber $version.FullSemVer;
            
            $script:AssemblyVersion = $version.ClassicVersion;
            $script:InformationalVersion = $version.InformationalVersion;
            $script:NuGetVersion = $version.ClassicVersion;
        }
        else {
            Write-Output $json -join "`n";
        }
}

task ApplyAssemblyVersioning {
	Get-ChildItem -Path $SrcDir -Filter "?*AssemblyInfo.cs" -Recurse -Force |
	foreach-object {  

		Set-ItemProperty -Path $_.FullName -Name IsReadOnly -Value $false

        $content = Get-Content $_.FullName
        
        if ($script:AssemblyVersion) {
    		Write-Output "Updating " $_.FullName "with version" $script:AssemblyVersion
    	    $content = $content -replace 'AssemblyVersion\("(.+)"\)', ('AssemblyVersion("' + $script:AssemblyVersion + '")')
            $content = $content -replace 'AssemblyFileVersion\("(.+)"\)', ('AssemblyFileVersion("' + $script:AssemblyVersion + '")')
        }
		
        if ($script:InformationalVersion) {
    		Write-Output "Updating " $_.FullName "with information version" $script:InformationalVersion
            $content = $content -replace 'AssemblyInformationalVersion\("(.+)"\)', ('AssemblyInformationalVersion("' + $script:InformationalVersion + '")')
        }
        
	    Set-Content -Path $_.FullName $content
	}    
}

task Compile -Description "Compiling solution." { 
	exec { msbuild /nologo /verbosity:minimal $SolutionFilePath /p:Configuration=Release }
}

task RunTests -depends Compile -Description "Running all unit tests." {
    $xunitRunner = "$BaseDirectory\Packages\xunit.runner.console.2.0.0\tools\xunit.console.exe"
	
    if (!(Test-Path $ReportsDir)) {
		New-Item $ReportsDir -Type Directory
	}

	Get-ChildItem "$BaseDirectory\Tests" -Recurse -Include *.Specs.dll | 
		Where-Object { ($_.FullName -notlike "*obj*") } | 
			% {
				$project = $_.BaseName
	
				Write-Host "Running the unit tests in $_"
				exec { . $xunitRunner "$_" -html "$ReportsDir\$project.html"  }
			}
}

task CreateNuGetPackages -depends Compile -Description "Creating NuGet package." {
	Get-ChildItem $SrcDir -Recurse -Include *.nuspec | % {
		exec { 
            if (!$NuGetVersion) {
                $NuGetVersion = "0.1.0.0"
            }
        
        Write-Host $_
            . "$NugetExe" pack $_ -o $NugetOutputDir -version $NuGetVersion 
        }
	}
}
