//#pragma once
#ifndef UTILITYS_H
#define UTILITYS_H
#include <TlHelp32.h>
#include <netlistmgr.h>
#include "DebugLogOutput.h"

namespace
{
	DebugLogOutput g_DbgLogOutput(L"UpdateFunc.log");


	HKEY OpenKey(HKEY hRootKey, WCHAR* strKey, DWORD* outError)
	{
		HKEY hKey = 0;
		DWORD nError = RegOpenKeyEx(hRootKey, strKey, NULL, KEY_ALL_ACCESS, &hKey);
		*outError = nError;

		return hKey;
	}

	HKEY OpenKeyWOW6432(HKEY hRootKey, WCHAR* strKey, DWORD* outError)
	{
		HKEY hKey = 0;
		DWORD nError = RegOpenKeyEx(hRootKey, strKey, NULL, KEY_ALL_ACCESS | KEY_WOW64_32KEY, &hKey);
		*outError = nError;

		return hKey;
	}

	bool SetintVal(HKEY hKey, LPCTSTR lpValue, DWORD data)
	{
		LONG nError = RegSetValueEx(hKey, lpValue, NULL, REG_DWORD, (LPBYTE)&data, sizeof(DWORD));

		if (nError != ERROR_SUCCESS)
		{
			return false;
		}
		return true;
	}

	bool GetStrVal(HKEY hKey, LPCTSTR lpValue, WCHAR* outData, DWORD* outError)
	{
		DWORD size = MAX_PATH;
		DWORD type = REG_SZ;
		DWORD nError = RegQueryValueEx(hKey, lpValue, NULL, &type, (LPBYTE)outData, &size);
		*outError = nError;
		if (nError != ERROR_SUCCESS)
		{
			return false;
		}

		return true;

	}

