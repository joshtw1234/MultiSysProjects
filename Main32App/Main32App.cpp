// Main32App.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "Main32App.h"
#include <vector>
#include <strsafe.h>
#include <Shlwapi.h>
#pragma comment(lib, "Shlwapi.lib")


#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;                                // current instance
HWND hWnd;
WCHAR szTitle[MAX_LOADSTRING];                  // The title bar text
WCHAR szWindowClass[MAX_LOADSTRING];            // the main window class name
HWINEVENTHOOK g_hook;

// Forward declarations of functions included in this code module:
ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);
// Callback function that handles events.
//
void CALLBACK HandleWinEvent(HWINEVENTHOOK hook, DWORD event, HWND hwnd, LONG idObject, LONG idChild, DWORD dwEventThread, DWORD dwmsEventTime);
//std::vector<std::wstring> vsDll;
std::vector<HMODULE> vsDll;
//Func declaration
int LoadFuncDll();
void SendSpecialEvent(int methodIdx, LPCSTR strEvent);
//Module Export Func
typedef int(__stdcall *FUNC_DLL_Initialize)(void);
//typedef int(__stdcall *FUNC_DLL_InitializeB)(HWND* mHWND);
typedef int(__stdcall *FUNC_DLL_UnInitialize)(void);
typedef bool(__stdcall *FUNC_DLL_IsSupport)(void);
typedef bool(__stdcall *FUNC_DLL_Resume)(void);
typedef bool(__stdcall *FUNC_DLL_OMENPwerChange)(void);
typedef bool(__stdcall *FUNC_DLL_ForeGroundChange)(void);


int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    // TODO: Place code here.

    // Initialize global strings
    LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadStringW(hInstance, IDC_MAIN32APP, szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);

    // Perform application initialization:
    if (!InitInstance (hInstance, nCmdShow))
    {
        return FALSE;
    }

    HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_MAIN32APP));

    MSG msg;

    // Main message loop:
    while (GetMessage(&msg, nullptr, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }

    return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
    WNDCLASSEXW wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style          = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc    = WndProc;
    wcex.cbClsExtra     = 0;
    wcex.cbWndExtra     = 0;
    wcex.hInstance      = hInstance;
    wcex.hIcon          = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_MAIN32APP));
    wcex.hCursor        = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName   = MAKEINTRESOURCEW(IDC_MAIN32APP);
    wcex.lpszClassName  = szWindowClass;
    wcex.hIconSm        = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    return RegisterClassExW(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   hInst = hInstance; // Store instance handle in our global variable
   DWORD dwStyle = 0;
   DWORD dwStyleEx = 0;
   dwStyle |= WS_POPUP;     // no border or title bar
   dwStyleEx |= WS_EX_LAYERED;// | WS_EX_TOPMOST;// | WS_EX_NOACTIVATE;   // transparent, topmost, with no taskbar item
   dwStyleEx |= WS_EX_TOPMOST;
   dwStyleEx |= WS_EX_TOOLWINDOW;
   dwStyleEx |= WS_EX_NOACTIVATE;
#if true
   //Custmize window
   hWnd = CreateWindowEx(
	   dwStyleEx,
	   szWindowClass,
	   szTitle,
	   dwStyle,
	   100, 100,
	   300, 300,
	   NULL,
	   NULL,
	   hInstance,
	   NULL
   );
   
#else
   //Stander window
   HWND hWnd = CreateWindowW(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, nullptr, nullptr, hInstance, nullptr);
#endif
   g_hook = SetWinEventHook(
	   EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND,  // Range of events (4 to 5).
	   NULL,                                          // Handle to DLL.
	   HandleWinEvent,                                // The callback.
	   0, 0,              // Process and thread IDs of interest (0 = all)
	   WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS); // Flags.

   if (!hWnd)
   {
      return FALSE;
   }

#if true
   ShowWindow(hWnd, SW_HIDE);
#else
   ShowWindow(hWnd, nCmdShow);
#endif
   UpdateWindow(hWnd);
   return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND  - process the application menu
//  WM_PAINT    - Paint the main window
//  WM_DESTROY  - post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
    case WM_COMMAND:
        {
            //int wmId = LOWORD(wParam);
            //// Parse the menu selections:
            //switch (wmId)
            //{
            //case IDM_ABOUT:
            //    DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
            //    break;
            //case IDM_EXIT:
            //    DestroyWindow(hWnd);
            //    break;
            //default:
            //    return DefWindowProc(hWnd, message, wParam, lParam);
            //}
        }
        break;
    case WM_PAINT:
        {
            //PAINTSTRUCT ps;
            //HDC hdc = BeginPaint(hWnd, &ps);
            //// TODO: Add any drawing code that uses hdc here...
            //EndPaint(hWnd, &ps);
        }
        break;
	case WM_CREATE:
		LoadFuncDll();
		break;
    case WM_DESTROY:
		HMODULE hdll;
		for (int i = 0; i < vsDll.size(); i++)
		{
			hdll = vsDll[i];
			FUNC_DLL_UnInitialize uninitialize = (FUNC_DLL_UnInitialize)GetProcAddress(hdll, "Uninitialize");
			uninitialize();
			free(hdll);
		}
		vsDll.clear();
		
        PostQuitMessage(0);
        break;
	case WM_POWERBROADCAST:
		switch (wParam)
		{
		case PBT_APMRESUMEAUTOMATIC:

			break;
		case PBT_APMPOWERSTATUSCHANGE:
			SendSpecialEvent(1, "OMENPwerChange");
			break;
		default:
			break;
		}
		break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}

