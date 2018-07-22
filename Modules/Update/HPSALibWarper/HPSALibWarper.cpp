#include "HPSALibWarper.h"
#include "DebugLogOutput.h"
#include <vcclr.h>

using namespace System::Collections::Generic;
using namespace HP::SupportAssistant;

DebugLogOutput g_DbgLogOutput(L"HPSAWarpLib.log");

ref class HPSALIBCLR
{
public :
	static System::String ^ strOMEN = "OMEN Command Center";
	static System::String ^ strDebug = "HP Orbit";
	
	static AppUpdates ^ GetHPSAInstance()
	{
		if (appUp == nullptr)
		{
			appUp = gcnew AppUpdates();
		}
		return appUp;
	}
private :
	static AppUpdates ^ appUp = nullptr;
	
};
/*
C++ Ref methods
*/
HPSALibWarper::HPSALibWarper()
{
	
}


HPSALibWarper::~HPSALibWarper()
{
}

double HPSALibWarper::GetHPSALastAnalysis()
{
	AppUpdates ^ appUpdate = HPSALIBCLR::GetHPSAInstance();
	System::DateTime dtLastAnalyzse = appUpdate->GetLastAnalysisDate();
	System::DateTime dtNow = System::DateTime::Now;
	/*::StringCchPrintfW(g_DbgLogOutput.strDebugLogOutputString, DEBUG_MAX_STRING, L"%s", __FUNCTIONW__);
	g_DbgLogOutput.Output();*/
	return (dtNow - dtLastAnalyzse).TotalSeconds;

}

bool HPSALibWarper::HPSARunAnalysis()
{
	AppUpdates ^ appUpdate = HPSALIBCLR::GetHPSAInstance();
	return appUpdate->RunAnalysis(nullptr, 1);
}

int HPSALibWarper::GetOMENUpdate(int rIdx, int * uPri)
{
	int revIdx = -1;
	AppUpdates ^ appUpdate = HPSALIBCLR::GetHPSAInstance();
	::StringCchPrintfW(g_DbgLogOutput.strDebugLogOutputString, DEBUG_MAX_STRING, L"%s::Get HPSA API",__FUNCTIONW__);
	g_DbgLogOutput.Output();

	switch (rIdx)
	{
	case 1:
		appUpdate->RunAnalysis(nullptr, 1);
		return 0;
		break;
	default:
		break;
	}
	
	//Get Update List
	revIdx = 2;//No OMEN Update.
	List<HPSAUpdate^>^ lstUpdats = appUpdate->GetUpdatesList(nullptr, nullptr);
	for each (HPSAUpdate^ updateItem in lstUpdats)
	{
		if (updateItem->PackageTitle->Equals(HPSALIBCLR::strOMEN))
		{
			//Find OMEN Update.
			revIdx = 1;
			*uPri = updateItem->Importance;
		}
		pin_ptr<const wchar_t> pinchars = PtrToStringChars(updateItem->PackageTitle);
		::StringCchPrintfW(g_DbgLogOutput.strDebugLogOutputString, DEBUG_MAX_STRING, L"%s::Update Item [%s] Importence [%d] ", __FUNCTIONW__, pinchars, updateItem->Importance);
		g_DbgLogOutput.Output();

	}
	
	return revIdx;
}

bool HPSALibWarper::IsHPSARunning()
{
	AppUpdates^ app = HPSALIBCLR::GetHPSAInstance();
	return app->IsAnalysisRunning();
}



