// SoundDeviceUtil.h
#pragma once

#include <vcclr.h>
#include <stdlib.h>
#include <string>
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>
#include "windows.h"
#include "MmDeviceApiWrapper.h"

using namespace System;

namespace AudioDeviceUtil {

    public ref class AudioDeviceManager
    {                                                
                                                                      
    private: 
        MmDeviceApiWrapper^ apiWrapper;
        System::Collections::Generic::List<AudioDevice^>^ playbackDevices;
		System::Collections::Generic::List<AudioDevice^>^ captureDevices;

    public:  
        event System::EventHandler<AudioDeviceNotificationEventArgs^>^ AudioDeviceEvent;

        AudioDeviceManager(bool registerForNotification)
        {
            AudioDeviceManager();
            if(registerForNotification)
                MmDeviceApiWrapper::RegisterForNotification();
        }

        AudioDeviceManager()
        {
            apiWrapper = gcnew MmDeviceApiWrapper();
            MmDeviceApiWrapper::AudioDeviceNotification->AudioDeviceEvent +=  
                gcnew System::EventHandler<AudioDeviceNotificationEventArgs^>(this, &AudioDeviceUtil::AudioDeviceManager::OnAudioDeviceEvent) ;
            playbackDevices = MmDeviceApiWrapper::GetPlaybackDeviceList();
			captureDevices = MmDeviceApiWrapper::GetCaptureDeviceList();
        }

        virtual ~AudioDeviceManager()
        {
        }

        property bool RegisterForNotification
        {
            bool get()
            {
                return MmDeviceApiWrapper::IsRegisteredForNotification;
            }
            void set (bool value)
            {
                if( value )
                {
                    MmDeviceApiWrapper::RegisterForNotification();
                }
                else
                {
                    MmDeviceApiWrapper::UnRegisterForNotification();
                }
            }
        }

        // Getter for actually stored playback device list
        property  System::Collections::Generic::List<AudioDevice^>^ PlaybackDevices
        {
            System::Collections::Generic::List<AudioDevice^>^ get()
            {
                return playbackDevices;
            }
        }

		property  System::Collections::Generic::List<AudioDevice^>^ CaptureDevices
		{
			System::Collections::Generic::List<AudioDevice^>^ get()
			{
				return captureDevices;
			}
		}

        // Get / Set default device on the system and actualises the stored playback device list
        property System::String^ DefaultPlaybackDeviceName
        {
            System::String^ get()
            {
                AudioDevice^ dev = GetDefaultPlaybackDevice();
                return dev != nullptr ? dev->FriendlyName : System::String::Empty;
            }

            void set(System::String^ value)
            {
                SetDefaultPlaybackDeviceByName(value);
            }
        }

        // Actualises and returns the stored playback device list
        System::Collections::Generic::List<AudioDevice^>^ UpdatePlaybackDeviceList() 
        {
            playbackDevices = MmDeviceApiWrapper::GetPlaybackDeviceList();                                                      

            System::Diagnostics::Trace::WriteLineIf(playbackDevices->Count == 0,
                System::String::Format("[AudioDeviceManager] UpdatePlaybackDeviceList no device found."), "Error");

            return playbackDevices;
        }
            
    private: 

        // Get device and actualizes the stored playback device list
        AudioDevice^ GetDeviceByName(System::String^ deviceName)                                                       
        {
            for each (AudioDevice^ dev in UpdatePlaybackDeviceList())
            {
                if(dev->FriendlyName == deviceName) 
                {
                    return dev;
                }
            }

            System::Diagnostics::Trace::TraceError(
                "[AudioDeviceManager] Error: GetDeviceByName: \"" + deviceName + "\" not found!");

            return nullptr;
        }

        // Get default device and actulizes the stored device list 
        AudioDevice^ GetDefaultPlaybackDevice()
        {
            for each (AudioDevice^ dev in UpdatePlaybackDeviceList())
            {
                if(dev->IsDefaultDevice) 
                {
                    return dev;
                }
            }

            System::Diagnostics::Trace::TraceError(
                "[AudioDeviceManager] Error: GetDefaultPlaybackDeviceName not succeeded!");

            return nullptr;
        }

        // Set default playback device on the system and actualizes the stored playback device list
        void SetDefaultPlaybackDeviceByName(System::String^ deviceName)
        {
            AudioDevice^ dev = GetDeviceByName(deviceName); 

            if( dev == nullptr || dev->IsDefaultDevice || dev->DeviceState != AudioDeviceStateType::Active )                  
            {
                System::Diagnostics::Trace::TraceInformation(
                    "[AudioDeviceManager] Can't set " + deviceName + " to default. Not found or already the default or not active." );
                return;
            }

            std::wstring devIDString = ToWString(dev->DeviceID);

            System::Diagnostics::Trace::TraceInformation(
                "[AudioDeviceManager] DefaultPlaybackDevice to set: \"" + 
                deviceName + "\" Id: " + gcnew System::String(devIDString.c_str()));

            if(!devIDString.empty() && MmDeviceApiWrapper::SetDefaultPlaybackDevice(devIDString.c_str()))
            {
                for each (AudioDevice^ dev in playbackDevices)
                {
                    dev->IsDefaultDevice = (dev->FriendlyName == deviceName) ;
                }
            }
            else
            {
                System::Diagnostics::Trace::TraceError(
                    "[AudioDeviceManager] Error: {0} not set!", deviceName);
            }
        }

        // Event handler for AudioDeviceNotifications 
        void OnAudioDeviceEvent(Object^ sender, AudioDeviceNotificationEventArgs^ e)
        {
            switch(e->Reason)
            {
            case AudioDeviceNotificationEventType::DefaultDeviceChanged:
                for each (AudioDevice^ dev in playbackDevices)
                {
                    dev->IsDefaultDevice = (e->DeviceId == dev->DeviceID) ;
                }
                break;
            case AudioDeviceNotificationEventType::DeviceAdded:
            case AudioDeviceNotificationEventType::DeviceRemoved:
            case AudioDeviceNotificationEventType::DeviceStateChanged:
                playbackDevices = MmDeviceApiWrapper::GetPlaybackDeviceList();
                break;
            case AudioDeviceNotificationEventType::PropertyValueChanged:
                // not handled yet
                break;
            }

            System::Diagnostics::Trace::TraceInformation(
                "[AudioDeviceManager] {0} event received for {1}", e->Reason, e->DeviceId);
            // forward the event ...
            AudioDeviceEvent(this, e);
        }

        std::wstring ToWString(System::String^ value)
        {
            return msclr::interop::marshal_as<std::wstring>(value);
        }
    };
}
