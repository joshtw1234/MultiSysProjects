// UpdateFunc.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "Utilitys.h"
#include "PipeServer.h"
#include "SimpleCommandFactoryImp.h"
#include "CommandHandler.h"
#include "UpdateFunc.h"

#define AMD_BUFFER_SIZE 512
PipeServer *g_PipeServer;
SimpleCommandFactoryImp *g_SimpleCommandFactoryImp = new SimpleCommandFactoryImp();

WCHAR HPSARegPath[MAX_PATH] = L"SOFTWARE\\Hewlett-Packard\\HPActiveSupport";
WCHAR OMENUpdatPath[MAX_PATH] = L"SOFTWARE\\HP\\OMEN Ally\\Settings\\Updates";
//
DWORD IsHPSAInstalled = 0, IsHPSASrvRunning = 0;

bool CheckHPSAInstalled()
{
	bool rev = false;
	DWORD dwErr = 0, dwRetVal = 0;
	WCHAR sz[MAX_PATH] = {};
	HKEY hAMDKey = OpenKeyWOW6432(HKEY_LOCAL_MACHINE, HPSARegPath, &dwErr);
	if (NULL != hAMDKey && GetStrVal(hAMDKey, L"version", sz, &dwErr))
	{
		rev = true;
	}
	else
	{
		::StringCchPrintfW(g_DbgLogOutput.strDebugLogOutputString, DEBUG_MAX_STRING, L"%s::HPSA not Installed", __FUNCTIONW__);
		g_DbgLogOutput.Output();
	}
	RegCloseKey(hAMDKey);
	return rev;
}

void GetForeGroundWind()
{
	WCHAR wnd_title[MAX_PATH];
	HWND hwnd = GetForegroundWindow(); // get handle of currently active window
	GetWindowText(hwnd, wnd_title, MAX_PATH);
	if (wcslen(wnd_title) != 0 && wcscmp(L"Task Switching", wnd_title) != 0 && wcscmp(L"New notification", wnd_title) != 0)
	{
		//tos.DisplayToast(g_PipeServer, wnd_title);
	}
	else
	{
		//tos.DisplayToast(L"None");
	}

}

UPDATEFUNC_API void Initialize(void)
{
	if (CheckHPSAInstalled())
	{
		IsHPSAInstalled = 1;
	}

	DWORD regResult = 0;
	HKEY hKeyUpdate = OpenKey(HKEY_CURRENT_USER, OMENUpdatPath, &regResult);
	if (NULL == hKeyUpdate)
	{
		regResult = RegCreateKeyW(HKEY_CURRENT_USER, OMENUpdatPath, &hKeyUpdate);
		if (ERROR_SUCCESS == regResult)
		{
			SetintVal(hKeyUpdate, L"HPSAInstalled", IsHPSAInstalled);
		}
	}
	else
	{
		SetintVal(hKeyUpdate, L"HPSAInstalled", IsHPSAInstalled);
	}

	wchar_t * errMsg = L"";
	//Check HPSA Service is running.
	int qRev = QueryWindowsService(L"HPSupportSolutionsFrameworkService");
	switch (qRev)
	{
	case 0:
		errMsg = L"HPSA is runing.";
		IsHPSASrvRunning = 1;
		break;
	case -1:
		errMsg = L"Can not access Service Manager.";
		break;
	case -2:
		errMsg = L"HPSA not Exist.";
		break;
	case -3:
		errMsg = L"HPSA is not runing.";
		break;
	default:
		break;
	}
	::StringCchPrintfW(g_DbgLogOutput.strDebugLogOutputString, DEBUG_MAX_STRING, L"%s::%s", __FUNCTIONW__, errMsg);
	g_DbgLogOutput.Output();

	if (qRev < 0)
	{
		SetintVal(hKeyUpdate, L"HPSARunning", IsHPSASrvRunning);
		RegCloseKey(hKeyUpdate);
		return;
	}

	SetintVal(hKeyUpdate, L"HPSARunning", IsHPSASrvRunning);
	RegCloseKey(hKeyUpdate);

	g_PipeServer = PipeServer::GetInstance();
	if (g_PipeServer)
	{
		g_PipeServer->SetCommandFactory(g_SimpleCommandFactoryImp);
		g_PipeServer->SetPipeName(L"\\\\.\\pipe\\MyUpdate");
		g_PipeServer->SetInstances(2);
		g_PipeServer->SetBufferSize(AMD_BUFFER_SIZE);
		g_PipeServer->Start();

	}

	CommandHandlerInitHPSAThreadProc *cmd = new CommandHandlerInitHPSAThreadProc();
	cmd->Process(g_PipeServer, NULL, NULL, 0);
	delete cmd;
}

UPDATEFUNC_API void Uninitialize(void)
{
	//return UPDATEFUNC_API void();

}

UPDATEFUNC_API bool IsSupport(void)
{
	return true;
	//return UPDATEFUNC_API bool();
}

UPDATEFUNC_API void OMENPwerChange(void)
{
	int thisisi = 0;
	//tos.DisplayToast();
}

UPDATEFUNC_API void ForeGroundChange(void)
{
	//return UPDATEFUNC_API void();
	if (IsHPSAInstalled && IsHPSASrvRunning)
	{
		GetForeGroundWind();
	}
}