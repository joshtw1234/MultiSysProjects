#pragma once
#include "IoProtocolPipeClient.h"

typedef struct 
{
public:
	int commandId;
	unsigned char data[256];
}PipeReceiveData, *LPPipeReceiveData;

// 
typedef void(*ON_PIPE_CONNECTED)();
typedef void(*ON_DATA_RECEIVED)(PipeReceiveData *pipeData);

class PipeClientBase
{
	private:
		wchar_t *pipePath;
		static HANDLE readEvent;
		static DWORD WINAPI PipeClientReadThread(LPVOID lpParam);

	protected:
		static IoProtocolPipeClient *protocol;

		PipeClientBase(wchar_t *path);

	public:
		ON_PIPE_CONNECTED *OnPipeConnected;
		static ON_DATA_RECEIVED  *OnDataReceived;

		virtual ~PipeClientBase();
		bool Connect();
		bool Disconnect();
};