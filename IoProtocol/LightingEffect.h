#pragma once
#include <Windows.h>
#include "LightingData.h"
#include "IoProtocol.h"
#include <vector>
#include <queue>

#define WAIT_OBJECT_TIMEOUT 1000
#define MAX_ZONE_SIZE 10

class ILightingEffect
{
public:

	virtual BOOL SetCommand(_In_ LPIoCommand lpCommand) = 0;
	// Set / Get Lighting color & brightness by Zone
	virtual BOOL SetLedZoneColorInfo(_In_ LPLedZoneColorInfo lpZoneInfo, _In_  int  nArraySize) = 0; 
	virtual BOOL SetLedZoneColorInfo(_In_ LPLedZoneColorInfo lpZoneInfo, _In_  int  nArraySize, BYTE byBrightness) = 0;
	virtual BOOL GetLedZoneColorInfo(_Out_ LPLedZoneColorInfo lpZoneInfo, _In_ int nArraySize) = 0;

	virtual BOOL SetLightingOff() = 0;

	// Set / Get Monitor by Zone
	virtual BOOL SetLedZoneMonitorInfo(_In_ LPLedZoneMonitorInfo lpZoneInfo, _In_  int  nArraySize) = 0;
	virtual BOOL SetLedZoneMonitorInfo(_In_ LPLedZoneMonitorInfo lpZoneInfo, _In_  int  nArraySize, BYTE byBrightness) = 0;
	virtual BOOL GetLedZoneMonitorInfo(_Out_ LPLedZoneMonitorInfo lpZoneInfo) = 0;

	// Set / Get Audio by Zone
	virtual BOOL SetLedZoneAudioInfo(_In_ LPLedZoneAudioInfo lpZoneInfo, _In_  int  nArraySize) = 0;
	virtual BOOL SetLedZoneAudioInfo(_In_ LPLedZoneAudioInfo lpZoneInfo, _In_  int  nArraySize, BYTE byBrightness) = 0;
	virtual BOOL GetLedZoneAudioInfo(_Out_ LPLedZoneAudioInfo lpZoneInfo) = 0;

	// Color Show Mode
	virtual BOOL SetColorShow(_In_ LedZoneColorInfo lpZoneInfo[][MAX_ZONE_SIZE], _In_  int  nStageSize, _In_ int nArraySize, _In_ int nDuration, BYTE byBrightness) = 0;
};


class CLightingEffectNuvoton : public ILightingEffect
{
private:
	IIoProtocol  *m_IoProtocol;
	//HANDLE m_WriteThreadHandle;
	//HANDLE m_ReadThreadHandle;
	WORD CalCheckSum(unsigned char *buf, WORD size);

public:
	CLightingEffectNuvoton(IIoProtocol *protocol = NULL);
	virtual ~CLightingEffectNuvoton();

	//HANDLE GetWriteThreadHandle();
	//HANDLE GetReadThreadHandle();

	virtual BOOL SetCommand(_In_ LPIoCommand lpCommand);

	// Set / Get Lighting color & brightness by Zone
	virtual BOOL SetLedZoneColorInfo(_In_ LPLedZoneColorInfo lpZoneInfo, _In_  int  nArraySize);
	virtual BOOL SetLedZoneColorInfo(_In_ LPLedZoneColorInfo lpZoneInfo, _In_  int  nArraySize, BYTE byBrightness);
	virtual BOOL GetLedZoneColorInfo(_Out_ LPLedZoneColorInfo lpZoneInfo, _In_ int nArraySize);

	// Lighting Off Mode 
	virtual BOOL SetLightingOff();

	// Set / Get Monitor by Zone
	virtual BOOL SetLedZoneMonitorInfo(_In_ LPLedZoneMonitorInfo lpZoneInfo, _In_  int  nArraySize);
	virtual BOOL SetLedZoneMonitorInfo(_In_ LPLedZoneMonitorInfo lpZoneInfo, _In_  int  nArraySize, BYTE byBrightness);
	virtual BOOL GetLedZoneMonitorInfo(_Out_ LPLedZoneMonitorInfo lpZoneInfo);

	// Set / Get Audio by Zone
	virtual BOOL SetLedZoneAudioInfo(_In_ LPLedZoneAudioInfo lpZoneInfo, _In_  int  nArraySize);
	virtual BOOL SetLedZoneAudioInfo(_In_ LPLedZoneAudioInfo lpZoneInfo, _In_  int  nArraySize, BYTE byBrightness);
	virtual BOOL GetLedZoneAudioInfo(_Out_ LPLedZoneAudioInfo lpZoneInfo);

	virtual BOOL SetColorShow(_In_ LedZoneColorInfo lpZoneInfolpZoneInfo[][MAX_ZONE_SIZE], _In_  int  nStageSize, _In_ int nArraySize, _In_ int nDuration, BYTE byBrightness);

};
