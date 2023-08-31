#include "WMIHelper.h"


void GetSafeArry(IWbemClassObject* pclsObj, SAFEARRAY* ssAry, BSTR HUGEP* pBSTR)
{
    HRESULT hr;
    for (long i = 0; i < ssAry->rgsabound->cElements; i++)
    {
        wprintf(L"Properties: %s\n", pBSTR[i]);
        VARIANT vtProp;
        int ee = 0;
        //VariantInit(&vtProp);
        // Get the value of the Name property
        hr = pclsObj->Get(pBSTR[i], 0, &vtProp, 0, 0);
        if (SUCCEEDED(hr))
        {
            switch (V_VT(&vtProp))
            {
            case VT_NULL:
                wprintf(L"Property Value is NULL\n");
                break;
            case VT_BSTR:
                wprintf(L"%s\n", vtProp.bstrVal);

                break;
            case VT_I4:
                wprintf(L"%d\n", vtProp.intVal);
                break;
            case VT_BOOL:
                if (vtProp.boolVal == -1)
                {
                    wprintf(L"True\n", vtProp.intVal);
                }
                else
                {
                    wprintf(L"False\n", vtProp.intVal);
                }
                //ee = 123;
                break;
            case 8200:
                BSTR HUGEP * pSubBSTR;
                hr = SafeArrayAccessData(vtProp.parray, (void HUGEP * FAR*) & pSubBSTR);
                if (SUCCEEDED(hr))
                {
                    for (long i = 0; i < vtProp.parray->rgsabound->cElements; i++)
                    {
                        wprintf(L"SubAry: %s\n", pSubBSTR[i]);
                    }
                }
                SafeArrayUnaccessData(vtProp.parray);
                break;
            default:
                wprintf(L"Unknow::%d\n", V_VT(&vtProp));
                break;
            }
            VariantClear(&vtProp);

            if (0 == wcscmp(pBSTR[i], L"PossibleValues"))
            {
                int cc = 0;
            }
        }
    }
}


void GetWBMCLSProperty(IWbemClassObject* pClsObj)
{
    SAFEARRAY* nameAry;
    HRESULT hres = pClsObj->GetNames(NULL, WBEM_FLAG_ALWAYS, NULL, &nameAry);
    /*if (FAILED(hres))
    {
        errCd = hres;
        CoUninitialize();
        return rev;
    }*/
    wprintf(L"=========================================\n");

    //Get WMI Names
    BSTR HUGEP* pBSTR;
    hres = SafeArrayAccessData(nameAry, (void HUGEP * FAR*) & pBSTR);
    /*if (FAILED(hres))
    {
        errCd = hres;
        pclsObj->Release();
        CoUninitialize();
        return rev;
    }*/
    GetSafeArry(pClsObj, nameAry, pBSTR);
    SafeArrayUnaccessData(nameAry);
}


WMIHelper::WMIHelper()
{
}

WMIHelper::~WMIHelper()
{
}

bool WMIHelper::MSDNDemoWMI()
{
    return false;
}

