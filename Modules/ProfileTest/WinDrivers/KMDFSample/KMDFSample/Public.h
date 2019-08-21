/*++

Module Name:

    public.h

Abstract:

    This module contains the common declarations shared by driver
    and user applications.

Environment:

    user and kernel

--*/

//
// Define an Interface Guid so that apps can find the device and talk to it.
//

DEFINE_GUID (GUID_DEVINTERFACE_KMDFSample,
    0x920f9bbd,0xe581,0x4410,0xa5,0x52,0x77,0x05,0x62,0x38,0x6d,0x75);
// {920f9bbd-e581-4410-a552-770562386d75}