void CALLBACK HandleWinEvent(HWINEVENTHOOK hook, DWORD event, HWND hwnd, LONG idObject, LONG idChild, DWORD dwEventThread, DWORD dwmsEventTime)
{
	switch (event)
	{
	case EVENT_SYSTEM_FOREGROUND:
		SendSpecialEvent(2, "ForeGroundChange");
		break;
	default:
		break;
	}
}

int LoadFuncDll()
{
	wchar_t buffer[MAX_PATH], strModulePath[MAX_PATH], strModuleDllFullPathName[MAX_PATH];
	GetModuleFileName(NULL, buffer, MAX_PATH);
	PathRemoveFileSpec(buffer);
	StringCchPrintf(strModulePath, MAX_PATH, L"%s\\Modules\\Func\\*.dll", buffer);

	HANDLE hFind;
	WIN32_FIND_DATA data;

	hFind = FindFirstFile(strModulePath, &data);
	if (hFind != INVALID_HANDLE_VALUE) {
		do {
			StringCchPrintf(strModuleDllFullPathName, MAX_PATH, L"%s\\Modules\\Func\\%s", buffer, data.cFileName);
			// load dll
			HMODULE hDll = LoadLibrary(strModuleDllFullPathName);
			if (hDll)
			{
				// cal IsSupport
				FUNC_DLL_IsSupport isSupport = (FUNC_DLL_IsSupport)GetProcAddress(hDll, "IsSupport");
				if (isSupport && isSupport())
				{
					FUNC_DLL_Initialize Initialize = (FUNC_DLL_Initialize)GetProcAddress(hDll, "Initialize");
					if (Initialize)
					{
						Initialize();
						//InitializeB(&hWnd);
						vsDll.push_back(hDll);
					}
				}
			}
			
		} while (FindNextFile(hFind, &data));
		FindClose(hFind);
	}
	return 0;
}

void SendSpecialEvent(int methodIdx, LPCSTR strEvent)
{
	FUNC_DLL_Resume resume = NULL;
	FUNC_DLL_OMENPwerChange pwChange = NULL;
	FUNC_DLL_ForeGroundChange wndChange = NULL;
	for (int i = 0; i < vsDll.size(); i++)
	{
		switch (methodIdx)
		{
		case 0:
			resume = (FUNC_DLL_Resume)GetProcAddress(vsDll[i], strEvent);
			if (resume)
			{
				resume();
			}
			break;
		case 1:
			pwChange = (FUNC_DLL_OMENPwerChange)GetProcAddress(vsDll[i], strEvent);
			if (pwChange)
			{
				pwChange();
			}
			break;
		case 2:
			wndChange = (FUNC_DLL_ForeGroundChange)GetProcAddress(vsDll[i], strEvent);
			if (wndChange)
			{
				wndChange();
			}
			break;
		default:
			break;
		}

	}
}
