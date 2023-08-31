// ConsoleWMITester.cpp : This file contains the 'main' function. Program execution begins and ends there.
//
#include <iostream>
#include "WMIHelper.h"

int main()
{
    BSTR rootStr = SysAllocString(L"ROOT\\CIMV2");
    rootStr = SysAllocString(L"ROOT\\HP\\InstrumentedBIOS");
	BSTR qryStr = SysAllocString(L"SELECT * FROM Win32_OperatingSystem");
	/*qryStr = L"SELECT * FROM Win32_PhysicalMemory";
	qryStr = L"SELECT * FROM HP_BIOSSettingInterface";
	qryStr = L"SELECT * FROM HP_BIOSEnumeration";
	qryStr = L"Win32_Process";*/
	qryStr = SysAllocString(L"SELECT * FROM HP_BIOSSetting");
	//qryStr = SysAllocString(L"SELECT * FROM HP_BIOSSettingInterface");
	/*BSTR meshodStr = L"Create";
	meshodStr = L"SetBIOSSetting";*/
	BSTR proName = SysAllocString(L"Network Boot");
	BSTR proNameVal = SysAllocString(L"Enable");
	HRESULT hresult;

    WMIHelper _wmiHelper;
    _wmiHelper.MSDNDemoWMI();
	_wmiHelper.GetWMIPropertyByName(hresult, &rootStr, &qryStr, &proName);
    std::cout << "Hello World!\n";

	//_wmiHelper.SetWMIPropertyByName(hresult, &rootStr, &proName, &proNameVal);

	std::cout << "Josh Set Done!\n" << std::hex << hresult << "\n";

    SysFreeString(rootStr);
	SysFreeString(qryStr);
	SysFreeString(proName);
	SysFreeString(proNameVal);
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
