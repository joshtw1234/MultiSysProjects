#include "pch.h"
#include "WMILoaderExport.h"
#include "WMIWorker.h"

HPWMILOADER_API int WMILoaderExport()
{
    return 0;
}

HPWMILOADER_API int WMIDemoFunc()
{
    WMIWorker hpwmi;
    bool isgood = hpwmi.MSDNDemoWMI();
    return 1;
}