wchar_t WMIHelper::GetWMIPropertyByName(HRESULT& errCd, BSTR* rootPath, BSTR* qryPath, BSTR* proName)
{
    //HRESULT hres;

    // Step 1: --------------------------------------------------
    // Initialize COM. ------------------------------------------

    errCd = CoInitializeEx(0, COINIT_MULTITHREADED);
    if (FAILED(errCd))
    {
        cout << "Failed to initialize COM library. Error code = 0x"
            << hex << errCd << endl;
        return 1;                  // Program has failed.
    }

    // Step 2: --------------------------------------------------
    // Set general COM security levels --------------------------

    errCd = CoInitializeSecurity(
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


    if (FAILED(errCd))
    {
        cout << "Failed to initialize security. Error code = 0x"
            << hex << errCd << endl;
        CoUninitialize();
        return 1;                    // Program has failed.
    }

    // Step 3: ---------------------------------------------------
    // Obtain the initial locator to WMI -------------------------

    IWbemLocator* pLoc = NULL;

    errCd = CoCreateInstance(
        CLSID_WbemLocator,
        0,
        CLSCTX_INPROC_SERVER,
        IID_IWbemLocator, (LPVOID*)&pLoc);

    if (FAILED(errCd))
    {
        cout << "Failed to create IWbemLocator object."
            << " Err code = 0x"
            << hex << errCd << endl;
        CoUninitialize();
        return 1;                 // Program has failed.
    }

    // Step 4: -----------------------------------------------------
    // Connect to WMI through the IWbemLocator::ConnectServer method

    IWbemServices* pSvc = NULL;

    // Connect to the root\cimv2 namespace with
    // the current user and obtain pointer pSvc
    // to make IWbemServices calls.
    errCd = pLoc->ConnectServer(
        *rootPath, // Object path of WMI namespace
        NULL,                    // User name. NULL = current user
        NULL,                    // User password. NULL = current
        0,                       // Locale. NULL indicates current
        NULL,                    // Security flags.
        0,                       // Authority (for example, Kerberos)
        0,                       // Context object 
        &pSvc                    // pointer to IWbemServices proxy
    );

    if (FAILED(errCd))
    {
        cout << "Could not connect. Error code = 0x"
            << hex << errCd << endl;
        pLoc->Release();
        CoUninitialize();
        return 1;                // Program has failed.
    }
    const std::string stdstr(_bstr_t(*rootPath, true));
    cout << "Connected to ROOT\\" << stdstr << endl;


    // Step 5: --------------------------------------------------
    // Set security levels on the proxy -------------------------

    errCd = CoSetProxyBlanket(
        pSvc,                        // Indicates the proxy to set
        RPC_C_AUTHN_WINNT,           // RPC_C_AUTHN_xxx
        RPC_C_AUTHZ_NONE,            // RPC_C_AUTHZ_xxx
        NULL,                        // Server principal name 
        RPC_C_AUTHN_LEVEL_CALL,      // RPC_C_AUTHN_LEVEL_xxx 
        RPC_C_IMP_LEVEL_IMPERSONATE, // RPC_C_IMP_LEVEL_xxx
        NULL,                        // client identity
        EOAC_NONE                    // proxy capabilities 
    );

    if (FAILED(errCd))
    {
        cout << "Could not set proxy blanket. Error code = 0x"
            << hex << errCd << endl;
        pSvc->Release();
        pLoc->Release();
        CoUninitialize();
        return 1;               // Program has failed.
    }

    // Step 6: --------------------------------------------------
    // Use the IWbemServices pointer to make requests of WMI ----

    // For example, get the name of the operating system
    IEnumWbemClassObject* pEnumerator = NULL;
    errCd = pSvc->ExecQuery(
        bstr_t("WQL"),
        *qryPath,
        WBEM_FLAG_FORWARD_ONLY | WBEM_FLAG_RETURN_IMMEDIATELY,
        NULL,
        &pEnumerator);

    if (FAILED(errCd))
    {
        cout << "Query for operating system name failed."
            << " Error code = 0x"
            << hex << errCd << endl;
        pSvc->Release();
        pLoc->Release();
        CoUninitialize();
        return 1;               // Program has failed.
    }

    // Step 7: -------------------------------------------------
    // Get the data from the query in step 6 -------------------

    IWbemClassObject* pclsObj = NULL;
    ULONG uReturn = 0;

    while (pEnumerator)
    {
        HRESULT hr = pEnumerator->Next(WBEM_INFINITE, 1,
            &pclsObj, &uReturn);

        if (0 == uReturn)
        {
            break;
        }

        VARIANT vtProp;
        VARIANT vtPropValue;
        VariantInit(&vtProp);
        VariantInit(&vtPropValue);
        // Get the value of the Name property
        hr = pclsObj->Get(L"Name", 0, &vtProp, 0, 0);
        hr = pclsObj->Get(L"CurrentValue", 0, &vtPropValue, 0, 0);
        BSTR mmm = SysAllocString(L"Empty or INT");
        //Remove empty name field
        int cpV = wcscmp(vtProp.bstrVal, L" ");
        if (0 != cpV)
        {
            

            switch (vtPropValue.vt)
            {
            case VT_BSTR:
                mmm = vtPropValue.bstrVal;
                break;
            case VT_I4:
                break;
            case VT_BOOL:
                break;
            case VT_ARRAY | VT_BSTR:	// string array
            case VT_ARRAY | VT_I4:		// int array
            case VT_ARRAY | VT_BOOL:	// bool array
                break;
            default:
                break;
            }
            wcout << " Josh WMI Name : [" << vtProp.bstrVal << "] Current Value : [" << mmm << "]" << endl;
            cpV = wcscmp(vtProp.bstrVal, *proName);
            if (0 == cpV)
            {
                wcout << " Josh Found : [" << vtProp.bstrVal << "]" << endl;
            }
        }
        VariantClear(&vtProp);
        VariantClear(&vtPropValue);

        SysFreeString(mmm);
        pclsObj->Release();
        
    }

    // Cleanup
    // ========

    pSvc->Release();
    pLoc->Release();
    pEnumerator->Release();
    CoUninitialize();

    return 0;   // Program successfully completed.
    return L'\0';
}

wchar_t WMIHelper::SetWMIPropertyByName(HRESULT& errCd, BSTR* rootPath, BSTR* wmiName, BSTR* wmiValue)
{
    wchar_t returnRev = L'\0';
    // Initialize COM
    errCd = CoInitializeEx(0, COINIT_MULTITHREADED);
    if (FAILED(errCd)) {
        cout << L"CoInitializeEx" << errCd << endl;
    }
    wchar_t sss[] = L"ROOT\\HP\\InstrumentedBIOS";
    // Connect to WMI
    IWbemServices* pSvc = ConnectToWmi(sss);
    if (pSvc == NULL) {				// cannot connect to WMI
        CoUninitialize();
        return returnRev;              // Program has failed.
    }

    // Get object path for ExecMethod
    IEnumWbemClassObject* pEnum = NULL;
    errCd = pSvc->CreateInstanceEnum(_bstr_t(L"HP_BIOSSettingInterface"),
        WBEM_FLAG_RETURN_IMMEDIATELY | WBEM_FLAG_FORWARD_ONLY,
        NULL,
        &pEnum);
    if (FAILED(errCd)) {
        cout << L"IWbemServices->CreateInstanceEnum" << errCd << endl;

        pSvc->Release();
        CoUninitialize();
        return returnRev;               // Program has failed.
    }

    IWbemClassObject* pInstance = NULL;
    ULONG ulReturned;
    errCd = pEnum->Next(WBEM_INFINITE, 1, &pInstance, &ulReturned);
    pEnum->Release();
    if (FAILED(errCd)) {
        cout << L"IEnumWbemClassObject->Next" << errCd << endl;

        pSvc->Release();
        CoUninitialize();
        return returnRev;               // Program has failed.
    }

    _variant_t vPath;
    errCd = pInstance->Get(_bstr_t(L"__PATH"), 0, &vPath, 0, 0);
    pInstance->Release();
    if (FAILED(errCd)) {
        cout << L"IWbemClassObject->Get(path)"<< errCd << endl;

        pSvc->Release();
        CoUninitialize();
        return returnRev;               // Program has failed.
    }

    // Get class name
    IWbemClassObject* pClsObj = NULL;
    errCd = pSvc->GetObject(_bstr_t(L"HP_BIOSSettingInterface"), 0, NULL, &pClsObj, NULL);
    if (FAILED(errCd)) {
        cout << L"IWbemService->GetObject"<< errCd << endl;

        pSvc->Release();
        CoUninitialize();
        return returnRev;               // Program has failed.
    }

    // Get method
    IWbemClassObject* pInParamObj = NULL;
    IWbemClassObject* pOutParamObj = NULL;
    bool isSetSystemDefaults = false;
    if (isSetSystemDefaults) {
        errCd = pClsObj->GetMethod(_bstr_t(L"SetSystemDefaults"), NULL, &pInParamObj, &pOutParamObj);
    }
    else {
        errCd = pClsObj->GetMethod(_bstr_t(L"SetBIOSSetting"), NULL, &pInParamObj, &pOutParamObj);
    }
    pClsObj->Release();
    if (FAILED(errCd)) {
        cout << L"IWbemClassObject->GetMethod"<< errCd << endl;

        pSvc->Release();
        CoUninitialize();
        return returnRev;               // Program has failed.
    }
    //GetWBMCLSProperty(pInParamObj);
    // Spawn instance to put input parameter
    IWbemClassObject* pInParamIns = NULL;
    errCd = pInParamObj->SpawnInstance(0, &pInParamIns);
   

    pInParamObj->Release();
    pOutParamObj->Release();
    if (FAILED(errCd)) {
        cout << L"IWbemClassObject->SpawnInstance"<< errCd << endl;

        pSvc->Release();
        CoUninitialize();
        return returnRev;               // Program has failed.
    }

    // put parameters
  /*  variant_t vName = wmiName;
    variant_t vValue = wmiValue;
    variant_t vPassword = L"";*/
    VARIANT vtInName, vtInValue, vtInPass;
    VariantInit(&vtInName);
    vtInName.vt = VT_BSTR;
    vtInName.bstrVal = *wmiName;
    VariantInit(&vtInValue);
    vtInValue.vt = VT_BSTR;
    vtInValue.bstrVal = *wmiValue;
    VariantInit(&vtInPass);
    vtInPass.vt = VT_BSTR;
    BSTR vtInSTR = SysAllocString(L"");
    vtInPass.bstrVal = vtInSTR;
    SysFreeString(vtInSTR);

   /* errCd = pInParamObj->Get(_bstr_t(L"Name"), 0, &vtInName, 0, 0);
    errCd = pInParamObj->Get(_bstr_t(L"Value"), 0, &vtInValue, 0, 0);
    errCd = pInParamObj->Get(_bstr_t(L"Password"), 0, &vtInPass, 0, 0);*/

    if (!isSetSystemDefaults) {
        // put name
        errCd = pInParamIns->Put(_bstr_t(L"Name"), 0, &vtInName, vtInName.vt);
        if (FAILED(errCd)) {
            cout << L"IWbemClassObject->Put(name)"<< errCd << endl;

            pInParamIns->Release();
            pSvc->Release();
            CoUninitialize();
            return returnRev;               // Program has failed.
        }

        // put value
        errCd = pInParamIns->Put(_bstr_t(L"Value"), 0, &vtInValue, vtInValue.vt);
        if (FAILED(errCd)) {
            cout << L"IWbemClassObject->Put(value)"<< errCd << endl;

            pInParamIns->Release();
            pSvc->Release();
            CoUninitialize();
            return returnRev;               // Program has failed.
        }
    }

    // put password
    errCd = pInParamIns->Put(_bstr_t(L"Password"), 0, &vtInPass, vtInPass.vt);
    if (FAILED(errCd)) {
        cout << L"IWbemClassObject->Put(password)"<< errCd << endl;

        pInParamIns->Release();
        pSvc->Release();
        CoUninitialize();
        return returnRev;               // Program has failed.
    }

    pOutParamObj = NULL;
    if (isSetSystemDefaults) {
        errCd = pSvc->ExecMethod(vPath.bstrVal, _bstr_t(L"SetSystemDefaults"), 0, NULL, pInParamIns, &pOutParamObj, NULL);
    }
    else {
        errCd = pSvc->ExecMethod(vPath.bstrVal, _bstr_t(L"SetBIOSSetting"), 0, NULL, pInParamIns, &pOutParamObj, NULL);
    }
    pInParamIns->Release();
    if (FAILED(errCd)) {
        cout << L"IWbemServices->ExecMethod"<< errCd << endl;

        pSvc->Release();
        CoUninitialize();
        return returnRev;               // Program has failed.
    }
    else {
        // Get the return code
        /*
            0 = Success
            1 = Not Supported
            2 = Unspecified Error
            3 = Timeout
            4 = Failed
            5 = Invalid Parameter
            6 = Access Denied
        */
        _variant_t vOutVal;
        errCd = pOutParamObj->Get(L"Return", 0, &vOutVal, 0, 0);
        pOutParamObj->Release();
        if (FAILED(errCd)) {
            cout << L"IWbemClassObject->Get(Return)"<< errCd << endl;

            pSvc->Release();
            CoUninitialize();
            return returnRev;               // Program has failed.
        }

        // set return code to response,
     /*   WValue returnCode;
        returnCode.SetUint(vOutVal);
        response->AddMember(RESULT_JSON_NAME, returnCode, response->GetAllocator());

        result = HP_HSA_SUCCESS;*/
    }

    // Cleanup
    // ========

    pSvc->Release();
    CoUninitialize();
    return L'\0';
}

IWbemServices* WMIHelper::ConnectToWmi(wchar_t* wmiNamespace) {
    // Set general COM security levels --------------------------
    HRESULT hr = CoInitializeSecurity(
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
    if (FAILED(hr)) {
        if (hr != RPC_E_TOO_LATE) {		// we can ignore RPC_E_TOO_LATE (CoInitializeSecurity has already been called)
            cout << L"CoInitializeSecurity" << hr << endl;
        }
    }

    // Obtain the initial locator to WMI -------------------------
    IWbemLocator* pLoc = NULL;
    hr = CoCreateInstance(
        CLSID_WbemLocator,
        0,
        CLSCTX_INPROC_SERVER,
        IID_IWbemLocator, (LPVOID*)&pLoc);
    if (FAILED(hr)) {
       cout << L"CoCreateInstance" << hr << endl;

        return NULL;                 // Program has failed.
    }

    // Connect to WMI through the IWbemLocator::ConnectServer method

    // Connect to the namespace with
    // the current user and obtain pointer pSvc
    // to make IWbemServices calls.
    IWbemServices* pSvc = NULL;
    hr = pLoc->ConnectServer(
        _bstr_t(wmiNamespace),   // Object path of WMI namespace
        NULL,                    // User name. NULL = current user
        NULL,                    // User password. NULL = current
        0,                       // Locale. NULL indicates current
        NULL,                    // Security flags.
        0,                       // Authority (for example, Kerberos)
        0,                       // Context object 
        &pSvc                    // pointer to IWbemServices proxy
    );
    pLoc->Release();
    if (FAILED(hr)) {
        cout << L"IWbemLocator->ConnectServer(WmiNamespace)" << hr << endl;

        if (WBEM_E_INVALID_NAMESPACE == hr) {
            //*result = WMI::WMIErrors::GET_WMI_NAMESPACE_INVALID_ERROR;
        }

        return NULL;                // Program has failed.
    }

    // Set security levels on the proxy -------------------------
    hr = CoSetProxyBlanket(
        pSvc,                        // Indicates the proxy to set
        RPC_C_AUTHN_WINNT,           // RPC_C_AUTHN_xxx
        RPC_C_AUTHZ_NONE,            // RPC_C_AUTHZ_xxx
        NULL,                        // Server principal name 
        RPC_C_AUTHN_LEVEL_CALL,      // RPC_C_AUTHN_LEVEL_xxx 
        RPC_C_IMP_LEVEL_IMPERSONATE, // RPC_C_IMP_LEVEL_xxx
        NULL,                        // client identity
        EOAC_NONE                    // proxy capabilities 
    );
    if (FAILED(hr)) {
        cout << L"CoSetProxyBlanket" << hr << endl;

        pSvc->Release();
        return NULL;               // Program has failed.
    }

    return pSvc;
}




