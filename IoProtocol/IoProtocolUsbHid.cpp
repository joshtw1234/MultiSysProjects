#include "IoProtocolUsbHid.h"
extern "C"
{
	#include "setupapi.h"
	#include "hidsdi.h"
}

#pragma region IoProtocolUsbHid

IoProtocolUsbHid::IoProtocolUsbHid()
{
	this->m_ReadEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
	this->m_WriteEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
	this->m_CompareString = NULL;
}

BOOL IoProtocolUsbHid::OpenDevice()
{
	GUID HidGuid;
	HANDLE hDevHandle;
	SP_DEVICE_INTERFACE_DATA DevInterfaceData;
	HDEVINFO hDevInfoSet;
	PSP_DEVICE_INTERFACE_DETAIL_DATA	pDevDetailData;
	HIDD_ATTRIBUTES DevAttributes;
	DWORD MemberIndex = 0;
	DWORD RequiredSize;
	WCHAR MyDevPathName[MAX_PATH];
	BOOL DevFound = FALSE;
	DevInterfaceData.cbSize = sizeof(DevInterfaceData);
	DevAttributes.Size = sizeof(DevAttributes);

	HidD_GetHidGuid(&HidGuid);
	hDevInfoSet = SetupDiGetClassDevs(&HidGuid,
										NULL,
										NULL,
										DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);

	while (SetupDiEnumDeviceInterfaces(hDevInfoSet,
										NULL,
										&HidGuid,
										MemberIndex,
										&DevInterfaceData))
	{
		MemberIndex++;

		SetupDiGetDeviceInterfaceDetail(hDevInfoSet,
										&DevInterfaceData,
										NULL,
										NULL,
										&RequiredSize,
										NULL);
		pDevDetailData = (PSP_DEVICE_INTERFACE_DETAIL_DATA)malloc(RequiredSize);
		pDevDetailData->cbSize = sizeof(SP_DEVICE_INTERFACE_DETAIL_DATA);
		SetupDiGetDeviceInterfaceDetail(hDevInfoSet,
										&DevInterfaceData,
										pDevDetailData,
										RequiredSize,
										NULL,
										NULL);

		wcscpy_s(MyDevPathName, pDevDetailData->DevicePath);
		
		free(pDevDetailData);

		hDevHandle = CreateFile(MyDevPathName,
								NULL,
								FILE_SHARE_READ | FILE_SHARE_WRITE,
								NULL,
								OPEN_EXISTING,
								FILE_ATTRIBUTE_NORMAL,
								NULL);

		if (hDevHandle != INVALID_HANDLE_VALUE)
		{
			HidD_GetAttributes(hDevHandle, &DevAttributes);

			CloseHandle(hDevHandle);
			if (m_CompareString)
			{
				if (!wcsstr(MyDevPathName, m_CompareString))
				{
					continue;
				}
			}

			// find our device.
			if (DevAttributes.VendorID == this->m_Vid && DevAttributes.ProductID == this->m_Pid)
			{
				DevFound = TRUE;

				this->m_ReadHandle = CreateFile(MyDevPathName,
												GENERIC_READ,
					FILE_SHARE_READ | FILE_SHARE_WRITE,
												NULL,
												OPEN_EXISTING,
					FILE_FLAG_OVERLAPPED,
												NULL);

				this->m_WriteHandle = CreateFile(MyDevPathName,
												GENERIC_WRITE,
					FILE_SHARE_READ | FILE_SHARE_WRITE,
												NULL,
												OPEN_EXISTING,
					FILE_FLAG_OVERLAPPED,
												NULL);

				break;
			}
		}
	}

	SetupDiDestroyDeviceInfoList(hDevInfoSet);

	return DevFound;
}

BOOL IoProtocolUsbHid::CloseDevice()
{
	CloseHandle(this->m_ReadHandle);
	CloseHandle(this->m_WriteHandle);
	return 0;
}

