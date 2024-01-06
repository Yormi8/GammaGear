param([String]$outpath, [String]$inputs)

$projects = $inputs.split(";")
foreach ($p in $projects)
{
    $folder = Get-Item $p
    $name = $folder.Name
    if ((Test-Path "$outpath\$name.timestamp") -and ($folder.LastWriteTime.Ticks.ToString() -eq (Get-Content "$outpath\$name.timestamp")))
    {
        Write-Host "Skipping building $name because up-to-date build was found..."
        continue
    }

    pip wheel "$PWD\$p" -w "$outpath"
    if (!$?)
    {
        exit 1
    }
    New-Item -Path "$outpath" -Name "$name.timestamp" -ItemType "file" -Value $folder.LastWriteTime.Ticks | Out-Null

    # Python.Included writes to the appdata/local dir, get rid of the stuff that that it writes if we are updating our packages.
    Remove-Item -Path $env:LOCALAPPDATA\python-3.11.0-embed-amd64 -Recurse 2>$null
    Remove-Item -Path $env:LOCALAPPDATA\python-3.11.0-embed-amd64.zip 2>$null
}
exit 0
