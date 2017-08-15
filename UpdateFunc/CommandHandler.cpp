#include <windows.h>
#include <string>
#include "CommandHandler.h"
#if _DEBUG
#else
#include "HPSALibWarper.h"
#endif
#include "ToastMsg.h"
#include "Utilitys.h"

#define AMD_BUFFER_SIZE 512

#pragma region Command CPP methods
CToastMsg toast;
#if _DEBUG
#else
HPSALibWarper * hpsa = new HPSALibWarper();
#endif
std::string GetPipeDataToString(unsigned char * data, int dataSize)
{
	unsigned char tmpBuf[24] = {};
	memcpy_s(tmpBuf, 24, data, dataSize);
	std::string revStr(tmpBuf, tmpBuf + dataSize);
	revStr = revStr.erase(0, 1);
	return revStr;
}

std::wstring BSTRtoSTDWstring(wchar_t * wStr)
{
	std::wstring tmpStr(wStr);
	return tmpStr;
}
//
void SendPipeData(IPipeServerMethod * pipeServerMethod, HANDLE pipeInst, int commID, std::wstring sendStr)
{
	PipeData pipedata = {};

	pipedata.iCommandId = commID;
	pipedata.data = new unsigned char[AMD_BUFFER_SIZE] {};
	memcpy_s(pipedata.data, AMD_BUFFER_SIZE, sendStr.c_str(), (sendStr.length() + 1) * sizeof(wchar_t));
	if (NULL != pipeInst)
	{
		pipeServerMethod->SendData(pipeInst, &pipedata);
	}
	else
	{
		pipeServerMethod->SendData(&pipedata);
	}
	OutputDebugString(L"Send Frequency.");
	if (pipedata.data)
	{
		delete[] pipedata.data;
	}
}
//
void GetOMENUpdate(int rIdx, IPipeServerMethod * pipeServerMethod, HANDLE pipeInst)
{
	wchar_t revVal[MAX_PATH] = {};
	int uPri = -1, revIdx = -1;
#ifdef _DEBUG
	revIdx = 2;
#else
	//revIdx = 2;
	revIdx = hpsa->GetOMENUpdate(rIdx, &uPri);
#endif
	wsprintf(revVal, L"%d,%d", revIdx, uPri);
	switch (revIdx)
	{
	case 0:
		//HPSA is running.
		toast.DisplayToast(0);
		break;
	case 1:
		//There is OMEN Update 
		break;
	case 2:
		//There is no OMEN Update.
#if _DEBUG
		toast.DisplayToast(1);
#endif
		break;
	}

	SendPipeData(pipeServerMethod, pipeInst, COMMAND_GetOMENUpdate, revVal);
}
//
IPipeServerMethod * _pipeServerMethod;
#if _DEBUG
VOID CALLBACK CallBackCheckNetWorkProcc(
	HWND hwnd,        // handle to window for timer messages 
	UINT message,     // WM_TIMER message 
	UINT idTimer,     // timer identifier 
	DWORD dwTime)     // current system time 
{
	if (GetNetworkConnection())
	{
		SendPipeData(_pipeServerMethod, NULL, COMMAND_GetNetWork, L"1");
	}
	else
	{
		SendPipeData(_pipeServerMethod, NULL, COMMAND_GetNetWork, L"0");
	}
}
#else
#pragma region Thread Methods

wchar_t revVal[MAX_PATH] = {};
bool hpsaStatus = false;
bool netState = false;

DWORD WINAPI CheckHPSAThreadProc(
	_In_ LPVOID lpParameter
)
{
	
	while (true)
	{
		Sleep(2000);
		if (hpsaStatus != hpsa->IsHPSARunning())
		{

			hpsaStatus = !hpsaStatus;
			if (hpsaStatus)
			{
				//HPSA running analyze
				//Send notify to UI
				SendPipeData((IPipeServerMethod *)lpParameter, NULL, COMMAND_IsHPSARunning, L"1");
			}
			else
			{
				//HPSA is not runing analyze
				//Send notify to UI
				SendPipeData((IPipeServerMethod *)lpParameter, NULL, COMMAND_IsHPSARunning, L"0");
				//Get OMEN Update
				GetOMENUpdate(0, (IPipeServerMethod *)lpParameter, NULL);
			}
		}

	}

	return 0;
}

DWORD WINAPI AutoCheckHPSAThreadProc(
	_In_ LPVOID lpParameter
)
{
	wchar_t revVal[MAX_PATH] = {};

	while (true)
	{
		//Every 2 secs check last HPSA analyze time.
		Sleep(2000);
		
		if (!hpsa->IsHPSARunning() && hpsa->GetHPSALastAnalysis()> 90)
		{
			//If Last HPSA analyze time over 2 minutes, start to run HPSA analyze.
			if (hpsa->HPSARunAnalysis())
			{
				toast.DisplayToast(0);
			}
		}
	}

	return 0;
}

