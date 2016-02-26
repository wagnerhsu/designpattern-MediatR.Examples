function Get-SqlInstance { 
    param ( 
        [ValidateScript({ Test-Connection -ComputerName $_ -Quiet -Count 1 })] 
        [Parameter(ValueFromPipeline)] 
        [string[]]$Computername = 'localhost' 
    ) 
    process { 
        foreach ($Computer in $Computername) { 
            try { 
                $SqlServices = Get-Service -ComputerName $Computer -DisplayName 'SQL Server (*' 
                if (!$SqlServices) { 
                    Write-Verbose 'No instances found' 
                } else { 
                    $InstanceNames = $SqlServices | Select-Object @{ n = 'Instance'; e = { $_.DisplayName.Trim('SQL Server ').Trim(')').Trim('(') } } | Select-Object -ExpandProperty Instance 
                    foreach ($InstanceName in $InstanceNames) { 
                        [pscustomobject]@{ 'Computername' = $Computer; 'Instance' = $InstanceName } 
                    } 
                } 
            } catch { 
                Write-Error "Error: $($_.Exception.Message) - Line Number: $($_.InvocationInfo.ScriptLineNumber)" 
                $false 
            } 
        } 
    } 
} 
 
function Get-SqlLogin { 
    param ( 
        [ValidateScript({ Test-Connection -ComputerName $_ -Quiet -Count 1 })] 
        [Parameter(ValueFromPipelineByPropertyName)] 
        [string[]]$Computername = 'localhost', 
        [Parameter(ValueFromPipelineByPropertyName)] 
        [string]$Instance, 
        [string]$Name 
    ) 
    begin { 
        [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO') | Out-Null 
    } 
    process { 
        try { 
            foreach ($Computer in $Computername) { 
                $Instances = Get-SqlInstance -Computername $Computer 
                foreach ($Instance in $Instances.Instance) { 
                    if ($Instance -eq 'MSSQLSERVER') { 
                        $Server = new-object ('Microsoft.SqlServer.Management.Smo.Server') $Computer 
                    } else { 
                        $Server = new-object ('Microsoft.SqlServer.Management.Smo.Server') "$Computer`\$Instance" 
                    } 
                    if (!$Name) { 
                        $Server.Logins 
                    } else { 
                        $Server.Logins | where { $_.Name -eq $Name } 
                    } 
                } 
            } 
        } catch { 
            Write-Error "Error: $($_.Exception.Message) - Line Number: $($_.InvocationInfo.ScriptLineNumber)" 
            $false 
        } 
    } 
} 
 
function New-SqlLogin { 
    param ( 
        [ValidateScript({ Test-Connection -ComputerName $_ -Quiet -Count 1 })] 
        [Parameter(ValueFromPipelineByPropertyName)] 
        [string]$Computername = 'localhost', 
        [Parameter(ValueFromPipelineByPropertyName)] 
        [string]$Instance, 
        [string]$Username,
        [string]$Password
    ) 
    begin { 
        [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO') | Out-Null 
    } 
    process { 
        try { 
            $Server = New-Object ('Microsoft.SqlServer.Management.Smo.Server') $Computername 
            ## https://msdn.microsoft.com/en-us/library/microsoft.sqlserver.management.smo.logintype.aspx 
            $login = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Login -ArgumentList $Server, $Username 
            $login.LoginType = 'SqlLogin'
            $login.PasswordExpirationEnabled = $false
            $login.Create($Password)
            $login.AddToRole('dbcreator')
            'Sql login created'
        } catch { 
            Write-Error "Error: $($_.Exception.Message) - Line Number: $($_.InvocationInfo.ScriptLineNumber)" 
            'Creating sql login failed'
        } 
    } 
}

function Update-LoginMode {
    param (
        [Parameter(ValueFromPipelineByPropertyName)] 
        [string]$Computername = 'localhost', 
        [ValidateSet('Integrated', 'Mixed', 'Normal')]
        [string]$loginMode
    )
    begin { 
        [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO') | Out-Null 
    }
    process {
        try {
            $Server = New-Object ('Microsoft.SqlServer.Management.Smo.Server') $Computername 
            $Server.Settings.LoginMode = $loginmode
            $Server.Alter()
            'Sql login mode updated'
        } catch {
            Write-Error "Error: $($_.Exception.Message) - Line Number: $($_.InvocationInfo.ScriptLineNumber)" 
            'Update sql login mode failed.' 
        }
    }
}

$instance = Get-SqlInstance
$instance | New-SqlLogin -Username 'kf-a' -Password '$Kf@admin'
$instance | Update-LoginMode -loginMode 'Mixed'
$sqlService = Get-Service -ComputerName $instance.Computername -Name $instance.Instance

[System.Collections.ArrayList]$ServicesToRestart = @()

function Custom-GetDependServices ($ServiceInput)
{
	#Write-Host "Name of `$ServiceInput: $($ServiceInput.Name)"
	#Write-Host "Number of dependents: $($ServiceInput.DependentServices.Count)"
	If ($ServiceInput.DependentServices.Count -gt 0)
	{
		ForEach ($DepService in $ServiceInput.DependentServices)
		{
			#Write-Host "Dependent of $($ServiceInput.Name): $($Service.Name)"
			If ($DepService.Status -eq "Running")
			{
				#Write-Host "$($DepService.Name) is running."
				$CurrentService = Get-Service -Name $DepService.Name
				
                # get dependancies of running service
				Custom-GetDependServices $CurrentService                
			}
			Else
			{
				Write-Host "$($DepService.Name) is stopped. No Need to stop or start or check dependancies."
			}
			
		}
	}
    Write-Host "Service to Stop $($ServiceInput.Name)"
    if ($ServicesToRestart.Contains($ServiceInput.Name) -eq $false)
    {
        Write-Host "Adding service to stop $($ServiceInput.Name)"
        $ServicesToRestart.Add($ServiceInput.Name)
    }
}

# Get the main service
$Service = $sqlService

# Get dependancies and stop order
Custom-GetDependServices -ServiceInput $Service


Write-Host "-------------------------------------------"
Write-Host "Stopping Services"
Write-Host "-------------------------------------------"
foreach($ServiceToStop in $ServicesToRestart)
{
    Write-Host "Stop Service $ServiceToStop"
    Stop-Service $ServiceToStop -Verbose #-Force
}
Write-Host "-------------------------------------------"
Write-Host "Starting Services"
Write-Host "-------------------------------------------"
# Reverse stop order to get start order
$ServicesToRestart.Reverse()

foreach($ServiceToRestart in $ServicesToRestart)
{
    Write-Host "Start Service $ServiceToRestart"
    Start-Service $ServiceToRestart -Verbose
}
Write-Host "-------------------------------------------"
Write-Host "Restart of services completed"
Write-Host "-------------------------------------------"