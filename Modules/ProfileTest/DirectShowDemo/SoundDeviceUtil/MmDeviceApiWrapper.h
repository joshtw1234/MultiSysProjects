#pragma once
#include "Mmdeviceapi.h"
#include "PolicyConfig.h"
#include "Propidl.h"
#include "NotificationClient.h"
#include "AudioDevice.h"

namespace AudioDeviceUtil {

    ref class MmDeviceApiWrapper
    {
    public:
        MmDeviceApiWrapper(void)
        {
            //.Net threads are coinitialized...
            //CoInitializeEx(NULL, COINIT_MULTITHREADED);
            notificationRegistered = false;
            audioDeviceNotificationHelper = gcnew AudioDeviceNotificationHelper();
            pNotifyClient = NULL;

        }

        virtual ~MmDeviceApiWrapper(void)
        {
            //CoUninitialize();
            if (notificationRegistered)
                RegisterForNotification(false);
        }

        static property AudioDeviceNotificationHelper^ AudioDeviceNotification
        {
            AudioDeviceNotificationHelper^ get()
            {
                return audioDeviceNotificationHelper;
            }
        };

        static property bool IsRegisteredForNotification
        {
            bool get()
            {
                return notificationRegistered;
            }
        }

        // Enumerates playback device list and marks the default device by the appropriate flag
        static System::Collections::Generic::List<AudioDevice^>^ GetPlaybackDeviceList() 
        {
            System::Collections::Generic::List<AudioDevice^>^ playbackDevices = 
                gcnew System::Collections::Generic::List<AudioDevice^>();

            HRESULT hr = S_OK;//CoInitialize(NULL);
            HRESULT hr2 = S_OK; 
            if (SUCCEEDED(hr))
            {
                IMMDeviceEnumerator *pEnum = NULL;
                // Create a multimedia device enumerator.
                hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL,                
                    CLSCTX_ALL, __uuidof(IMMDeviceEnumerator), (void**)&pEnum);
                if (SUCCEEDED(hr))
                {
                    IMMDevice *pDevice;
                    IMMDeviceCollection *pDevices;
                    LPWSTR wstrDefaultID = L"";
                    // Enumerate the output devices.
                    hr = pEnum->EnumAudioEndpoints(eRender, 
                        DEVICE_STATE_ACTIVE | DEVICE_STATE_UNPLUGGED | DEVICE_STATE_DISABLED, &pDevices);
                    if (SUCCEEDED(hr))
                    {
                        HRESULT hrDef = pEnum->GetDefaultAudioEndpoint(eRender, eConsole, &pDevice);
                        if (SUCCEEDED(hrDef))
                        {
                            hrDef = pDevice->GetId(&wstrDefaultID);
                        }
                        System::Diagnostics::Trace::WriteLineIf(!SUCCEEDED(hrDef),
                            System::String::Format("[MmDeviceApiWrapper] GetDefaultAudioEndPoint failed: {0:X}", hr), "Error");
                    }

                    if (SUCCEEDED(hr))
                    {                                                  
                        UINT count;
                        pDevices->GetCount(&count);
                        if (SUCCEEDED(hr))
                        {
                            for (unsigned int i = 0; i < count; i++)
                            {
                                hr = pDevices->Item(i, &pDevice);
                                if (SUCCEEDED(hr))
                                {
                                    LPWSTR wstrID = NULL;
                                    DWORD dwState = 0;
                                    hr = pDevice->GetId(&wstrID);
                                    hr2 = pDevice->GetState(&dwState);
                                    if (SUCCEEDED(hr) && SUCCEEDED(hr2))
                                    {
                                        IPropertyStore *pStore;
                                        hr = pDevice->OpenPropertyStore(STGM_READ, &pStore);
                                        if (SUCCEEDED(hr))
                                        {
                                            PROPVARIANT friendlyName;
                                            PropVariantInit(&friendlyName);
                                            hr = pStore->GetValue(PKEY_Device_FriendlyName, &friendlyName);
                                            if (SUCCEEDED(hr))
                                            {
                                                System::String^ name = gcnew System::String(friendlyName.pwszVal);
                                                playbackDevices->Add(gcnew AudioDevice(gcnew System::String(wstrID), 
                                                    name, (AudioDeviceStateType)dwState, 0 == wcscmp(wstrID, wstrDefaultID)));
                                                PropVariantClear(&friendlyName);
                                            }
                                            pStore->Release();
                                        }
                                    }
                                    System::Diagnostics::Trace::WriteLineIf(!(SUCCEEDED(hr) && SUCCEEDED(hr2)),
                                        System::String::Format("[MmDeviceApiWrapper] GetID or GetState failed: {0:X}", hr), "Error");
                                    pDevice->Release();
                                }
                            }
                        }
                        pDevices->Release();
                    }
                    pEnum->Release();
                }
            }
            System::Diagnostics::Trace::WriteLineIf(!(SUCCEEDED(hr) && SUCCEEDED(hr2)),
                System::String::Format("[MmDeviceApiWrapper] Error: GetPlaybackDeviceList failed: {0:X}, {1:X}", hr, hr2), "Error");

            //CoUninitialize();
            return playbackDevices;
        }

