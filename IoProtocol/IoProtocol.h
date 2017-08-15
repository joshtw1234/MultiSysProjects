#pragma once
#include <Windows.h>
#include <string>



using namespace std;

class IIoProtocol
{
public:
	HANDLE m_ReadHandle = NULL;
	HANDLE m_WriteHandle = NULL;

	virtual BOOL OpenDevice() = 0;
	virtual BOOL CloseDevice() = 0;
	virtual BOOL Read(_In_ unsigned char * lpBuffer, _In_  int nNumberOfBytesToRead, _Out_  LPDWORD lpNumberOfBytesRead) = 0;
	virtual BOOL Write(_In_ unsigned char * lpBuffer, _In_  int nNumberOfBytesToWrite, _Out_  LPDWORD lpNumberOfBytesWritten) = 0;
};