VOID CALLBACK CallBackAutoRunHPSAProc(
	HWND hwnd,        // handle to window for timer messages 
	UINT message,     // WM_TIMER message 
	UINT idTimer,     // timer identifier 
	DWORD dwTime)     // current system time 
{
	
	
	if (netState != GetNetworkConnection())
	{
		//Network connect status change
		netState = !netState;
		if (netState)
		{
			SendPipeData(_pipeServerMethod, NULL, COMMAND_GetNetWork, L"1");
		}
		else
		{
			SendPipeData(_pipeServerMethod, NULL, COMMAND_GetNetWork, L"0");
		}
	}
	else
	{
		//Network connect status no change
		if (netState && !hpsa->IsHPSARunning() && hpsa->GetHPSALastAnalysis() > 90)
		{
			//If Last HPSA analyze time over 2 minutes, start to run HPSA analyze.
			if (hpsa->HPSARunAnalysis())
			{
				toast.DisplayToast(0);
			}
		}
	}

}

VOID CALLBACK CallBackCheckHPSAIsRunningProcc(
	HWND hwnd,        // handle to window for timer messages 
	UINT message,     // WM_TIMER message 
	UINT idTimer,     // timer identifier 
	DWORD dwTime)     // current system time 
{
	if (hpsaStatus != hpsa->IsHPSARunning())
	{
		hpsaStatus = !hpsaStatus;
		if (hpsaStatus)
		{
			//HPSA running analyze
			//Send notify to UI
			SendPipeData(_pipeServerMethod, NULL, COMMAND_IsHPSARunning, L"1");
		}
		else
		{
			//HPSA is not runing analyze
			//Send notify to UI
			SendPipeData(_pipeServerMethod, NULL, COMMAND_IsHPSARunning, L"0");
			//Get OMEN Update
			GetOMENUpdate(0, _pipeServerMethod, NULL);
		}
	}
}
#pragma endregion
#endif
#pragma endregion

#pragma region CommandHandlerTuning

CommandHandlerIsOverClockAble::~CommandHandlerIsOverClockAble()
{
}

void CommandHandlerIsOverClockAble::Process(IPipeServerMethod * pipeServerMethod, HANDLE pipeInst, unsigned char * data, int size)
{

	//SendPipeData(pipeServerMethod, pipeInst, COMMAND_X, L"This is Josh from FUnc.");
	toast.DisplayToast(0);

}

CommandHandlerGetkOMENUpdate::~CommandHandlerGetkOMENUpdate()
{
}

void CommandHandlerGetkOMENUpdate::Process(IPipeServerMethod * pipeServerMethod, HANDLE pipeInst, unsigned char * data, int size)
{
	if (GetNetworkConnection())
	{
		GetOMENUpdate(1, pipeServerMethod, pipeInst);
	}
	else
	{
		SendPipeData(_pipeServerMethod, NULL, COMMAND_GetNetWork, L"0");
	}
}


CommandHandlerInitHPSAThreadProc::~CommandHandlerInitHPSAThreadProc()
{
}

void CommandHandlerInitHPSAThreadProc::Process(IPipeServerMethod * pipeServerMethod, HANDLE pipeInst, unsigned char * data, int size)
{
	DWORD revVal = 0;
	if (!_pipeServerMethod)
	{
		_pipeServerMethod = pipeServerMethod;
	}
#if _DEBUG
	GetOMENUpdate(1, pipeServerMethod, pipeInst);
	// Set the timer for Check HPSA Status
	HRESULT uResult = SetTimer(NULL,									// handle to main window 
		2,										// timer identifier 
		1000,									// 10-second interval 
		(TIMERPROC)CallBackCheckNetWorkProcc); // timer callback 
#else
	//GetOMENUpdate(pipeServerMethod, pipeInst);
	//CreateThread(NULL, NULL, CheckHPSAThreadProc, pipeServerMethod, NULL, &revVal);
	//CreateThread(NULL, NULL, AutoCheckHPSAThreadProc, pipeServerMethod, NULL, &revVal);

	// Set the timer for Run HPSA
	HRESULT uResult = SetTimer(NULL,									// handle to main window 
							   1,										// timer identifier 
							   2000,									// 10-second interval 
							   (TIMERPROC)CallBackAutoRunHPSAProc); // timer callback 
	// Set the timer for Check HPSA Status
	uResult = SetTimer(NULL,									// handle to main window 
					   2,										// timer identifier 
					   1000,									// 10-second interval 
					   (TIMERPROC)CallBackCheckHPSAIsRunningProcc); // timer callback 
	
#endif

}
#pragma endregion

CommandHandlerIsHPSARunning::~CommandHandlerIsHPSARunning()
{
}

void CommandHandlerIsHPSARunning::Process(IPipeServerMethod * pipeServerMethod, HANDLE pipeInst, unsigned char * data, int size)
{
	wchar_t * HPSARunning = L"0";
	if (GetNetworkConnection())
	{
#if _DEBUG
#else
		if (hpsa->IsHPSARunning())
		{
			HPSARunning = L"1";
		}
#endif

		SendPipeData(pipeServerMethod, pipeInst, COMMAND_IsHPSARunning, HPSARunning);
	}
	else
	{
		SendPipeData(_pipeServerMethod, NULL, COMMAND_GetNetWork, L"0");
	}
}