BOOL IoProtocolUsbHid::Read(unsigned char *  lpBuffer, int nNumberOfBytesToRead, LPDWORD lpNumberOfBytesRead)
{
	OVERLAPPED overlapped;
	memset(&overlapped, 0, sizeof(overlapped));
	overlapped.hEvent = this->m_ReadEvent;
	//unsigned char buf[65];

	unsigned char *buf = new unsigned char[nNumberOfBytesToRead + 1];
	buf[0] = 0x01;
	memcpy_s(buf + 1, nNumberOfBytesToRead, lpBuffer, nNumberOfBytesToRead);

	::ReadFile(this->m_ReadHandle, buf, nNumberOfBytesToRead +1, NULL, &overlapped);

	if (GetLastError() == ERROR_IO_PENDING || GetLastError() == ERROR_SUCCESS)
	{
		if (WaitForSingleObject(this->m_ReadEvent, 3000) == WAIT_OBJECT_0)
		{	
			ResetEvent(this->m_ReadEvent);
			GetOverlappedResult(this->m_ReadHandle, &overlapped, lpNumberOfBytesRead, TRUE);
			lpNumberOfBytesRead--;
//			memcpy(lpBuffer, buf, 64);
			memcpy_s(lpBuffer, nNumberOfBytesToRead, buf+1, nNumberOfBytesToRead);


			return TRUE;
		}
	}

	return FALSE;
}

BOOL IoProtocolUsbHid::Write(unsigned char *  lpBuffer, int nNumberOfBytesToWrite, LPDWORD lpNumberOfBytesWritten)
{
	OVERLAPPED overlapped;
	memset(&overlapped, 0, sizeof(overlapped));
	ResetEvent(this->m_WriteEvent);
	overlapped.hEvent = this->m_WriteEvent;

	unsigned char *buf = new unsigned char[nNumberOfBytesToWrite + 1];
	buf[0] = 0x01;
	memcpy_s(buf + 1, nNumberOfBytesToWrite, lpBuffer, nNumberOfBytesToWrite);
	::WriteFile(this->m_WriteHandle, buf, nNumberOfBytesToWrite + 1, NULL, &overlapped);
	int a = GetLastError();
	if (GetLastError() == ERROR_IO_PENDING || GetLastError() == ERROR_SUCCESS)
	{
		if (WaitForSingleObject(overlapped.hEvent, 3000) == WAIT_OBJECT_0)
		{
			ResetEvent(this->m_WriteEvent);
			GetOverlappedResult(this->m_WriteHandle, &overlapped, lpNumberOfBytesWritten, TRUE);
			lpNumberOfBytesWritten--;
			delete[] buf;
			return TRUE;
		}
	}

	delete[] buf;
	return FALSE;
}
#pragma endregion

#pragma region IoProtocolUsbHidNuvoton

IoProtocolUsbHidNuvoton::IoProtocolUsbHidNuvoton(WORD wPid, WORD wVid)
{
	this->m_Pid = wPid;
	this->m_Vid = wVid;
}

IoProtocolUsbHidNuvoton::~IoProtocolUsbHidNuvoton()
{

}
#pragma endregion

#pragma region IoProtocolUsbHidDafron

IoProtocolUsbHidDafron::IoProtocolUsbHidDafron(WORD wPid, WORD wVid)
{
	this->m_Pid = wPid;
	this->m_Vid = wVid;
}

IoProtocolUsbHidDafron::~IoProtocolUsbHidDafron()
{
}

void IoProtocolUsbHidDafron::SetFeature()
{
	/*
	BOOLEAN __stdcall HidD_SetFeature(
  _In_ HANDLE HidDeviceObject,
  _In_ PVOID  ReportBuffer,
  _In_ ULONG  ReportBufferLength
);
	*/

	unsigned char report[9] = { 0x0, 
		                        0x08, 0x0, 0x03, 0x0A, 0x32, 0x01, 0x04, 0};
	report[8] = 0xFF - report[1] - report[2] - report[3] - report[4] - report[5] - report[6] - report[7];

	BOOLEAN result = HidD_SetFeature(this->m_WriteHandle, report, 9);
	int a = GetLastError();
	a++;
}

#pragma endregion

IoProtocolUsbHidPrimax::IoProtocolUsbHidPrimax(WORD wPid, WORD wVid, _In_ WCHAR *strCompareString)
{
	this->m_Pid = wPid;
	this->m_Vid = wVid;
	this->m_CompareString = strCompareString;
}

IoProtocolUsbHidPrimax::~IoProtocolUsbHidPrimax()
{
}