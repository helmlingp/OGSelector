$patterns = '*.json','OGSelector.exe'
Get-ChildItem -Recurse -File | Where-Object { $name = $_.Name;   $patterns | Where-Object { $name -like $_ }} | Remove-Item -Force