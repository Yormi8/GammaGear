param([String]$outpath, [String]$inputs, [String]$config)

$projects = $inputs.split(";")
foreach ($p in $projects)
{
    $folder = Get-Item $p
    $name = $folder.Name

    Write-Host "Building $name"

    if ($config -eq "Debug")
    {
        cargo build --lib --target-dir "$outpath\ggkatsuba" --manifest-path "$p/Cargo.toml"
    }
    else
    {
        cargo build --lib --target-dir "$outpath\ggkatsuba" --manifest-path "$p/Cargo.toml" --release
    }

    if (!$?)
    {
        exit 1
    }
}
exit 0
