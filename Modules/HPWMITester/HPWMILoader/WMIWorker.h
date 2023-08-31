#pragma once
#include <strsafe.h>
#include <wbemcli.h>
#include <wbemProv.h>
# pragma comment(lib, "wbemuuid.lib")

class WMIWorker
{
public:
	WMIWorker();
	~WMIWorker();
	bool MSDNDemoWMI();

};
