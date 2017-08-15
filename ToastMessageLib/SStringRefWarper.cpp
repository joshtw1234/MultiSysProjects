#include <winstring.h>
#include "SStringRefWarper.h"

#pragma comment(lib, "RuntimeObject.lib")



SStringRefWarper::SStringRefWarper()
{
}


SStringRefWarper::~SStringRefWarper()
{
	WindowsDeleteString(_hstring);
}

SStringRefWarper::SStringRefWarper(PCWSTR stringRef, UINT32 length)
{
	HRESULT hr = WindowsCreateStringReference(stringRef, length, &_header, &_hstring);

	if (FAILED(hr))
	{
		RaiseException(static_cast<DWORD>(STATUS_INVALID_PARAMETER), EXCEPTION_NONCONTINUABLE, 0, nullptr);
	}
}

HSTRING SStringRefWarper::Get()
{
	return _hstring;
}
