#pragma once

namespace AudioDeviceUtil {
    public ref class AudioDevice
    {
    public:
        System::String^ DeviceID;
        System::String^ FriendlyName;
        AudioDeviceStateType DeviceState;
        bool IsDefaultDevice;

        AudioDevice()
        {
        }

        AudioDevice(
            System::String^ deviceID,
            System::String^ friendlyName,
            AudioDeviceStateType deviceState,
            bool isDefaultDevice )
        {
            DeviceID = deviceID;
            FriendlyName = friendlyName;
            DeviceState = deviceState;
            IsDefaultDevice = isDefaultDevice;
        }

        AudioDevice(const AudioDevice^ value)
        {
            DeviceID     = value->DeviceID;
            FriendlyName = value->FriendlyName;
            DeviceState  = value->DeviceState;
            IsDefaultDevice = value->IsDefaultDevice;
        }

        virtual ~AudioDevice()
        {
            System::Diagnostics::Trace::TraceInformation(
                System::String::Format("[AudioDevice] destructor call on: \"{0}\"", FriendlyName));
        }

        property System::String^ ToStringFormat
        {
            System::String^ get()
            {
                return gcnew System::String("{2}[{1}] {0}");
            }
        }

        virtual System::String^ ToString() override
        {
            return System::String::Format(ToStringFormat, FriendlyName, DeviceState, IsDefaultDevice ? "*" : "");
        }
    };                              
}