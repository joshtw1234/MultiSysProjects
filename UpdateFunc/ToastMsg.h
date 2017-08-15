#pragma once
#include <windows.ui.notifications.h>

class CToastMsg
{
public:
	CToastMsg();
	~CToastMsg();
	void DisplayToast(int xmlIdx);
private:
	HRESULT GetButtonToastXml(ABI::Windows::Data::Xml::Dom::IXmlDocument ** inputXml);
	HRESULT GetDefaultToastXml(ABI::Windows::Data::Xml::Dom::IXmlDocument ** inputXml);
	HRESULT TestGetXml(int xmlIdx, LPCWSTR xmlStr, _Outptr_ ABI::Windows::Data::Xml::Dom::IXmlDocument** inputXml);

};

