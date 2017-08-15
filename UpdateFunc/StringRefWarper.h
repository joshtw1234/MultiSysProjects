#pragma once
#include <winstring.h>
class StringRefWarper
{
public:
	StringRefWarper();
	~StringRefWarper();
	//StringRefWarper(_In_reads_(length) PCWSTR stringRef, _In_ UINT32 length);
#if true
	StringRefWarper(_In_reads_(length) PCWSTR stringRef, _In_ UINT32 length) throw()
	{
		HRESULT hr = WindowsCreateStringReference(stringRef, length, &_header, &_hstring);

		if (FAILED(hr))
		{
			RaiseException(static_cast<DWORD>(STATUS_INVALID_PARAMETER), EXCEPTION_NONCONTINUABLE, 0, nullptr);
		}
	}

	template <size_t N>
	StringRefWarper(_In_reads_(N) wchar_t const (&stringRef)[N]) throw()
	{
		UINT32 length = N - 1;
		HRESULT hr = WindowsCreateStringReference(stringRef, length, &_header, &_hstring);

		if (FAILED(hr))
		{
			RaiseException(static_cast<DWORD>(STATUS_INVALID_PARAMETER), EXCEPTION_NONCONTINUABLE, 0, nullptr);
		}
	}

	template <size_t _>
	StringRefWarper(_In_reads_(_) wchar_t(&stringRef)[_]) throw()
	{
		UINT32 length;
		HRESULT hr = SizeTToUInt32(wcslen(stringRef), &length);

		if (FAILED(hr))
		{
			RaiseException(static_cast<DWORD>(STATUS_INVALID_PARAMETER), EXCEPTION_NONCONTINUABLE, 0, nullptr);
		}

		WindowsCreateStringReference(stringRef, length, &_header, &_hstring);
	}

	HSTRING Get() const throw()
	{
		return _hstring;
	}
#endif
private:
	HSTRING             _hstring;
	HSTRING_HEADER      _header;
};

