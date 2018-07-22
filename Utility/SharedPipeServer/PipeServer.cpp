#include "PipeServer.h"

HANDLE PipeServer::m_ThreadHandle = NULL;
HANDLE *PipeServer::m_PipeInst = NULL;
HANDLE PipeServer::m_ThreadStopEvent = NULL;
ISimpleCommandFactory* PipeServer::m_Factory = NULL;
WCHAR *PipeServer::m_PipeName = NULL;
int PipeServer::m_Instance = 1;
PipeServer *m_PipeServer = NULL;
PipeServer* PipeServer::m_PipeServer = NULL;
int PipeServer::m_BufferSize = DEFAULT_BUFFER_SIZE;

PipeServer::PipeServer()
{
	m_ThreadStopEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
	m_PipeName = new WCHAR[100];
	wcscpy_s(m_PipeName, 100, PIPE_NAME);
}

PipeServer * PipeServer::GetInstance()
{
	if (m_PipeServer == NULL)
	{
		m_PipeServer = new PipeServer();
	}

	return m_PipeServer;
}

PipeServer::~PipeServer()
{
	if (m_ThreadStopEvent)
		CloseHandle(m_ThreadStopEvent);

	if (m_PipeName)
		delete[] m_PipeName;

	if (m_PipeInst)
	{
		delete[] m_PipeInst;
	}
}

void PipeServer::SetPipeName(WCHAR * pipeName)
{
	m_PipeName = new WCHAR[100];
	wcscpy_s(m_PipeName, 100, pipeName);
}

void PipeServer::SetInstances(int instance)
{
	m_Instance = instance;
}

void PipeServer::SetBufferSize(int size)
{
	this->m_BufferSize = size;
}


