#include "PipeClientBase.h"


HANDLE PipeClientBase::readEvent = NULL;
IoProtocolPipeClient * PipeClientBase::protocol = NULL;
ON_DATA_RECEIVED  *PipeClientBase::OnDataReceived = NULL;

PipeClientBase::PipeClientBase(wchar_t *path)
{
	this->pipePath = new wchar_t[255];
	this->readEvent = CreateEvent(NULL, FALSE, FALSE, L"PipeClientReadEvent");
}

PipeClientBase::~PipeClientBase()
{
	if (this->pipePath)
		delete[] this->pipePath;

	if (this->protocol)
		delete this->protocol;

	CloseHandle(this->readEvent);
}

bool PipeClientBase::Connect()
{
	this->protocol = new IoProtocolPipeClient(this->pipePath);
	if (this->protocol)
	{
		if (this->protocol->OpenDevice())
		{
			if (this->OnPipeConnected)
			{
				(*this->OnPipeConnected)();
				CreateThread(NULL, 0, PipeClientReadThread, NULL, 0, NULL);
				return true;
			}
		}
		else
		{

		}
	}

	return false;
}

bool PipeClientBase::Disconnect()
{
	if (this->protocol)
	{
		SetEvent(this->readEvent);

		return this->protocol->CloseDevice();
	}

	return false;
}

DWORD WINAPI PipeClientBase::PipeClientReadThread(LPVOID lpParam)
{
	unsigned char buf[260];
	DWORD byteRead;

	while (WaitForSingleObject(readEvent, 200) == WAIT_TIMEOUT)
	{
		if (protocol)
		{
			if (protocol->Read(buf, 260, &byteRead))
			{
				PipeReceiveData data;
				data.commandId = (int)buf;
				memcpy_s(&(data.data), 256, buf + sizeof(int), 256);

				(*OnDataReceived)(&data);
			}
		}
	}
	return 0;
}