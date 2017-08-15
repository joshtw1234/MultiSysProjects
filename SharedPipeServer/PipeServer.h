#pragma once
#include "windows.h"
#include "IPipeServerMethod.h"
#include "ISimpleCommandFactory.h"

#define PIPE_NAME L"\\\\.\\pipe\\HPCruiserPipe"
#define PIPE_BUFSIZE 4096
#define PIPE_TIMEOUT 5000

typedef void(*RECEIVE_PIPE_DATA_PROC)(unsigned char const * const, int);

class PipeServer : public IPipeServerMethod
{
	private:
		static WCHAR *m_PipeName;
		static int m_Instance;
		static ISimpleCommandFactory *m_Factory;
		static DWORD WINAPI PipeServerHostWorkerThread(LPVOID lpParam);
		static void OnReceiveData(IPipeServerMethod *pipeServerMethod, HANDLE pipeInst, LPPipeData data);
		static PipeServer *m_PipeServer;

		static HANDLE m_ThreadHandle;
		static HANDLE *m_PipeInst;
		static HANDLE m_ThreadStopEvent;
		static int m_BufferSize;

		PipeServer();
	public:


		static PipeServer* GetInstance();
		//PipeServer();
		//PipeServer(WCHAR *pipeName);
		//PipeServer(WCHAR *pipeName, int instance);
		virtual ~PipeServer();
		virtual bool SendData(HANDLE pipeInst, LPPipeData data);
		virtual bool SendData(LPPipeData data);
		void SetCommandFactory(ISimpleCommandFactory *factory);
		bool Start();
		void Stop();
		void SetPipeName(WCHAR *pipeName);
		void SetInstances(int instance);
		void SetBufferSize(int size);
};