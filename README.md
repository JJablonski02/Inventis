Powershell script to change .axaml files to UTF8 encoding

Get-ChildItem -Recurse -Filter *.axaml | ForEach-Object {
    $filePath = $_.FullName
    $content = Get-Content -Path $filePath -Encoding Default
    Set-Content -Path $filePath -Value $content -Encoding UTF8
    Write-Host "Converted: $filePath"
}
