#pragma once

#include "Mmdeviceapi.h"
#include "Functiondiscoverykeys_devpkey.h"
#include <msclr\auto_gcroot.h>

//-----------------------------------------------------------
// Example implementation of IMMNotificationClient interface.
// When the status of audio endpoint devices change, the
// MMDevice module calls these methods to notify the client.
//-----------------------------------------------------------

#define SAFE_RELEASE(punk)  \
              if ((punk) != NULL)  \
                { (punk)->Release(); (punk) = NULL; }

#define SAFE_DELETE(punk)  \
              if ((punk) != NULL)  \
                { delete (punk); (punk) = NULL; }

namespace AudioDeviceUtil {

    public enum class AudioDeviceStateType
    {
        Active        = 1,
        Disabled      = 2,
        NotPresent    = 4,
        Unplugged     = 8,
        StateMaskAll = 15,
    };

    public enum class AudioDeviceNotificationEventType 
    { 
        DefaultDeviceChanged,
        DeviceAdded,
        DeviceRemoved,
        DeviceStateChanged,
        PropertyValueChanged
    };

    public ref class AudioDeviceNotificationEventArgs : public System::EventArgs
    {
    public: 
        property System::String^ DeviceId;
        property AudioDeviceNotificationEventType Reason;
        property AudioDeviceStateType State;
        AudioDeviceNotificationEventArgs()
        {
            State = AudioDeviceStateType::StateMaskAll;
        }
        AudioDeviceNotificationEventArgs( AudioDeviceNotificationEventType type, System::String^ devId)
        {
            AudioDeviceNotificationEventArgs();
            Reason = type;
            DeviceId = devId; 
        }
        ~AudioDeviceNotificationEventArgs() {}
    };

    // http://www.codeproject.com/Articles/19354/Quick-C-CLI-Learn-C-CLI-in-less-than-minutes
    public ref class AudioDeviceNotification 
    {
    public: 
        delegate void NotifyDelegate(AudioDeviceNotificationEventArgs^);
        static event NotifyDelegate ^NotifyEvent;

        event System::EventHandler<AudioDeviceNotificationEventArgs^>^ AudioDeviceEvent;

        AudioDeviceNotification()
        {
            NotifyEvent += gcnew NotifyDelegate(this, &AudioDeviceNotification::ManagedNotification); 
        }
        ~AudioDeviceNotification()
        {
        }
        void ManagedNotification(AudioDeviceNotificationEventArgs^ e)
        {
            AudioDeviceEvent(this, e);
        }
    };

    private ref class AudioDeviceNotificationHelper : public AudioDeviceNotification 
    {
    private:
        AudioDeviceNotification ^ patient;
    public:
        AudioDeviceNotificationHelper()
        {
            patient = gcnew AudioDeviceNotification();
        }
        void ForwardNotification(AudioDeviceNotificationEventArgs^ e) {
            patient->NotifyEvent(e);
        }
    };

    public class CMMNotificationClient : IMMNotificationClient
    {
        LONG _cRef;
        IMMDeviceEnumerator *_pEnumerator;

        msclr::auto_gcroot<AudioDeviceNotificationHelper^> _notificationForwarder;
        msclr::auto_gcroot<System::String^> _lastDeviceId; 

     public:
        CMMNotificationClient(AudioDeviceNotificationHelper^ adn) : _cRef(1), _pEnumerator(NULL)
        {
            _notificationForwarder = adn;
        }

        ~CMMNotificationClient()
        {
            SAFE_RELEASE(_pEnumerator)
        }

        // IUnknown methods -- AddRef, Release, and QueryInterface
        ULONG STDMETHODCALLTYPE AddRef()
        {
            return InterlockedIncrement(&_cRef);
        }

        ULONG STDMETHODCALLTYPE Release()
        {
            ULONG ulRef = InterlockedDecrement(&_cRef);
            if (0 == ulRef)
            {
                delete this;
            }
            return ulRef;
        }

        HRESULT STDMETHODCALLTYPE QueryInterface( REFIID riid, VOID **ppvInterface)
        {
            if (IID_IUnknown == riid)
            {
                AddRef();
                *ppvInterface = (IUnknown*)this;
            }
            else if (__uuidof(IMMNotificationClient) == riid)
            {
                AddRef();
                *ppvInterface = (IMMNotificationClient*)this;
            }
            else
            {
                *ppvInterface = NULL;
                return E_NOINTERFACE;
            }
            return S_OK;
        }

