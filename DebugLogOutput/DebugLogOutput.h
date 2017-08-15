#pragma once

#include <strsafe.h>
#include <Shlwapi.h>
#pragma comment(lib, "Shlwapi.lib")
#define DEBUG_MAX_STRING 1024

class DebugLogOutput
{
public:
	DebugLogOutput(LPWSTR);
	~DebugLogOutput();
	int Output();
	WCHAR strDebugLogOutputString[DEBUG_MAX_STRING];
	int CmdOutput();

private:
	WCHAR strLogFileName[MAX_PATH];

};

