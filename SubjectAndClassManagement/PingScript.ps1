
$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent

$hostname = "localhost"
$port = 7212
$logFile = Join-Path -Path $scriptDir -ChildPath "log.txt"

function Test-WebAppAvailability {
    param (
        [string]$hostname,
        [int]$port
    )
    
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $responseTime = -1
    
    $executionTime = Measure-Command {
        $result = Test-NetConnection -ComputerName $hostname -Port $port
    }
    
    if ($result.TcpTestSucceeded) {
        $status = "Online"
        $responseTime = $executionTime.TotalMilliseconds
    } else {
        $status = "Offline"
    }
    
    $logMessage = "$timestamp - $status - Response Time: ${responseTime}ms"
    Add-Content -Path $logFile -Value $logMessage
    Write-Output $logMessage
}


while ($true) {
    Test-WebAppAvailability -hostname $hostname -port $port
    Start-Sleep -Seconds 5
}