		// Enumerates playback device list and marks the default device by the appropriate flag
		static System::Collections::Generic::List<AudioDevice^>^ GetCaptureDeviceList()
		{
			System::Collections::Generic::List<AudioDevice^>^ captureDevices =
				gcnew System::Collections::Generic::List<AudioDevice^>();

			HRESULT hr = S_OK;//CoInitialize(NULL);
			HRESULT hr2 = S_OK;
			if (SUCCEEDED(hr))
			{
				IMMDeviceEnumerator *pEnum = NULL;
				// Create a multimedia device enumerator.
				hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL,
					CLSCTX_ALL, __uuidof(IMMDeviceEnumerator), (void**)&pEnum);
				if (SUCCEEDED(hr))
				{
					IMMDevice *pDevice;
					IMMDeviceCollection *pDevices;
					LPWSTR wstrDefaultID = L"";
					// Enumerate the output devices.
					hr = pEnum->EnumAudioEndpoints(eCapture,
						DEVICE_STATE_ACTIVE | DEVICE_STATE_UNPLUGGED | DEVICE_STATE_DISABLED, &pDevices);
					if (SUCCEEDED(hr))
					{
						HRESULT hrDef = pEnum->GetDefaultAudioEndpoint(eCapture, eConsole, &pDevice);
						if (SUCCEEDED(hrDef))
						{
							hrDef = pDevice->GetId(&wstrDefaultID);
						}
						System::Diagnostics::Trace::WriteLineIf(!SUCCEEDED(hrDef),
							System::String::Format("[MmDeviceApiWrapper] GetDefaultAudioEndPoint failed: {0:X}", hr), "Error");
					}

					if (SUCCEEDED(hr))
					{
						UINT count;
						pDevices->GetCount(&count);
						if (SUCCEEDED(hr))
						{
							for (unsigned int i = 0; i < count; i++)
							{
								hr = pDevices->Item(i, &pDevice);
								if (SUCCEEDED(hr))
								{
									LPWSTR wstrID = NULL;
									DWORD dwState = 0;
									hr = pDevice->GetId(&wstrID);
									hr2 = pDevice->GetState(&dwState);
									if (SUCCEEDED(hr) && SUCCEEDED(hr2))
									{
										IPropertyStore *pStore;
										hr = pDevice->OpenPropertyStore(STGM_READ, &pStore);
										if (SUCCEEDED(hr))
										{
											PROPVARIANT friendlyName;
											PropVariantInit(&friendlyName);
											hr = pStore->GetValue(PKEY_Device_FriendlyName, &friendlyName);
											if (SUCCEEDED(hr))
											{
												System::String^ name = gcnew System::String(friendlyName.pwszVal);
												captureDevices->Add(gcnew AudioDevice(gcnew System::String(wstrID),
													name, (AudioDeviceStateType)dwState, 0 == wcscmp(wstrID, wstrDefaultID)));
												PropVariantClear(&friendlyName);
											}
											pStore->Release();
										}
									}
									System::Diagnostics::Trace::WriteLineIf(!(SUCCEEDED(hr) && SUCCEEDED(hr2)),
										System::String::Format("[MmDeviceApiWrapper] GetID or GetState failed: {0:X}", hr), "Error");
									pDevice->Release();
								}
							}
						}
						pDevices->Release();
					}
					pEnum->Release();
				}
			}
			System::Diagnostics::Trace::WriteLineIf(!(SUCCEEDED(hr) && SUCCEEDED(hr2)),
				System::String::Format("[MmDeviceApiWrapper] Error: GetPlaybackDeviceList failed: {0:X}, {1:X}", hr, hr2), "Error");

