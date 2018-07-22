#include <wrl\module.h>
#include <strsafe.h>
#include "SStringRefWarper.h"
#include "SToastMsg.h"
#include "ToastEventHandler.h"

using namespace ABI::Windows::UI::Notifications;
using namespace ABI::Windows::Data::Xml::Dom;
using namespace Microsoft::WRL;
using namespace Microsoft::WRL::Wrappers;
using namespace Windows::Foundation;


CSToastMsg::CSToastMsg()
{
}


CSToastMsg::~CSToastMsg()
{
}

#pragma region Inside Methods

HRESULT SetNodeValueString(_In_ HSTRING inputString, _In_ IXmlNode *node, _In_ IXmlDocument *xml)
{
	ComPtr<IXmlText> inputText;

	HRESULT hr = xml->CreateTextNode(inputString, &inputText);
	if (SUCCEEDED(hr))
	{
		ComPtr<IXmlNode> inputTextNode;

		hr = inputText.As(&inputTextNode);
		if (SUCCEEDED(hr))
		{
			ComPtr<IXmlNode> pAppendedChild;
			hr = node->AppendChild(inputTextNode.Get(), &pAppendedChild);
		}
	}

	return hr;
}

HRESULT SetImageSrc(_In_z_ wchar_t *imagePath, _In_ IXmlDocument *toastXml)
{
	wchar_t imageSrc[MAX_PATH] = L"file:///";
	HRESULT hr = StringCchCat(imageSrc, ARRAYSIZE(imageSrc), imagePath);
	if (SUCCEEDED(hr))
	{
		/*::StringCchPrintfW(g_DbgLogOutput.strDebugLogOutputString, DEBUG_MAX_STRING, L"%s::%s", __FUNCTIONW__, imageSrc);
		g_DbgLogOutput.Output();*/
		ComPtr<IXmlNodeList> nodeList;
		hr = toastXml->GetElementsByTagName(SStringRefWarper(L"image").Get(), &nodeList);
		if (SUCCEEDED(hr))
		{
			ComPtr<IXmlNode> imageNode;
			hr = nodeList->Item(0, &imageNode);
			if (SUCCEEDED(hr))
			{
				ComPtr<IXmlNamedNodeMap> attributes;

				hr = imageNode->get_Attributes(&attributes);
				if (SUCCEEDED(hr))
				{
					ComPtr<IXmlNode> srcAttribute;

					hr = attributes->GetNamedItem(SStringRefWarper(L"src").Get(), &srcAttribute);
					if (SUCCEEDED(hr))
					{
						hr = SetNodeValueString(SStringRefWarper(imageSrc).Get(), srcAttribute.Get(), toastXml);
					}
				}
			}
		}
	}
	return hr;
}
#pragma endregion

#if true

void CSToastMsg::DisplayToast(int xmlIdx)
{
	CoInitialize(NULL);

	ComPtr<IToastNotificationManagerStatics> toastStatics;
	//Step 1 Create toast manager.
	HRESULT hr = GetActivationFactory(SStringRefWarper(RuntimeClass_Windows_UI_Notifications_ToastNotificationManager).Get(), &toastStatics);
	if (SUCCEEDED(hr))
	{
		//Step 2 Create Notifier
		ComPtr<IToastNotifier> notifier;
		hr = toastStatics->CreateToastNotifierWithId(SStringRefWarper(L"OMEN Command Center").Get(), &notifier);
		if (SUCCEEDED(hr))
		{
			ComPtr<IToastNotification> toast;
			ComPtr<IXmlDocument> toastXml;
			ComPtr<IToastNotificationFactory> factory;
			//Step 3 Create Factory
			hr = GetActivationFactory(SStringRefWarper(RuntimeClass_Windows_UI_Notifications_ToastNotification).Get(), &factory);
			if (SUCCEEDED(hr))
			{
				//Step 4 Get temp toast XML
				//hr = toastStatics->GetTemplateContent(ToastTemplateType_ToastImageAndText04, &toastXml);
				switch (xmlIdx)
				{
				case 1:
					hr = GetButtonToastXml(&toastXml);
					break;
				default:
					hr = GetDefaultToastXml(&toastXml);
					break;
				}

				if (SUCCEEDED(hr))
				{
					wchar_t *imagePath = _wfullpath(nullptr, L"hpomen.ico", MAX_PATH);
					hr = imagePath != nullptr ? S_OK : HRESULT_FROM_WIN32(ERROR_FILE_NOT_FOUND);
					if (SUCCEEDED(hr))
					{
						/*::StringCchPrintfW(g_DbgLogOutput2.strDebugLogOutputString, DEBUG_MAX_STRING, L"%s::%s", __FUNCTIONW__, imagePath);
						g_DbgLogOutput2.Output();*/
						hr = SetImageSrc(imagePath, toastXml.Get());
						if (SUCCEEDED(hr))
						{
							/*::StringCchPrintfW(g_DbgLogOutput.strDebugLogOutputString, DEBUG_MAX_STRING, L"%s::Set Image good", __FUNCTIONW__);
							g_DbgLogOutput.Output();*/
							//Step 5 Create Toast message
							hr = factory->CreateToastNotification(toastXml.Get(), &toast);
							if (SUCCEEDED(hr))
							{
								//Step 6 Register the event handlers
								EventRegistrationToken activatedToken, dismissedToken, failedToken;
								ComPtr<ToastEventHandler> eventHandler(new ToastEventHandler());
								//ComPtr<ToastEventHandler> eventHandler(new ToastEventHandler(myPipeSrv));
								//ComPtr<ToastEventHandler> eventHandler(new ToastEventHandler(_mainWND));

								hr = toast->add_Activated(eventHandler.Get(), &activatedToken);
								if (SUCCEEDED(hr))
								{
									hr = toast->add_Dismissed(eventHandler.Get(), &dismissedToken);
									if (SUCCEEDED(hr))
									{
										hr = toast->add_Failed(eventHandler.Get(), &failedToken);
										if (SUCCEEDED(hr))
										{
											//Step 7 Show toast message
											hr = notifier->Show(toast.Get());
										}
									}
								}
							}
						}
					}
				}

			}
		}
	}
}


