param([String]$Parameter1="C:\Projects\Framework\3.00-Dev\Foundation\MiddleTier\Framework.Entity\")

Set-ExecutionPolicy Unrestricted -Scope CurrentUser -Force

("Parameter1: " + $Parameter1)

$configFiles=get-childitem -Path $Parameter1*.cs
foreach ($file in $configFiles)
{
	(Get-Content $file.PSPath) | 
	Foreach-Object {$_-replace "public System.Guid ID", "public override System.Guid ID" `
		-replace "public int ID", "public override int ID" `
		-replace "public System.Guid Key", "public override System.Guid Key" `
		-replace "public byte", "public override byte" `
		-replace "public int RecordStatus", "public override int RecordStatus" `
		-replace "public System.DateTime CreatedDate { get; set; }", "public override System.DateTime CreatedDate { get; set; }" `
		-replace "public System.DateTime ModifiedDate { get; set; }", "public override System.DateTime ModifiedDate { get; set; }"
	} | 
	Set-Content $file.PSPath
}