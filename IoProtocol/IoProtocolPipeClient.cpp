#include "IoProtocolPipeClient.h"

IoProtocolPipeClient::IoProtocolPipeClient(LPTSTR lpszPipename) : m_PipeName(NULL),
																  m_PipeInst(INVALID_HANDLE_VALUE)
{
	this->m_PipeName = new WCHAR[255];
	wcscpy_s(this->m_PipeName, 255, lpszPipename);
	this->m_ReadEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
	this->m_WriteEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
}

IoProtocolPipeClient::~IoProtocolPipeClient()
{
	if (this->m_PipeName)
	{
		delete[] this->m_PipeName;
	}

	if (this->m_PipeInst != INVALID_HANDLE_VALUE)
	{
		CloseHandle(this->m_PipeInst);
		this->m_PipeInst = INVALID_HANDLE_VALUE;
	}
}

BOOL IoProtocolPipeClient::OpenDevice()
{
	this->m_PipeInst = CreateFile(this->m_PipeName,   // pipe name 
									GENERIC_READ |  // read and write access 
									GENERIC_WRITE,
									0,              // no sharing 
									NULL,           // default security attributes
									OPEN_EXISTING,  // opens existing pipe 
									FILE_FLAG_OVERLAPPED,  // default attributes 
									NULL);          // no template file 

	// The pipe connected; change to message-read mode. 
	if (this->m_PipeInst != INVALID_HANDLE_VALUE)
	{
		DWORD dwMode = PIPE_READMODE_MESSAGE;
		BOOL bSuccess = SetNamedPipeHandleState(this->m_PipeInst,    // pipe handle 
												&dwMode,  // new pipe mode 
												NULL,     // don't set maximum bytes 
												NULL);    // don't set maximum time 
		if (!bSuccess)
		{
			CloseHandle(this->m_PipeInst);
			return FALSE;
		}

		return TRUE;
	}

	return  FALSE;
}

BOOL IoProtocolPipeClient::CloseDevice()
{
	if (this->m_PipeInst != INVALID_HANDLE_VALUE)
	{
		CloseHandle(this->m_PipeInst);
		this->m_PipeInst = INVALID_HANDLE_VALUE;
	}

	return TRUE;
}

BOOL IoProtocolPipeClient::Read(unsigned char * lpBuffer, int nNumberOfBytesToRead, LPDWORD lpNumberOfBytesRead)
{
	OVERLAPPED overlapped;
	memset(&overlapped, 0, sizeof(overlapped));
	ResetEvent(this->m_ReadEvent);
	overlapped.hEvent = this->m_ReadEvent;

	if(this->m_PipeInst)
	{
		::ReadFile(this->m_PipeInst, lpBuffer, nNumberOfBytesToRead, NULL, &overlapped);
		if (GetLastError() == ERROR_IO_PENDING || GetLastError() == ERROR_SUCCESS)
		{
			if (WaitForSingleObject(this->m_ReadEvent, INFINITE) == WAIT_OBJECT_0)
			{
				GetOverlappedResult(this->m_PipeInst, &overlapped, lpNumberOfBytesRead, TRUE);
				return TRUE;
			}
		}
	}


	return FALSE;
}

BOOL IoProtocolPipeClient::Write(unsigned char * lpBuffer, int nNumberOfBytesToWrite, LPDWORD lpNumberOfBytesWritten)
{
	OVERLAPPED overlapped;
	memset(&overlapped, 0, sizeof(overlapped));
	ResetEvent(this->m_WriteEvent);
	overlapped.hEvent = this->m_WriteEvent;

	if (this->m_PipeInst)
	{
		::WriteFile(this->m_PipeInst, lpBuffer, nNumberOfBytesToWrite, NULL, &overlapped);
		FlushFileBuffers(this->m_PipeInst);
		if (GetLastError() == ERROR_IO_PENDING || GetLastError() == ERROR_SUCCESS)
		{
			if (WaitForSingleObject(overlapped.hEvent, INFINITE) == WAIT_OBJECT_0)
			{				
				GetOverlappedResult(this->m_PipeInst, &overlapped, lpNumberOfBytesWritten, TRUE);
				return TRUE;
			}
		}
	}

	return FALSE;
}
