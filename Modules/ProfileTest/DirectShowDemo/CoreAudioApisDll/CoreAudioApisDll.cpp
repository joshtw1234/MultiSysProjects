// CoreAudioApisDll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "CoreAudioApisDll.h"


// This is an example of an exported variable
COREAUDIOAPISDLL_API int nCoreAudioApisDll=0;

// This is an example of an exported function.
COREAUDIOAPISDLL_API int fnCoreAudioApisDll(void)
{
    return 42;
}

// This is the constructor of a class that has been exported.
CCoreAudioApisDll::CCoreAudioApisDll()
{
    return;
}
