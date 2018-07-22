#include "DebugLogOutput.h"



DebugLogOutput::DebugLogOutput(LPWSTR strLogOutputFileName)
{
	::StringCchCopyW(strLogFileName, MAX_PATH, strLogOutputFileName);
}


DebugLogOutput::~DebugLogOutput()
{
}


int DebugLogOutput::Output()
{
	DWORD dwSize;
	HANDLE hDataFile = 0;
	WCHAR DataFilePath[MAX_PATH];
	wchar_t buffer[MAX_PATH];

	GetModuleFileNameW(NULL, buffer, MAX_PATH);
	PathRemoveFileSpecW(buffer);

	//PathAppendW(DataFilePath, L"C:\\HP\\Data");
	PathAppendW(DataFilePath, buffer);
	if (ERROR_PATH_NOT_FOUND == ::CreateDirectoryW(DataFilePath, NULL))
	{
	}
	PathAppendW(DataFilePath, strLogFileName);
	hDataFile = CreateFileW(DataFilePath, // open Two.txt
		FILE_APPEND_DATA,         // open for writing
		FILE_SHARE_READ,          // allow multiple readers
		NULL,                     // no security
		OPEN_ALWAYS,              // open or create
		FILE_ATTRIBUTE_NORMAL,    // normal file
		NULL);                    // no attr. template

	SYSTEMTIME st, lt;
	GetSystemTime(&st);
	GetLocalTime(&lt);

	WCHAR lpOutputStrDisplay[DEBUG_MAX_STRING];
	size_t sz;

	::StringCchPrintfW(lpOutputStrDisplay, DEBUG_MAX_STRING, L"%04d:%02d:%02d:%02d:%02d:%02d:%04d:%s\r\n", lt.wYear, lt.wMonth, lt.wDay, lt.wHour, lt.wMinute, lt.wSecond, lt.wMilliseconds, strDebugLogOutputString);
	::StringCbLengthW(lpOutputStrDisplay, STRSAFE_MAX_CCH * sizeof(TCHAR), &sz);
	::wprintf(lpOutputStrDisplay);

	if (hDataFile != INVALID_HANDLE_VALUE)
	{
		WriteFile(hDataFile, lpOutputStrDisplay, sz, &dwSize, NULL);
		::CloseHandle(hDataFile);
	}


	return dwSize;

}


int DebugLogOutput::CmdOutput()
{

	SYSTEMTIME st, lt;
	GetSystemTime(&st);
	GetLocalTime(&lt);

	WCHAR lpOutputStrDisplay[DEBUG_MAX_STRING];
	::StringCchPrintfW(lpOutputStrDisplay, DEBUG_MAX_STRING, L"%04d:%02d:%02d:%02d:%02d:%02d:%04d:%s\n", lt.wYear, lt.wMonth, lt.wDay, lt.wHour, lt.wMinute, lt.wSecond, lt.wMilliseconds, strDebugLogOutputString);
	::wprintf(lpOutputStrDisplay);

	return lstrlenW(lpOutputStrDisplay);

}