			//CoUninitialize();
			return captureDevices;
		}

        // Get default playback device on the system 
        static AudioDevice^ GetDefaultPlaybackDevice()
        {
            AudioDevice^ defaultPlaybackDevice = nullptr;
            HRESULT hr = S_OK;//CoInitialize(NULL);
            //HRESULT hr = CoInitializeEx(NULL, COINIT_MULTITHREADED);
            HRESULT hr2 = S_OK;
            if (SUCCEEDED(hr))
            {
                IMMDeviceEnumerator *pEnum = NULL;
                // Create a multimedia device enumerator.
                hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, 
                    CLSCTX_ALL, __uuidof(IMMDeviceEnumerator), (void**)&pEnum);
                if (SUCCEEDED(hr))
                {
                    IMMDevice *pDevice;
                    // Enumerate the output devices.
                    hr = pEnum->GetDefaultAudioEndpoint(eRender, eConsole, &pDevice);
                    LPWSTR wstrID = NULL;
                    hr = pDevice->GetId(&wstrID);
                    DWORD dwState = 0;
                    hr2 = pDevice->GetState(&dwState);
                    if (SUCCEEDED(hr) && SUCCEEDED(hr2))
                    {
                        IPropertyStore *pStore;
                        hr = pDevice->OpenPropertyStore(STGM_READ, &pStore);
                        if (SUCCEEDED(hr))
                        {
                            PROPVARIANT friendlyName;
                            PropVariantInit(&friendlyName);
                            hr = pStore->GetValue(PKEY_Device_FriendlyName, &friendlyName);
                            if (SUCCEEDED(hr))
                            {
                                defaultPlaybackDevice = gcnew AudioDevice(
                                gcnew System::String(friendlyName.pwszVal), gcnew System::String(wstrID), 
                                (AudioDeviceStateType)dwState, true);
                            }
                            PropVariantClear(&friendlyName);
                        }
                        pStore->Release();
                    }
                    pDevice->Release();
                }
            }
            System::Diagnostics::Trace::WriteLineIf(!(SUCCEEDED(hr) && SUCCEEDED(hr2)),
                System::String::Format("[MmDeviceApiWrapper] Error: GetDefaultPlaybackDevice failed: {0:X}, {1:X}", hr, hr2), "Error");

            //CoUninitialize();
            return defaultPlaybackDevice;
        }

        // Set default playback device on the system
        // returns true if succeeded.
        static bool SetDefaultPlaybackDevice(LPCWSTR devIDString)
        {	
            IPolicyConfigVista *pPolicyConfig;
            ERole reserved = eConsole;

            HRESULT hr = CoCreateInstance(__uuidof(CPolicyConfigVistaClient), 
                NULL, CLSCTX_ALL, __uuidof(IPolicyConfigVista), (LPVOID *)&pPolicyConfig);

            System::Diagnostics::Trace::WriteLineIf(!SUCCEEDED(hr),
                System::String::Format("[MmDeviceApiWrapper] SetDefaultPlaybackDevice CoCreate failed: {0:X}", hr), "Error");

            if (SUCCEEDED(hr))
            {
                System::Diagnostics::Trace::WriteLine(
                    System::String::Format("[MmDeviceApiWrapper] SetDefaultPlaybackDevice to devId {0}", gcnew System::String(devIDString)), "Information");

                hr = pPolicyConfig->SetDefaultEndpoint(devIDString, reserved);

                System::Diagnostics::Trace::WriteLineIf(SUCCEEDED(hr),
                    System::String::Format("[MmDeviceApiWrapper] SetDefaultPlaybackDevice SetDefEndPoint succeeded."), "Information");
                System::Diagnostics::Trace::WriteLineIf(!SUCCEEDED(hr),
                    System::String::Format("[MmDeviceApiWrapper] SetDefaultPlaybackDevice SetDefEndPoint failed: {0:X}", hr), "Error");

                pPolicyConfig->Release();
            }
            return SUCCEEDED(hr);
        }
        
        static bool RegisterForNotification()
        {
            if(!notificationRegistered)
            {
                pNotifyClient = new CMMNotificationClient(audioDeviceNotificationHelper);
                notificationRegistered = RegisterForNotification(true);
            }
            return notificationRegistered;
        }

        static bool UnRegisterForNotification()
        {
            if(notificationRegistered && pNotifyClient)
            {
                notificationRegistered = !RegisterForNotification(false);
                SAFE_DELETE(pNotifyClient);
                return notificationRegistered;
            }
            else
            {
                return false;
            }
        }

    private:

        static AudioDeviceNotificationHelper^ audioDeviceNotificationHelper;
        static bool notificationRegistered;
        static CMMNotificationClient* pNotifyClient;

        // Registered or unregister for notification.
        // The clients can use the event of the A 
        // returns true if succeeded.
        static bool RegisterForNotification(bool registerForNotification)
        {
            HRESULT hr = S_OK;//CoInitialize(NULL);
            if (SUCCEEDED(hr))
            {                                                                
                IMMDeviceEnumerator *pEnum = NULL;
                // Create a multimedia device enumerator.
                hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL,
                    CLSCTX_ALL, __uuidof(IMMDeviceEnumerator), (void**)&pEnum);
                if (SUCCEEDED(hr))
                {
                    IMMNotificationClient* pNotify = (IMMNotificationClient*)(pNotifyClient);
                    if(!registerForNotification)
                    {
                        hr = pEnum->UnregisterEndpointNotificationCallback(pNotify);
                    }
                    else
                    {                                                         
                        hr = pEnum->RegisterEndpointNotificationCallback(pNotify);
                    } 
                    System::Diagnostics::Trace::WriteLineIf(SUCCEEDED(hr),
                        System::String::Format("[MmDeviceApiWrapper] {0}Register for notification succeeded.", 
                        registerForNotification ? "" : "Un" ), "Information");
                    System::Diagnostics::Trace::WriteLineIf(!SUCCEEDED(hr),
                        System::String::Format("[MmDeviceApiWrapper] Error: {0}Register for notification not succeded! Code: {1}", 
                        registerForNotification ? "" : "Un" ,
                        (hr == E_POINTER ? "E_POINTER" : 
                        (hr == E_OUTOFMEMORY ? "E_OUTOFMEMORY" : 
                        (hr == E_NOTFOUND ? "E_NOTFOUND" : "NOT_DEFINED")))), "Error");
                }
                pEnum->Release();
            }
            //CoUninitialize();
            return SUCCEEDED(hr);
        }

    };
}


