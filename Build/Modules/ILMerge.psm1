$script:ilMergeModule = @{}
$script:ilMergeModule.ilMergePath = $null

function Merge-Assemblies {
	Param(
		$files,
		$outputFile,
		$exclude,
		$keyfile
	)

	$exclude | out-file ".\exclude.txt"

	$args = @(
		"/internalize:exclude.txt", 
		"/xmldocs",
		"/wildcards",
        "/out:$outputFile",
        "/allowdup"
		) + $files

	if($ilMergeModule.ilMergePath -eq $null)
	{
		write-error "IlMerge Path is not defined. Please set variable `$ilMergeModule.ilMergePath"
	}
    
    Write-Host "$ilMergeModule.ilMergePath $args"
    
	$ilmerge_path = $ilMergeModule.ilMergePath
    & $ilmerge_path $args 

	if($LastExitCode -ne 0) {
		write-error "Merge Failed"
	}
	
	remove-item ".\exclude.txt"
}

Export-ModuleMember -Variable "ilMergeModule" -Function "Merge-Assemblies"