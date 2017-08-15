#include "CToastWarper.h"
#include <windows.ui.notifications.h>

using namespace Microsoft::Toolkit::Uwp::Notifications;
using namespace ABI::Windows::UI::Notifications;
//using namespace ABI::Windows::Data::Xml::Dom;
using namespace System::Xml;

ref class ToastCLR
{
public:
	
	static ToastContent ^ GetToastContent()
	{
		if (nullptr == toastContent)
		{
			toastContent = gcnew ToastContent;
			toastContent->Launch = "Shit123";
			AdaptiveText^ atxt = gcnew AdaptiveText();
			atxt->Text = gcnew BindableString("CCCC");
			ToastBindingGeneric ^ BindingGeneric = gcnew ToastBindingGeneric();
			BindingGeneric->Children->Add(atxt);

			ToastVisual ^ tVis = gcnew ToastVisual();
			tVis->BindingGeneric = BindingGeneric;
			
		}
		return toastContent;
	}
private:
	static ToastContent ^ toastContent = nullptr;
};

CCToastWarper::CCToastWarper()
{
}


CCToastWarper::~CCToastWarper()
{
}

void CCToastWarper::GetToastContent()
{
	ToastContent ^ tt = ToastCLR::GetToastContent();
	XmlDocument^ doc = gcnew XmlDocument;
	doc->LoadXml(tt->GetContent());
	
 
}