DWORD PipeServer::PipeServerHostWorkerThread(LPVOID lpParam)
{
	IPipeServerMethod *pipeServerMethod = (IPipeServerMethod *) PipeServer::GetInstance();
	HANDLE pipeInstance = *((HANDLE *)lpParam);
	
	// Wait connect.
	OutputDebugString(L"Wait for client connection\n");
	while (WaitForSingleObject(m_ThreadStopEvent, 50) != WAIT_OBJECT_0)
	{
		OVERLAPPED overlapped;
		memset(&overlapped, 0, sizeof(overlapped));
		HANDLE multiEvent[2]{ CreateEvent(NULL, TRUE, TRUE, NULL), m_ThreadStopEvent };

		overlapped.hEvent = multiEvent[0];

		if (ConnectNamedPipe(pipeInstance, &overlapped))
		{
			OutputDebugString(L"ConnectNamedPipe failed\n");
			continue;
		}

		switch (GetLastError())
		{
				// The overlapped connection in progress. 
			case ERROR_IO_PENDING:
				OutputDebugString(L"GetLastError() : ERROR_IO_PENDING.\n");
				break;

				// Client is already connected, so signal an event. 
			case ERROR_PIPE_CONNECTED:
				OutputDebugString(L"GetLastError() : ConnectNamedPipe connected.\n");
				SetEvent(multiEvent[0]);

				break;

				// If an error occurs during the connect operation... 
			default:
			{
				OutputDebugString(L"GetLastError() : ConnectNamedPipe failed.\n");
				DisconnectNamedPipe(pipeInstance);
				continue;
			}
		}

		DWORD dwResult = WaitForMultipleObjects(2, multiEvent, FALSE, INFINITE);
		if (0 == dwResult - WAIT_OBJECT_0)
		{
			DWORD dwRet;
			unsigned char *buf = new unsigned char[sizeof(int) + m_BufferSize]{ 0 };
			if (GetOverlappedResult(pipeInstance, &overlapped, &dwRet, FALSE))
			{
				OutputDebugString(L"GetOverlappedResult : Client connected\n");
				while (WaitForSingleObject(m_ThreadStopEvent, 50) != WAIT_OBJECT_0)
				{

					if (pipeInstance)
					{
						OutputDebugString(L"ReadFile g_PipeInst In.....\n");
						DWORD byteRead = 0;
						OVERLAPPED overlap;
						memset(&overlap, 0, sizeof(overlap));
						overlap.hEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
						ReadFile(pipeInstance, buf, sizeof(int) + m_BufferSize, NULL, &overlap);
						
						if (GetLastError() == ERROR_PIPE_LISTENING)
						{
							OutputDebugString(L"No client is connected. continue\n");
							break;
						}
						if (GetLastError() == ERROR_BROKEN_PIPE)
						{
							OutputDebugString(L"ERROR_BROKEN_PIPE. continue\n");
							break;
						}
						if (GetLastError() == ERROR_PIPE_NOT_CONNECTED)
						{
							OutputDebugString(L"ERROR_PIPE_NOT_CONNECTED continue.\n");
							break;
						}

						if (GetLastError() == ERROR_IO_PENDING || GetLastError() == ERROR_SUCCESS)
						{
							HANDLE multiEvent[2] = { overlap.hEvent , m_ThreadStopEvent };
							DWORD  dwResult = WaitForMultipleObjects(2, multiEvent, FALSE, INFINITE);
							if (1 == dwResult - WAIT_OBJECT_0)
							{
								OutputDebugString(L"1 == dwResult - WAIT_OBJECT_0.\n");
								break;
							}
							if (dwResult == WAIT_OBJECT_0)
							{
								OutputDebugString(L"ReadFile g_PipeInst Out true.\n");
								OutputDebugString(L"ServiceWorkerThread : read data from client.\n");
								GetOverlappedResult(pipeInstance, &overlap, &byteRead, TRUE);
								if (byteRead >0)
								{
									PipeData data;
									data.iCommandId = *((int *)buf);
									data.data = buf + sizeof(int);
									OnReceiveData(pipeServerMethod, pipeInstance, &data);
								}
							} // end if (!ReadFile(.....
							else
							{
								OutputDebugString(L"ReadFile g_PipeInst Out Timeout.\n");
							}
						}
					}
				}
			}
			else
			{
				OutputDebugString(L"GetOverlappedResult : Client is not connected\n");
			}

			if (buf)
			{
				delete[] buf;
			}

			DisconnectNamedPipe(pipeInstance);
			continue;
		}


		// stop event
		else if (1 == dwResult - WAIT_OBJECT_0)
		{
			OutputDebugString(L"Stop event signaled.\n");
			break;
		}
		else
		{
			OutputDebugString(L"Wait client connecting timeout.....\n");
			continue;
		}
	}

	return 0;
}

void PipeServer::OnReceiveData(IPipeServerMethod *pipeServerMethod, HANDLE pipeInst, LPPipeData data)
{
	if (m_Factory)
	{
		std::unique_ptr<ICommandHandler> handler = m_Factory->GetCommandHandler(data->iCommandId);
		if (handler)
		{
			handler->Process(pipeServerMethod, pipeInst, data->data, sizeof(data->data));
		}
	}
	// write something back to client if no factory was set?
	// pipeServerMethod->SendData()
}

bool PipeServer::SendData(HANDLE pipeInst, LPPipeData data)
{
	DWORD dwWrite;
	bool result = false;
	OVERLAPPED overlapped;
	memset(&overlapped, 0, sizeof(overlapped));
	overlapped.hEvent = CreateEvent(NULL, TRUE, FALSE, NULL);

	if (pipeInst)
	{
		unsigned char *buffer = new unsigned char[sizeof(int) + m_BufferSize];

		*((int*)buffer) = data->iCommandId;

		if (!data->data)
		{
			data->data = new unsigned char[m_BufferSize] {0};
			memcpy_s(buffer + sizeof(int), m_BufferSize, data->data, m_BufferSize);
			delete[] data->data;
		}
		else
		{
			memcpy_s(buffer + sizeof(int), m_BufferSize, data->data, m_BufferSize);
		}

		WriteFile(pipeInst, buffer, sizeof(int) + m_BufferSize, NULL, &overlapped);
		if (GetLastError() == ERROR_IO_PENDING || GetLastError() == ERROR_SUCCESS)
		{
			if (WaitForSingleObject(overlapped.hEvent, INFINITE) == WAIT_OBJECT_0)
			{
				if (GetOverlappedResult(pipeInst, &overlapped, &dwWrite, TRUE))
				{
					result = true;
				}				
			}
		}

		delete[] buffer;
		return result;
	}

	return false;
}