HRESULT CSToastMsg::GetButtonToastXml(ABI::Windows::Data::Xml::Dom::IXmlDocument ** inputXml)
{
	HStringReference toastXML(
		L"<toast scenario=\"reminder\">"
		L" <visual version=\"1\">"
		//L" <binding template=\"ToastGeneric\">"
		L" <binding template=\"ToastImageAndText02\">"
		L" <image id=\"1\" src=\"\" alt=\"Placeholder image\" />"
		L" <text id=\"1\">OMEN Command Center</text>"
		L" <text id=\"1\">Uh oh! Looks like the dark side won and the update did not work. What would you like to do?</text>"
		L" </binding>"
		L" </visual>"
		L" <actions>"
		L"  <action activationType=\"foreground\" content=\"Cancel\" arguments=\"Cancel\" />"
		L"  <action activationType=\"foreground\" content=\"Try again\" arguments=\"ReTry\" />"
		L" </actions>"
		L"</toast>");

	//Create XML Doc IO
	ComPtr<ABI::Windows::Data::Xml::Dom::IXmlDocumentIO> xmlDocument;
	HRESULT hr = Windows::Foundation::ActivateInstance(SStringRefWarper(RuntimeClass_Windows_Data_Xml_Dom_XmlDocument).Get(), &xmlDocument);
	if (SUCCEEDED(hr))
	{
		//Load XML from HString
		//hr = xmlDocument->LoadXml(StringRefWarper(xmlStr, lenSz).Get());
		hr = xmlDocument->LoadXml(toastXML.Get());
		if (SUCCEEDED(hr))
		{
			hr = xmlDocument.CopyTo(inputXml);
		}
	}
	return hr;
}

HRESULT CSToastMsg::GetDefaultToastXml(ABI::Windows::Data::Xml::Dom::IXmlDocument ** inputXml)
{
	HStringReference toastXML(
		L"<toast scenario=\"reminder\">"
		L" <visual version=\"1\">"
		//L" <binding template=\"ToastGeneric\">"
		L" <binding template=\"ToastImageAndText02\">"
		L" <image id=\"1\" src=\"\" alt=\"Placeholder image\" />"
		L" <text id=\"1\">OMEN Command Center</text>"
		L" <text id=\"1\">We are downloading and installing the new update. This might take a few minutes.</text>"
		L" <text id=\"1\"></text>"
		L" </binding>"
		L" </visual>"
		/*L" <actions>"
		L"  <action activationType=\"foreground\" content=\"Cancel\" arguments=\"Cancel\" />"
		L"  <action activationType=\"foreground\" content=\"Try again\" arguments=\"ReTry\" />"
		L" </actions>"*/
		L"</toast>");

	//Create XML Doc IO
	ComPtr<ABI::Windows::Data::Xml::Dom::IXmlDocumentIO> xmlDocument;
	HRESULT hr = Windows::Foundation::ActivateInstance(SStringRefWarper(RuntimeClass_Windows_Data_Xml_Dom_XmlDocument).Get(), &xmlDocument);
	if (SUCCEEDED(hr))
	{
		//Load XML from HString
		//hr = xmlDocument->LoadXml(StringRefWarper(xmlStr, lenSz).Get());
		hr = xmlDocument->LoadXml(toastXML.Get());
		if (SUCCEEDED(hr))
		{
			hr = xmlDocument.CopyTo(inputXml);
		}
	}
	return hr;
}

HRESULT CSToastMsg::TestGetXml(int xmlIdx, LPCWSTR xmlStr, ABI::Windows::Data::Xml::Dom::IXmlDocument ** inputXml)
{
	HStringReference toastXML(
		L"<toast scenario=\"reminder\">"
		L" <visual version=\"1\">"
		//L" <binding template=\"ToastGeneric\">"
		L" <binding template=\"ToastImageAndText02\">"
		L" <image id=\"1\" src=\"\" alt=\"Placeholder image\" />"
		L" <text id=\"1\">OMEN Command Center</text>"
		L" <text id=\"1\">Uh oh! Looks like the dark side won and the update did not work. What would you like to do?</text>"
		L" </binding>"
		L" </visual>"
		L" <actions>"
		L"  <action activationType=\"foreground\" content=\"Cancel\" arguments=\"Cancel\" />"
		L"  <action activationType=\"foreground\" content=\"Try again\" arguments=\"ReTry\" />"
		L" </actions>"
		L"</toast>");

	//Create XML Doc IO
	ComPtr<ABI::Windows::Data::Xml::Dom::IXmlDocumentIO> xmlDocument;
	HRESULT hr = Windows::Foundation::ActivateInstance(SStringRefWarper(RuntimeClass_Windows_Data_Xml_Dom_XmlDocument).Get(), &xmlDocument);
	if (SUCCEEDED(hr))
	{
		//Load XML from HString
		//hr = xmlDocument->LoadXml(StringRefWarper(xmlStr, lenSz).Get());
		hr = xmlDocument->LoadXml(toastXML.Get());
		if (SUCCEEDED(hr))
		{
			hr = xmlDocument.CopyTo(inputXml);
		}
	}
	return hr;
}
#endif



