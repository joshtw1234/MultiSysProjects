#pragma once
#include <strsafe.h>
#include <wbemcli.h>
#include <wbemProv.h>
# pragma comment(lib, "wbemuuid.lib")

/*
Structure for store get WMI Class info
*/
struct WMIDATA
{
	/* [in] WMI root path */
	BSTR rootPath;
	/* [in] WMI WQL path for query WMI Class object */
	BSTR wqlPath;
	/* [out] WMI full path */
	BSTR fullPath;
	/* [in] WMI Name property */
	BSTR pName;
	/* [out] WMI Value property */
	VARIANT pValue;
};


class PublicWMICls
{
public:
	PublicWMICls();
	~PublicWMICls();
	/* This is in test stage not complete yet. */
	bool SetBIOSValue(BSTR * bName, BSTR * bValue, BSTR * bPassd);

	/* Get BIOS value. Before call this function should call GetBIOSClass first. */
	bool GetBIOSValue(WMIDATA & pWMI, IWbemClassObject *pInClsObj);
	/* Get BIOS WMI Class*/
	bool GetBIOSClass(WMIDATA & pWMIData, IWbemClassObject ** pOutClsObj);
private:
	/*
	Get WMI Service Obj. If failed must call CoUninitialize for release COM memory.
	*/
	bool GetWMIServiceObj(IWbemServices** wmiSvc, HRESULT & errCd, BSTR * rootPath);
	/* This is in test stage not complete yet. */
	bool GetWMIObjFullPath(HRESULT & errCode, BSTR * rootPath, BSTR * objPath, VARIANT * vPath);
	/* This is in test stage not complete yet. */
	bool ExecWMIMethod(HRESULT & errCd, BSTR * rootPath, BSTR * clsName, BSTR * fullPath, BSTR * methodName, VARIANT vtInP1, VARIANT vtInP2, VARIANT vtInP3);
	/*
	Get IWbemClassObject Property Data. This is for debug purpose to print out property value in IWbemClassObject.
	*/
	void PrintWbemClassProperty(IWbemClassObject *pClsObj);
	void GetSafeArryData(IWbemClassObject *pclsObj, SAFEARRAY* nameAry);
	void PrintVariantData(VARIANT vValue);
	/*
	Release IUnknown Object.
	*/
	void ReleaseWMIObj(IUnknown *wmiObj);
};