bool PipeServer::SendData(LPPipeData data)
{
	DWORD dwWrite;
	bool result = false;
	OVERLAPPED overlapped;
	memset(&overlapped, 0, sizeof(overlapped));
	overlapped.hEvent = CreateEvent(NULL, TRUE, FALSE, NULL);


	unsigned char *buffer = new unsigned char[sizeof(int) + m_BufferSize];

	*((int*)buffer) = data->iCommandId;

	if (!data->data)
	{
		data->data = new unsigned char[m_BufferSize] {0};
		memcpy_s(buffer + sizeof(int), m_BufferSize, data->data, m_BufferSize);
		delete[] data->data;
	}
	else
	{
		memcpy_s(buffer + sizeof(int), m_BufferSize, data->data, m_BufferSize);
	}

	// broadcst to clients
	for (int i = 0; i < m_Instance; i++)
	{
		WriteFile(m_PipeInst[i], buffer, sizeof(int) + m_BufferSize, NULL, &overlapped);
		if (GetLastError() == ERROR_IO_PENDING || GetLastError() == ERROR_SUCCESS)
		{
			if (WaitForSingleObject(overlapped.hEvent, INFINITE) == WAIT_OBJECT_0)
			{
				if (GetOverlappedResult(m_PipeInst[i], &overlapped, &dwWrite, TRUE))
				{
					result = true;
				}
			}
		}
	}


	delete[] buffer;
	return result;


	return false;
}

void PipeServer::SetCommandFactory(ISimpleCommandFactory * factory)
{
	this->m_Factory = factory;
}

bool PipeServer::Start()
{
	ResetEvent(this->m_ThreadStopEvent);

	// pipe security
	SECURITY_ATTRIBUTES sa;
	SECURITY_DESCRIPTOR sd;
	InitializeSecurityDescriptor(&sd, SECURITY_DESCRIPTOR_REVISION);
	SetSecurityDescriptorDacl(&sd, TRUE, (PACL)NULL, FALSE);

	sa.nLength = (DWORD) sizeof(SECURITY_ATTRIBUTES);
	sa.lpSecurityDescriptor = (LPVOID)&sd;
	sa.bInheritHandle = TRUE;


	m_PipeInst = new HANDLE[m_Instance];

	for (int i = 0; i < m_Instance; i++)
	{
		// create pipe server
		m_PipeInst[i] = CreateNamedPipe(m_PipeName,                        // pipe name 
			PIPE_ACCESS_DUPLEX | FILE_FLAG_OVERLAPPED,					 // open mode , read/write access //
			PIPE_TYPE_MESSAGE | PIPE_READMODE_MESSAGE |
			PIPE_WAIT,							 // pipe mode
			m_Instance,            // number of instances
			PIPE_BUFSIZE * sizeof(TCHAR),				 // output buffer size 
			PIPE_BUFSIZE * sizeof(TCHAR),				 // input buffer size 
			PIPE_TIMEOUT,						 // client time-out
			&sa);

		if (m_PipeInst[i] == INVALID_HANDLE_VALUE)
		{
			OutputDebugString(L"CreateNamedPipe failed\n");
			return 0;
		}

		CreateThread(NULL, 0, this->PipeServerHostWorkerThread, (LPVOID) &(m_PipeInst[i]), 0, NULL);
	}


	

	return true;
}

void PipeServer::Stop()
{
	SetEvent(this->m_ThreadStopEvent);
}

