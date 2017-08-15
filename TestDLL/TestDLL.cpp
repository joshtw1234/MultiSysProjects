// TestDLL.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "SToastMsg.h"
#include "TestDLL.h"

CSToastMsg tos;
TESTDLL_API void Initialize(void)
{
	tos.DisplayToast(1);
}

TESTDLL_API void Uninitialize(void)
{
	//return TESTDLL_API void();

}

TESTDLL_API bool IsSupport(void)
{
	return true;
	//return TESTDLL_API bool();
}

TESTDLL_API void OMENPwerChange(void)
{
	int thisisi = 0;
	//tos.DisplayToast();
}

TESTDLL_API void ForeGroundChange(void)
{
	
}
