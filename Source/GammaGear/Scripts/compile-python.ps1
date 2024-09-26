param([String]$outpath, [String]$inputs)

$projects = $inputs.split(";")

foreach ($p in $projects)
{
    # Get the most recently edited file's write time
    $ticks = (Get-ChildItem $p -Recurse -File | Sort-Object -Descending -Property LastWriteTime | select -First 1).LastWriteTime.Ticks.ToString()

    $folder = Get-Item $p
    $name = $folder.Name

    Write-Host "Building $name"

    if ((Test-Path "$outpath\$name.timestamp") -and ($ticks -eq (Get-Content "$outpath\$name.timestamp")))
    {
        Write-Host "Skipping building $name because up-to-date build was found..."
        continue
    }

    pip wheel "$PWD\$p" -w "$outpath" --find-links "$outpath"
    if (!$?)
    {
        exit 1
    }

    if (Test-Path "$outpath\$name.timestamp")
    {
        Remove-Item "$outpath\$name.timestamp"
    }
    New-Item -Path "$outpath" -Name "$name.timestamp" -ItemType "file" -Value $ticks 2>$null | Out-Null


    #  Moved installation directory to executable path
    # Python.Included writes to the appdata/local dir, get rid of the stuff that that it writes if we are updating our packages.
    #Remove-Item -Path $env:LOCALAPPDATA\python-3.11.0-embed-amd64 -Recurse 2>$null
    #Remove-Item -Path $env:LOCALAPPDATA\python-3.11.0-embed-amd64.zip 2>$null
}
exit 0
