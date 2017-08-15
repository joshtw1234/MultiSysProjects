#pragma once
#include "IoProtocol.h"

#define MAX_BUFFER_SIZE 200

class IoProtocolPipeClient : public  IIoProtocol
{
private:
	HANDLE m_PipeInst = NULL;
	HANDLE m_ReadEvent = NULL;
	HANDLE m_WriteEvent = NULL;
	WCHAR *m_PipeName = NULL;
protected:
	
public:
	IoProtocolPipeClient(LPTSTR lpszPipename);
	virtual ~IoProtocolPipeClient();
	virtual BOOL OpenDevice();
	virtual BOOL CloseDevice();
	virtual BOOL Read(_In_ unsigned char * lpBuffer, _In_  int nNumberOfBytesToRead, _Out_  LPDWORD lpNumberOfBytesRead);
	virtual BOOL Write(_In_ unsigned char * lpBuffer, _In_  int nNumberOfBytesToWrite, _Out_  LPDWORD lpNumberOfBytesWritten);
};