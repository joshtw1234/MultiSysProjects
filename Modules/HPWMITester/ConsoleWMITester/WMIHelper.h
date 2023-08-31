#pragma once
//#include <strsafe.h>
//#include <wbemcli.h>
//#include <wbemProv.h>
//#include <iostream>
//# pragma comment(lib, "wbemuuid.lib")

#define _WIN32_DCOM
#include <iostream>
using namespace std;
#include <comdef.h>
#include <Wbemidl.h>

#pragma comment(lib, "wbemuuid.lib")

class WMIHelper
{
public:
	WMIHelper();
	~WMIHelper();
	bool MSDNDemoWMI();
	wchar_t GetWMIPropertyByName(HRESULT& errCd, BSTR* rootPath, BSTR* qryPath, BSTR* proName);
	wchar_t SetWMIPropertyByName(HRESULT& errCd, BSTR* rootPath, BSTR* wmiName, BSTR* wmiValue);
private:
	IWbemServices* ConnectToWmi(wchar_t* wmiNamespace);

};