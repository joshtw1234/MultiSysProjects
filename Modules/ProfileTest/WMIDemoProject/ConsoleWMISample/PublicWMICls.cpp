#include "stdafx.h"
#include "PublicWMICls.h"

PublicWMICls::PublicWMICls()
{
}


PublicWMICls::~PublicWMICls()
{
}

bool PublicWMICls::SetBIOSValue(BSTR * bName, BSTR * bValue, BSTR * bPassd)
{
	bool rev = false;
	HRESULT errCode;
	BSTR rootPath = SysAllocString(L"ROOT\\HP\\InstrumentedBIOS");
	BSTR clsName = SysAllocString(L"HPBIOS_BIOSSettingInterface");
	BSTR insPath = SysAllocString(L"HPBIOS_BIOSSettingInterface.InstanceName=\"ACPI\\\\PNP0C14\\\\0_0\"");
	BSTR methodName = SysAllocString(L"SetBIOSSetting");
	VARIANT vPath, vtInP1, vtInP2, vtInP3;
	if (!GetWMIObjFullPath(errCode, &rootPath, &insPath, &vPath))
	{
		goto EXITSetBIOSValue;
	}
	PrintVariantData(vPath);
	vtInP1.vt = VT_BSTR;
	vtInP1.bstrVal = *bName;
	vtInP2.vt = VT_BSTR;
	vtInP2.bstrVal = *bValue;
	vtInP3.vt = VT_BSTR;
	vtInP3.bstrVal = *bPassd;
	if (!ExecWMIMethod(errCode, &rootPath, &clsName, &vPath.bstrVal, &methodName, vtInP1, vtInP2, vtInP3))
	{
		goto EXITSetBIOSValue;
	}
	rev = true;
EXITSetBIOSValue:

	SysFreeString(rootPath);
	SysFreeString(clsName);
	SysFreeString(insPath);
	SysFreeString(methodName);
	VariantClear(&vPath);
	VariantClear(&vtInP1);
	VariantClear(&vtInP2);
	VariantClear(&vtInP3);
	return rev;
}


bool PublicWMICls::GetBIOSValue(WMIDATA & pWMI, IWbemClassObject * pInClsObj)
{
	wchar_t valWs[64] = {};
	bool rev = false;
	HRESULT hres;
	hres = pInClsObj->Get(pWMI.pName, 0, &pWMI.pValue, NULL, NULL);
	if (SUCCEEDED(hres))
	{
		rev = true;
	}
	return rev;
}

bool PublicWMICls::GetBIOSClass(WMIDATA & pWMIData, IWbemClassObject ** pOutClsObj)
{
	bool rev = false;
	wchar_t wwQL[] = L"WQL";
	HRESULT hres;
	IWbemServices *pWMISvc = NULL;
	IEnumWbemClassObject* pEnumerator = NULL;
	ULONG uReturn = 0;
	if (!GetWMIServiceObj(&pWMISvc, hres, &pWMIData.rootPath))
	{
		goto EXITGetBIOSClass;
	}
	hres = pWMISvc->ExecQuery(wwQL, pWMIData.wqlPath, WBEM_FLAG_FORWARD_ONLY | WBEM_FLAG_RETURN_IMMEDIATELY, NULL, &pEnumerator);
	if (FAILED(hres))
	{
		goto EXITGetBIOSClass;
	}
	hres = pEnumerator->Next(WBEM_INFINITE, 1, pOutClsObj, &uReturn);
	if (0 == uReturn)
	{
		goto EXITGetBIOSClass;
	}
	if (FAILED(hres))
	{
		goto EXITGetBIOSClass;
	}

	//ansTxt = L"True";
	rev = true;
	PrintWbemClassProperty(*pOutClsObj);
EXITGetBIOSClass:
	ReleaseWMIObj(pWMISvc);
	ReleaseWMIObj(pEnumerator);
	return rev;
}

