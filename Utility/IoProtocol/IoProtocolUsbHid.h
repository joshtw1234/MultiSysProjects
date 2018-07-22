#pragma once
#include "IoProtocol.h"

class IoProtocolUsbHid : public  IIoProtocol
{
private:
	HANDLE m_ReadEvent;
	HANDLE m_WriteEvent;
	
protected:
	IoProtocolUsbHid();  // protected constructor, client can not new this class
	WORD m_Pid;
	WORD m_Vid;
	WCHAR *m_CompareString;

public:
	virtual BOOL OpenDevice();
	virtual BOOL CloseDevice();
	virtual BOOL Read(_In_ unsigned char * lpBuffer, _In_  int nNumberOfBytesToRead, _Out_  LPDWORD lpNumberOfBytesRead);
	virtual BOOL Write(_In_ unsigned char * lpBuffer, _In_  int nNumberOfBytesToWrite, _Out_  LPDWORD lpNumberOfBytesWritten);

};

class IoProtocolUsbHidNuvoton : public IoProtocolUsbHid
{
public:
	IoProtocolUsbHidNuvoton(_In_ WORD wPid, _In_ WORD wVid);
	~IoProtocolUsbHidNuvoton();
};

class IoProtocolUsbHidDafron : public IoProtocolUsbHid
{
public:
	IoProtocolUsbHidDafron(_In_ WORD wPid, _In_ WORD wVid);
	~IoProtocolUsbHidDafron();
	void SetFeature();
};

class IoProtocolUsbHidPrimax : public IoProtocolUsbHid
{
public:
	IoProtocolUsbHidPrimax(_In_ WORD wPid, _In_ WORD wVid, _In_ WCHAR *strCompareString);
	~IoProtocolUsbHidPrimax();
};