	bool GetintVal(HKEY hKey, LPCTSTR lpValue, DWORD* outData, DWORD* outError)
	{
		DWORD size = sizeof(DWORD);
		DWORD type = REG_DWORD;
		DWORD nError = RegQueryValueEx(hKey, lpValue, NULL, &type, (LPBYTE)outData, &size);
		*outError = nError;
		if (nError != ERROR_SUCCESS)
		{
			return false;
		}

		return true;
	}
	/*
		@bref Query Service is runing.
		return -1 : Service Manager not avaliable.
		return -2 : Service not exist.
		return -3 : Service is not runing.
		return 0 : Service is runing.
	*/
	INT QueryWindowsService(WCHAR * strServiceName)
	{
		SERVICE_STATUS ServiceStatus;
		SC_HANDLE hSCM = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
		if (!hSCM)
			return -1;

		SC_HANDLE hOpenService = OpenService(hSCM, strServiceName, SC_MANAGER_ALL_ACCESS);
		if (GetLastError() == ERROR_SERVICE_DOES_NOT_EXIST)
		{
			return   -2;
		}
		QueryServiceStatus(hOpenService, &ServiceStatus);
		if (ServiceStatus.dwCurrentState != SERVICE_RUNNING)
		{
			//std::cout << "Failure  Status :" << ServiceStatus.dwCurrentState << std::endl;
			return -3;
		}

		CloseServiceHandle(hOpenService);
		CloseServiceHandle(hSCM);
		return 0;
	}
	//Worked
	void SetForegroundWindowInternalT(HWND hWnd)
	{
		if (!::IsWindow(hWnd)) return;

		//relation time of SetForegroundWindow lock
		DWORD lockTimeOut = 0;
		HWND  hCurrWnd = ::GetForegroundWindow();
		DWORD dwThisTID = ::GetCurrentThreadId(),
			dwCurrTID = ::GetWindowThreadProcessId(hCurrWnd, 0);

		//we need to bypass some limitations from Microsoft :)
		if (dwThisTID != dwCurrTID)
		{
			::AttachThreadInput(dwThisTID, dwCurrTID, TRUE);

			::SystemParametersInfo(SPI_GETFOREGROUNDLOCKTIMEOUT, 0, &lockTimeOut, 0);
			::SystemParametersInfo(SPI_SETFOREGROUNDLOCKTIMEOUT, 0, 0, SPIF_SENDWININICHANGE | SPIF_UPDATEINIFILE);

			::AllowSetForegroundWindow(ASFW_ANY);
		}

		::SetForegroundWindow(hWnd);

		if (dwThisTID != dwCurrTID)
		{
			::SystemParametersInfo(SPI_SETFOREGROUNDLOCKTIMEOUT, 0, (PVOID)lockTimeOut, SPIF_SENDWININICHANGE | SPIF_UPDATEINIFILE);
			::AttachThreadInput(dwThisTID, dwCurrTID, FALSE);
		}
	}
	//Failed
	void SetForegroundWindowInternal(HWND hWnd)
	{
		if (!::IsWindow(hWnd)) return;

		BYTE keyState[256] = { 0 };
		//to unlock SetForegroundWindow we need to imitate Alt pressing
		if (::GetKeyboardState((LPBYTE)&keyState))
		{
			if (!(keyState[VK_MENU] & 0x80))
			{
				::keybd_event(VK_MENU, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
			}
		}

		::SetForegroundWindow(hWnd);

		if (::GetKeyboardState((LPBYTE)&keyState))
		{
			if (!(keyState[VK_MENU] & 0x80))
			{
				::keybd_event(VK_MENU, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
			}
		}
	}

	BOOL CALLBACK EnumWindowsProcMy(HWND hwnd, LPARAM lParam)
	{
		DWORD lpdwProcessId;
		WCHAR wnd_title[MAX_PATH];
		GetWindowThreadProcessId(hwnd, &lpdwProcessId);
		if (lpdwProcessId == lParam)
		{
			GetWindowText(hwnd, wnd_title, MAX_PATH);
			if (0 == wcscmp(L"MainWindow", wnd_title))
			{
				//SetForegroundWindow(hwnd);
				SetForegroundWindowInternalT(hwnd);
				return FALSE;
			}
		}
		return TRUE;
	}

	DWORD StartProcessByPath(wchar_t * procPath)
	{
		STARTUPINFO lpStartupInfo;
		PROCESS_INFORMATION lpProcessInfo;

		memset(&lpStartupInfo, 0, sizeof(lpStartupInfo));
		memset(&lpProcessInfo, 0, sizeof(lpProcessInfo));
		return CreateProcess(procPath, NULL, NULL, NULL, FALSE, CREATE_DEFAULT_ERROR_MODE, NULL, NULL, &lpStartupInfo, &lpProcessInfo);
	}

	bool FindProcessByName(wchar_t * filename, HANDLE * procHandlee, DWORD * procID)
	{
		bool rev = false;
		HANDLE hSnapShot = CreateToolhelp32Snapshot(TH32CS_SNAPALL, NULL);
		PROCESSENTRY32 pEntry;
		pEntry.dwSize = sizeof(pEntry);
		BOOL hRes = Process32First(hSnapShot, &pEntry);
		while (hRes)
		{
			if (wcscmp(pEntry.szExeFile, filename) == 0)
			{
				rev = true;
				*procHandlee = OpenProcess(PROCESS_TERMINATE, 0, pEntry.th32ProcessID);
				*procID = pEntry.th32ProcessID;
				break;
			}
			hRes = Process32Next(hSnapShot, &pEntry);
		}
		CloseHandle(hSnapShot);
		return rev;
	}

	void StartProcess(wchar_t * fullProcPath)
	{
		wchar_t * fName = PathFindFileName(fullProcPath);
		HANDLE procHan = NULL;
		DWORD procID = -1;
		if (FindProcessByName(fName, &procHan, &procID))
		{
			::StringCchPrintfW(g_DbgLogOutput.strDebugLogOutputString, DEBUG_MAX_STRING, L"%s::Find Proc %s", __FUNCTIONW__, fName);
			g_DbgLogOutput.Output();
			if (NULL != procHan)
			{
				EnumWindows(EnumWindowsProcMy, procID);
			}
		}
		else
		{
			//wchar_t * lpApplicationName = L"C:\\Windows\\System32\\cmd.exe"; /* The program to be executed */
			DWORD rev = StartProcessByPath(fullProcPath);
			if (0 == rev)
			{
				rev = GetLastError();
			}

		}
	}

	
	void killProcessByName(wchar_t * filename)
	{
		HANDLE hSnapShot = CreateToolhelp32Snapshot(TH32CS_SNAPALL, NULL);
		PROCESSENTRY32 pEntry;
		pEntry.dwSize = sizeof(pEntry);
		BOOL hRes = Process32First(hSnapShot, &pEntry);
		while (hRes)
		{
			if (wcscmp(pEntry.szExeFile, filename) == 0)
			{
				HANDLE hProcess = OpenProcess(PROCESS_TERMINATE, 0,
					(DWORD)pEntry.th32ProcessID);
				if (hProcess != NULL)
				{
					TerminateProcess(hProcess, 9);
					CloseHandle(hProcess);
				}
			}
			hRes = Process32Next(hSnapShot, &pEntry);
		}
		CloseHandle(hSnapShot);
	}

	bool GetNetworkConnection()
	{
		CoInitialize(NULL);
		INetworkListManager * networkMgr;
		HRESULT hr = CoCreateInstance(CLSID_NetworkListManager, NULL, CLSCTX_ALL, IID_INetworkListManager, (LPVOID*)&networkMgr);
		if (SUCCEEDED(hr))
		{
			
			NLM_CONNECTIVITY netWork;
			hr = networkMgr->GetConnectivity(&netWork);
			if (SUCCEEDED(hr))
			{
				switch (netWork)
				{
				case NLM_CONNECTIVITY_DISCONNECTED:
					return false;
					break;
				case NLM_CONNECTIVITY_IPV4_NOTRAFFIC:
					break;
				case NLM_CONNECTIVITY_IPV6_NOTRAFFIC:
					break;
				case NLM_CONNECTIVITY_IPV4_SUBNET:
					break;
				case NLM_CONNECTIVITY_IPV4_LOCALNETWORK:
					break;
				case NLM_CONNECTIVITY_IPV4_INTERNET:
					break;
				case NLM_CONNECTIVITY_IPV6_SUBNET:
					break;
				case NLM_CONNECTIVITY_IPV6_LOCALNETWORK:
					break;
				case NLM_CONNECTIVITY_IPV6_INTERNET:
					break;
				default:
					break;
				}
			}
		}
		CoUninitialize();
		return true;

	}
	
}
#endif /* UTILITYS_H */