bool PublicWMICls::GetWMIServiceObj(IWbemServices ** wmiSvc, HRESULT & errCd, BSTR * rootPath)
{
	bool rev = false;
	HRESULT hres;
	IWbemLocator *pLoc = NULL;
	// Step 1: --------------------------------------------------
	// Initialize COM. ------------------------------------------
	//hres = CoInitializeEx(0, COINIT_MULTITHREADED);
	hres = CoInitialize(NULL);
	if (FAILED(hres))
	{
		goto GetWMIServiceObj;
	}
	// Step 2: -----------------------------------------------------
	hres = CoInitializeSecurity(
		NULL,
		-1,                          // COM authentication
		NULL,                        // Authentication services
		NULL,                        // Reserved
		RPC_C_AUTHN_LEVEL_DEFAULT,   // Default authentication 
		RPC_C_IMP_LEVEL_IMPERSONATE, // Default Impersonation  
		NULL,                        // Authentication info
		EOAC_NONE,                   // Additional capabilities 
		NULL                         // Reserved
	);

	if (FAILED(hres) && hres != RPC_E_TOO_LATE)
	{
		goto GetWMIServiceObj;
	}
	// Step 3: -----------------------------------------------------
	hres = CoCreateInstance(
		CLSID_WbemLocator,
		0,
		CLSCTX_INPROC_SERVER,
		IID_IWbemLocator, (LPVOID *)&pLoc);

	if (FAILED(hres))
	{
		goto GetWMIServiceObj;
	}

	// Step 4: -----------------------------------------------------
	// Connect to WMI through the IWbemLocator::ConnectServer method
	// Connect to the root\WMI namespace with
	// the current user and obtain pointer pSvc
	// to make IWbemServices calls.
	hres = pLoc->ConnectServer(
		*rootPath,						 // Object path of WMI namespace
		NULL,                    // User name. NULL = current user
		NULL,                    // User password. NULL = current
		0,                       // Locale. NULL indicates current
		NULL,                    // Security flags.
		0,                       // Authority (for example, Kerberos)
		0,                       // Context object 
		wmiSvc                    // pointer to IWbemServices proxy
	);

	if (FAILED(hres))
	{
		goto GetWMIServiceObj;
	}
	// Step 5: -----------------------------------------------------
	hres = CoSetProxyBlanket(
		*wmiSvc,                        // Indicates the proxy to set
		RPC_C_AUTHN_DEFAULT,
		//RPC_C_AUTHN_WINNT,           // RPC_C_AUTHN_xxx
		RPC_C_AUTHZ_DEFAULT,
		//RPC_C_AUTHZ_NONE,            // RPC_C_AUTHZ_xxx
		NULL,                        // Server principal name 
		RPC_C_AUTHN_LEVEL_CALL,      // RPC_C_AUTHN_LEVEL_xxx 
		RPC_C_IMP_LEVEL_IMPERSONATE, // RPC_C_IMP_LEVEL_xxx
		NULL,                        // client identity
		EOAC_NONE                    // proxy capabilities 
	);

	if (FAILED(hres))
	{
		goto GetWMIServiceObj;
	}
	rev = true;
GetWMIServiceObj:
	errCd = hres;
	pLoc->Release();
	return rev;
}

bool PublicWMICls::GetWMIObjFullPath(HRESULT & errCode, BSTR * rootPath, BSTR * objPath, VARIANT * vPath)
{
	bool rev = false;
	HRESULT hres;
	IWbemServices *pWMISvc = NULL;
	IWbemClassObject * pclsInsObj = NULL;
	BSTR strClassProp = SysAllocString(L"__PATH");
	if (!GetWMIServiceObj(&pWMISvc, hres, rootPath))
	{
		goto EXITGetWMIObjFullPath;
	}
	hres = pWMISvc->GetObject(*objPath, 0, NULL, &pclsInsObj, NULL);
	if (FAILED(hres))
	{
		goto EXITGetWMIObjFullPath;
	}
	PrintWbemClassProperty(pclsInsObj);
	//Most impotent Step.
	//get Instance Path property
	hres = pclsInsObj->Get(strClassProp, 0, vPath, 0, 0);
	if (FAILED(hres))
	{
		goto EXITGetWMIObjFullPath;
	}
	rev = true;
EXITGetWMIObjFullPath:
	errCode = hres;
	ReleaseWMIObj(pWMISvc);
	CoUninitialize();

	return rev;
}

