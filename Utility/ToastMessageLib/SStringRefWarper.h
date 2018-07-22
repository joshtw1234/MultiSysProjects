#pragma once
class SStringRefWarper
{
public:
	SStringRefWarper();
	~SStringRefWarper();
	SStringRefWarper(_In_reads_(length) PCWSTR stringRef, _In_ UINT32 length);

	template <size_t N>
	SStringRefWarper(_In_reads_(N) wchar_t const (&stringRef)[N]);

	template <size_t _>
	SStringRefWarper(_In_reads_(_) wchar_t(&stringRef)[_]);

	HSTRING Get();
private:
	HSTRING             _hstring;
	HSTRING_HEADER      _header;
};

template<size_t N>
inline SStringRefWarper::SStringRefWarper(wchar_t const(&stringRef)[N])
{
	UINT32 length = N - 1;
	HRESULT hr = WindowsCreateStringReference(stringRef, length, &_header, &_hstring);

	if (FAILED(hr))
	{
		RaiseException(static_cast<DWORD>(STATUS_INVALID_PARAMETER), EXCEPTION_NONCONTINUABLE, 0, nullptr);
	}
}

template<size_t _>
inline SStringRefWarper::SStringRefWarper(wchar_t(&stringRef)[_])
{
	UINT32 length;
	HRESULT hr = SizeTToUInt32(wcslen(stringRef), &length);

	if (FAILED(hr))
	{
		RaiseException(static_cast<DWORD>(STATUS_INVALID_PARAMETER), EXCEPTION_NONCONTINUABLE, 0, nullptr);
	}

	WindowsCreateStringReference(stringRef, length, &_header, &_hstring);
}