        // Callback methods for device-event notifications.

        HRESULT STDMETHODCALLTYPE OnDefaultDeviceChanged( EDataFlow flow, ERole role, LPCWSTR pwstrDeviceId)
        {
            /* example code
            char  *pszFlow = "?????";
            char  *pszRole = "?????";

            _PrintDeviceName(pwstrDeviceId);

            switch (flow)
            {
            case eRender:
            pszFlow = "eRender";
            break;
            case eCapture:
            pszFlow = "eCapture";
            break;
            }

            switch (role)
            {
            case eConsole:
            pszRole = "eConsole";
            break;
            case eMultimedia:
            pszRole = "eMultimedia";
            break;
            case eCommunications:
            pszRole = "eCommunications";
            break;
            }
            */
            System::String^ currentDeviceId = gcnew System::String(pwstrDeviceId);
            System::Diagnostics::Trace::TraceInformation(
                "[AudiDeviceUtil] OnDefaultDeviceChanged {2} flow:{0} role: {1}", (int)flow, (int)role, currentDeviceId);
            if(_lastDeviceId || 0 != System::String::Compare(_lastDeviceId, currentDeviceId)) 
            {
                _notificationForwarder->ForwardNotification(
                    gcnew AudioDeviceNotificationEventArgs(
                    AudioDeviceNotificationEventType::DefaultDeviceChanged,
                    currentDeviceId));
            }

            _lastDeviceId = currentDeviceId;

            return S_OK;
        }

        HRESULT STDMETHODCALLTYPE OnDeviceAdded(LPCWSTR pwstrDeviceId)
        {
            _notificationForwarder->ForwardNotification(
                gcnew AudioDeviceNotificationEventArgs(
                AudioDeviceNotificationEventType::DeviceAdded,
                gcnew System::String(pwstrDeviceId)));

            return S_OK;
        };

        HRESULT STDMETHODCALLTYPE OnDeviceRemoved(LPCWSTR pwstrDeviceId)
        {
            _notificationForwarder->ForwardNotification(
                gcnew AudioDeviceNotificationEventArgs(
                AudioDeviceNotificationEventType::DeviceRemoved,
                gcnew System::String(pwstrDeviceId)) );

            return S_OK;
        }

        HRESULT STDMETHODCALLTYPE OnDeviceStateChanged( LPCWSTR pwstrDeviceId, DWORD dwNewState)
        {
            auto changeEvent = gcnew AudioDeviceNotificationEventArgs(
                AudioDeviceNotificationEventType::DeviceStateChanged,
                gcnew System::String(pwstrDeviceId));

            switch (dwNewState)
            {
            case DEVICE_STATE_ACTIVE:
                changeEvent->State = AudioDeviceStateType::Active;
                break;
            case DEVICE_STATE_DISABLED:
                changeEvent->State = AudioDeviceStateType::Disabled;
                break;
            case DEVICE_STATE_NOTPRESENT:
                changeEvent->State = AudioDeviceStateType::NotPresent;
                break;
            case DEVICE_STATE_UNPLUGGED:
                changeEvent->State = AudioDeviceStateType::Unplugged;
                break;
            }

            _notificationForwarder->ForwardNotification(changeEvent);

            return S_OK;
        }

        HRESULT STDMETHODCALLTYPE OnPropertyValueChanged( LPCWSTR pwstrDeviceId, const PROPERTYKEY key)
        {
            // Not Supported...

            //printf("  -->Changed device property "
            //       "{%8.8x-%4.4x-%4.4x-%2.2x%2.2x-%2.2x%2.2x%2.2x%2.2x%2.2x%2.2x}#%d\n",
            //       key.fmtid.Data1, key.fmtid.Data2, key.fmtid.Data3,
            //       key.fmtid.Data4[0], key.fmtid.Data4[1],
            //       key.fmtid.Data4[2], key.fmtid.Data4[3],
            //       key.fmtid.Data4[4], key.fmtid.Data4[5],
            //       key.fmtid.Data4[6], key.fmtid.Data4[7],
            //       key.pid);

            return S_OK;
        }
    };
} // namespace AudioDeviceUtil