properties { 
	$BaseDirectory = Resolve-Path .. 
    
    $ProjectName = "Piercer"
    
	$NugetOutputDir = "$BaseDirectory\Artifacts"
	$SrcDir = "$BaseDirectory\Src"
    $ReportsDir = "$BaseDirectory\Artifacts"
	$SolutionFilePath = "$BaseDirectory\$ProjectName.sln"
	$AssemblyInfoFilePath = "$SrcDir\Piercer.Middleware\Properties\AssemblyInfo.cs"
    
    $NugetExe = "$BaseDirectory\Lib\nuget.exe"
	$NuGetVersion = "0.1.0.0"
}

task default -depends Clean, RestoreNugetPackages, Compile, CreateNuGetPackages

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

task Compile -Description "Compiling solution." { 
	exec { msbuild /nologo /verbosity:minimal $SolutionFilePath /p:Configuration=Release }
}

task CreateNuGetPackages -depends Compile -Description "Creating NuGet package." {
	Get-ChildItem $SrcDir -Recurse -Include *.nuspec | % {
		exec { 
        
        Write-Host $_
            . "$NugetExe" pack $_ -o $NugetOutputDir -version $NuGetVersion 
        }
	}
}
