// ConsoleWMISample.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "PublicWMICls.h"

enum WMITYPE
{
	WMI_BoardID,
	WMI_ProductName,
	WMI_MAX_Multiplier,
	WMI_MAX_Voltage,
	WMI_Network
};

int main()
{

	IWbemClassObject * wmiClsObj = NULL;
	PublicWMICls hpWMI;
	WMIDATA wData;
	WMITYPE temp = WMI_Network;
	//
	//swprintf(revWCh, L"%d|", temp);
	//revStr = BSTRtoSTDWstring(revWCh);
	//
	BSTR bName = SysAllocString(L"Network Boot");
	BSTR bValue = SysAllocString(L"Disable");//Switch this to "Enable" to set Network boot value for demo
	BSTR bPassd = SysAllocString(L"");
	wData.rootPath = SysAllocString(L"ROOT\\HP\\InstrumentedBIOS");
	wData.pName = SysAllocString(L"Value");
	switch (temp)
	{
	case WMI_BoardID:
		wData.wqlPath = SysAllocString(L"SELECT * FROM HPBIOS_BIOSString WHERE Name=\"System Board ID (ReadOnly)\"");
		break;
	case WMI_ProductName:
		wData.wqlPath = SysAllocString(L"SELECT * FROM HPBIOS_BIOSString WHERE Name=\"Product Name\"");
		break;
	case WMI_MAX_Multiplier:
		wData.wqlPath = SysAllocString(L"SELECT * FROM HPBIOS_BIOSString WHERE Name=\"OC Frequency Max Limit\"");
		break;
	case WMI_MAX_Voltage:
		wData.wqlPath = SysAllocString(L"SELECT * FROM HPBIOS_BIOSString WHERE Name=\"OC Voltage Max Limit\"");
		break;
	default:
		wData.wqlPath = SysAllocString(L"SELECT * FROM HPBIOS_BIOSEnumeration WHERE Name=\"Network Boot\"");
		break;
	}
	
	if (!hpWMI.GetBIOSClass(wData, &wmiClsObj))
	{
		goto EXITGETWMICLASS;
	}
	/*if (!hpWMI.GetBIOSValue(wData, wmiClsObj))
	{
		goto EXITGETWMICLASS;
	}*/

	/*if (!hpWMI.SetBIOSValue(&bName, &bValue, &bPassd))
	{
		goto EXITGETWMICLASS;
	}*/

EXITGETWMICLASS:
	if (wmiClsObj)
	{
		wmiClsObj->Release();
	}
	getchar();
    return 0;
}

