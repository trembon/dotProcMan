# dotProcMan
Dotnet Core application to automatically start and restart other applications through a web ui.

Developed on a Windows 10 and running on a Raspberry Pi 3 (raspbian)

## Functions
* configure processes in appsettings.json
* start/stop/restart processes manually
* automatically restart applications that crash
* send commands to the process through the web ui

## Limitation
Depending on dotnet core version and operating system, running a process as a different user might not be supported.

## Configuration
```
"Processes": [
	{
		"Name": "Example", // name of the process, only used in the web ui
		"FileName": "cmd", // the executable to start the process
		"Args": "", // arguments to the to the executable, like --help
		"WorkingDirectory": "", // directory to start the application in
		"Username": "", // username, to start the executable as another user
		"Password": "", // password, to start the executable as another user
		"StartDelay": 1, // seconds to delay automatic start when dotProcMan starts, 0 = no auto start
		"AutoRestart": false // if dotProcMan should restart the process if it exists with another exit code than 0
	}
]
```