bool PublicWMICls::ExecWMIMethod(HRESULT & errCd, BSTR * rootPath, BSTR * clsName, BSTR * fullPath, BSTR * methodName, VARIANT vtInP1, VARIANT vtInP2, VARIANT vtInP3)
{
	bool rev = false;
	HRESULT hres;
	IWbemServices *pWMISvc = NULL;
	IWbemClassObject * pclsObj = NULL;
	IWbemClassObject * pInParmObj = NULL;
	IWbemClassObject * pOutParmObj = NULL;
	IWbemClassObject * pInParamInsObj = NULL;
	IWbemClassObject * pOutParmInsObj = NULL;
	wchar_t parName[] = L"Name";
	wchar_t parValue[] = L"Value";
	wchar_t parPass[] = L"Password";
	//Let's GO
	if (!GetWMIServiceObj(&pWMISvc, hres, rootPath))
	{
		goto EXITExecWMIMethod;
	}
	//Start work from here
	//get class name
	hres = pWMISvc->GetObject(*clsName, 0, NULL, &pclsObj, NULL);
	if (FAILED(hres))
	{
		goto EXITExecWMIMethod;
	}
	//get Method.
	hres = pclsObj->GetMethod(*methodName, NULL, &pInParmObj, &pOutParmObj);
	if (FAILED(hres))
	{
		goto EXITExecWMIMethod;
	}
	//setup parameter.
	if (SysStringLen(vtInP3.bstrVal) != 0)
	{
		goto EXITExecWMIMethod;
	}
	//get new Instance then put parameter in.
	hres = pInParmObj->SpawnInstance(0, &pInParamInsObj);
	if (FAILED(hres))
	{
		goto EXITExecWMIMethod;
	}
	/*
	  put new data here.
	*/
	hres = pInParamInsObj->Put(parName, 0, &vtInP1, vtInP1.vt);
	if (FAILED(hres))
	{
		goto EXITExecWMIMethod;
	}
	hres = pInParamInsObj->Put(parValue, 0, &vtInP2, vtInP2.vt);
	if (FAILED(hres))
	{
		goto EXITExecWMIMethod;
	}
	hres = pInParamInsObj->Put(parPass, 0, &vtInP3, vtInP3.vt);
	if (FAILED(hres))
	{
		goto EXITExecWMIMethod;
	}
	PrintWbemClassProperty(pInParamInsObj);
	/* Execute WMI */
	hres = pWMISvc->ExecMethod(*fullPath, *methodName, 0, NULL, pInParamInsObj, &pOutParmInsObj, NULL);
	if (FAILED(hres))
	{
		goto EXITExecWMIMethod;
	}
	rev = true;
EXITExecWMIMethod:
	errCd = hres;
	//
	ReleaseWMIObj(pWMISvc);
	ReleaseWMIObj(pclsObj);
	ReleaseWMIObj(pInParmObj);
	ReleaseWMIObj(pOutParmObj);
	ReleaseWMIObj(pInParamInsObj);
	ReleaseWMIObj(pOutParmInsObj);
	//
	CoUninitialize();
	return rev;
}


void PublicWMICls::PrintWbemClassProperty(IWbemClassObject * pClsObj)
{
	SAFEARRAY* nameAry;
	pClsObj->GetNames(NULL, WBEM_FLAG_ALWAYS, NULL, &nameAry);
	//Get WMI Names
	GetSafeArryData(pClsObj, nameAry);
	//Release Array
	SafeArrayUnaccessData(nameAry);
}

void PublicWMICls::GetSafeArryData(IWbemClassObject * pclsObj, SAFEARRAY * nameAry)
{
	BSTR HUGEP *pBSTR;
	SafeArrayAccessData(nameAry, (void HUGEP* FAR*)&pBSTR);
	HRESULT hr;
	
	for (long i = 0; i < nameAry->rgsabound->cElements; i++)
	{
		if (0 == wcscmp(pBSTR[i], L"__PATH") ||
			0 == wcscmp(pBSTR[i], L"IsReadOnly") ||
			0 == wcscmp(pBSTR[i], L"Name") ||
			0 == wcscmp(pBSTR[i], L"Value"))
		{
			/*::StringCchPrintfW(wmiDbgLogOutput.strDebugLogOutputString, DEBUG_MAX_STRING, L"%s::Properties::%s", __FUNCTIONW__, pBSTR[i]);
			wmiDbgLogOutput.Output();*/
			VARIANT vtProp;
			// Get the value of the Name property
			hr = pclsObj->Get(pBSTR[i], 0, &vtProp, 0, 0);
			if (SUCCEEDED(hr))
			{
				PrintVariantData(vtProp);
				VariantClear(&vtProp);
			}
		}
	}
}

void PublicWMICls::PrintVariantData(VARIANT vValue)
{
	HRESULT hr;
	wchar_t errTxt[128] = { 0 };
	switch (V_VT(&vValue))
	{
	case VT_NULL:
		swprintf_s(errTxt, L"%s", L"Property Value is NULL");
		break;
	case VT_BSTR:
		swprintf_s(errTxt, L"%s", vValue.bstrVal);
		break;
	case VT_I4:
		swprintf_s(errTxt, L"%d", vValue.intVal);
		break;
	case VT_BOOL:
		if (vValue.boolVal == -1)
		{
			swprintf_s(errTxt, L"%s", L"True");
		}
		else
		{
			swprintf_s(errTxt, L"%s", L"False");
		}
		break;
	case 8200:
		BSTR HUGEP *pSubBSTR;
		hr = SafeArrayAccessData(vValue.parray, (void HUGEP* FAR*)&pSubBSTR);
		if (SUCCEEDED(hr))
		{
			for (long i = 0; i < vValue.parray->rgsabound->cElements; i++)
			{
				swprintf_s(errTxt, L"SubAry: %s", pSubBSTR[i]);
			}
		}
		SafeArrayUnaccessData(vValue.parray);
		break;
	default:
		swprintf_s(errTxt, L"Unknow::%d", V_VT(&vValue));
		break;
	}
	printf("%ls\n", errTxt);
}

void PublicWMICls::ReleaseWMIObj(IUnknown * wmiObj)
{
	if (wmiObj)
	{
		wmiObj->Release();
	}
}

