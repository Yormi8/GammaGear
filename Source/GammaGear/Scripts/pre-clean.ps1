param([String]$outpath)

Remove-Item "$outpath\*.timestamp" | Out-Null
Remove-Item "$outpath\*.whl" | Out-Null
