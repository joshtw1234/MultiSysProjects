#include <windows.ui.notifications.h>
//#include <wrl\implements.h>
#include <wrl\module.h>
#include "Utilitys.h"
#include "ToastEventHandler.h"

using namespace ABI::Windows::UI::Notifications;

ToastEventHandler::ToastEventHandler()
{
}

ToastEventHandler::ToastEventHandler(HWND hToActivate, HWND hEdit)
{
}

ToastEventHandler::ToastEventHandler(PipeServer * g_Srv)
{
}


ToastEventHandler::~ToastEventHandler()
{
}

IFACEMETHODIMP ToastEventHandler::Invoke(ABI::Windows::UI::Notifications::IToastNotification * sender, IInspectable * args)
{
#if true
	INT32 cmpRev = -1;
	HSTRING _Args, _strArg1, _strArg2;
	HSTRING_HEADER _head1, _head2;
	//
	IToastActivatedEventArgs * rootFrame;
	//
	HRESULT hr = args->QueryInterface(__uuidof(IToastActivatedEventArgs), (void **)&rootFrame);
	
	if (SUCCEEDED(hr))
	{
		hr = WindowsCreateStringReference(L"Cancel", 6, &_head1, &_strArg1);
		if (SUCCEEDED(hr))
		{
			hr = WindowsCreateStringReference(L"ReTry", 5, &_head2, &_strArg2);
			if (SUCCEEDED(hr))
			{
				hr = rootFrame->get_Arguments(&_Args);
				if (SUCCEEDED(hr))
				{
					hr = WindowsCompareStringOrdinal(_Args, _strArg1, &cmpRev);
					if (SUCCEEDED(hr))
					{
						if (0 == cmpRev)
						{
							//Click Cancel
						}
						else
						{
							hr = WindowsCompareStringOrdinal(_Args, _strArg2, &cmpRev);
							if (SUCCEEDED(hr))
							{
								if (0 == cmpRev)
								{
									//Click Try again
								}
								else
								{
									//Click UI
#if _DEBUG
									wchar_t *mainUI = L"..\\x64\\Debug\\Main32UI.exe";
									//wchar_t *mainUI = L"C:\\windows\\system32\\cmd.exe";
#else
									wchar_t *mainUI = _wfullpath(nullptr, L"Main32UI.exe", MAX_PATH);
#endif
									StartProcess(mainUI);
								}
							}
						}
					}
					
				}
			}
		}
	}
#else
	
#endif
	CoUninitialize();
	return S_OK;
}

IFACEMETHODIMP ToastEventHandler::Invoke(ABI::Windows::UI::Notifications::IToastNotification * sender, ABI::Windows::UI::Notifications::IToastDismissedEventArgs * e)
{
	ToastDismissalReason tdr;
	HRESULT hr = e->get_Reason(&tdr);
	if (SUCCEEDED(hr))
	{
		wchar_t *outputText;
		switch (tdr)
		{
		case ToastDismissalReason_ApplicationHidden:
			outputText = L"The application hid the toast using ToastNotifier.hide()";
			break;
		case ToastDismissalReason_UserCanceled:
			outputText = L"The user dismissed this toast";
			break;
		case ToastDismissalReason_TimedOut:
			outputText = L"The toast has timed out";
			break;
		default:
			outputText = L"Toast not activated";
			break;
		}

		/*  LRESULT succeeded = SendMessage(_hEdit, WM_SETTEXT, reinterpret_cast<WPARAM>(nullptr), reinterpret_cast<LPARAM>(outputText));
		hr = succeeded ? S_OK : E_FAIL;*/
	}
	CoUninitialize();
	return hr;
}

IFACEMETHODIMP ToastEventHandler::Invoke(ABI::Windows::UI::Notifications::IToastNotification * sender, ABI::Windows::UI::Notifications::IToastFailedEventArgs * e)
{
	/*LRESULT succeeded = SendMessage(_hEdit, WM_SETTEXT, reinterpret_cast<WPARAM>(nullptr), reinterpret_cast<LPARAM>(L"The toast encountered an error."));
	return succeeded ? S_OK : E_FAIL;*/
	return S_OK;
}
