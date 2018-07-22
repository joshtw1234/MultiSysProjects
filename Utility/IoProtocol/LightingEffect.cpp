#include "LightingEffect.h"
#include "IoProtocolUsbHid.h"

WORD CLightingEffectNuvoton::CalCheckSum(unsigned char * buf, WORD size)
{
	WORD sum;
	int i;

	i = 0;
	sum = 0;
	while (size--)
	{
		sum += buf[i++];
	}

	return sum;
}

CLightingEffectNuvoton::CLightingEffectNuvoton(IIoProtocol * protocol)
{
	if (protocol == NULL)
	{
		this->m_IoProtocol = new IoProtocolUsbHidNuvoton(0xE00C, 0x0416);
		
	}
	else
	{
		this->m_IoProtocol = protocol;
	}

	//this->m_IoProtocol->OpenDevice(); // do not want to handle IIoProtocol OpenDevice/CloseDevice here
}

CLightingEffectNuvoton::~CLightingEffectNuvoton()
{
	//this->m_IoProtocol->CloseDevice(); // do not want to handle IIoProtocol OpenDevice/CloseDevice here
	/*
	if (this->m_IoProtocol)
	{
		delete this->m_IoProtocol;
		this->m_IoProtocol = NULL;
	}
	*/
}

BOOL CLightingEffectNuvoton::SetLedZoneAudioInfo(LPLedZoneAudioInfo lpZoneInfo, int nArraySize)
{
	DWORD dwBytesWrite = 0;
	IoCommand cmd = { 0 };
	memset(&cmd, 0, sizeof(IoCommand));

	cmd.eType = CommandType::SET_ZONE_INFO;
	cmd.eMode = CommandMode::AUDIO_MONITOR_MODE;
	cmd.byNumOfTotalpackages = 1;
	cmd.byPackageIndex = 1;
	cmd.wDuration = NOT_USED;
	memcpy(cmd.LedZonesInfo.AudioZoneInfo, lpZoneInfo, sizeof(LedZoneAudioInfo) * nArraySize);
	cmd.byBrightness = MAX_BRIGHTNESS;
	cmd.byArraySizeUsed = nArraySize;

	return this->m_IoProtocol->Write((unsigned char *)&(cmd), sizeof(cmd), &dwBytesWrite);
}

BOOL CLightingEffectNuvoton::SetLedZoneAudioInfo(LPLedZoneAudioInfo lpZoneInfo, int nArraySize, BYTE byBrightness)
{
	DWORD dwBytesWrite = 0;
	IoCommand cmd = { 0 };
	memset(&cmd, 0, sizeof(IoCommand));

	cmd.eType = CommandType::SET_ZONE_INFO;
	cmd.eMode = CommandMode::AUDIO_MONITOR_MODE;
	cmd.byNumOfTotalpackages = 1;
	cmd.byPackageIndex = 1;
	cmd.wDuration = NOT_USED;
	memcpy(cmd.LedZonesInfo.AudioZoneInfo, lpZoneInfo, sizeof(LedZoneAudioInfo) * nArraySize);
	cmd.byBrightness = byBrightness;
	cmd.byArraySizeUsed = nArraySize;

	return this->m_IoProtocol->Write((unsigned char *)&(cmd), sizeof(cmd), &dwBytesWrite);
}

BOOL CLightingEffectNuvoton::GetLedZoneAudioInfo(_Out_ LPLedZoneAudioInfo lpZoneInfo)
{
	return FALSE;
}

BOOL CLightingEffectNuvoton::SetColorShow(_In_ LedZoneColorInfo lpZoneInfo[][MAX_ZONE_SIZE], _In_  int  nStageSize, _In_ int nArraySize, _In_ int nDuration, BYTE byBrightness)
{
	BOOL result;
	DWORD dwBytesWrite = 0;
	IoCommand cmd = { 0 };
	memset(&cmd, 0, sizeof(IoCommand));

	cmd.eType = CommandType::SET_ZONE_INFO;
	cmd.eMode = CommandMode::COLOR_SHOW_MODE;
	cmd.byNumOfTotalpackages = nStageSize;

	cmd.wDuration = nDuration;
	cmd.byBrightness = byBrightness;
	cmd.byArraySizeUsed = nArraySize;

	for (int i = 0; i < nStageSize; i++)
	{
		memcpy(cmd.LedZonesInfo.ColorZoneInfo, lpZoneInfo[i], sizeof(LedZoneColorInfo) * nArraySize);
		cmd.byPackageIndex = i+1;
		result = this->m_IoProtocol->Write((unsigned char *)&(cmd), sizeof(cmd), &dwBytesWrite);
		if (!result)
			break;
		
		Sleep(20);
	}



	return result;
}

BOOL CLightingEffectNuvoton::SetCommand(LPIoCommand lpCommand)
{
	DWORD dwBytesWrite = 0;

	return this->m_IoProtocol->Write((unsigned char *)lpCommand, 64, &dwBytesWrite);
	//return this->m_IoProtocol->Write((unsigned char *)lpCommand, sizeof(IoCommand), &dwBytesWrite);
}

BOOL CLightingEffectNuvoton::SetLedZoneColorInfo(LPLedZoneColorInfo lpZoneInfo, int nArraySize)
{
	DWORD dwBytesWrite = 0;
	IoCommand cmd = { 0 };
	memset(&cmd, 0, sizeof(IoCommand));

	cmd.eType = CommandType::SET_ZONE_INFO;
	cmd.eMode = CommandMode::STATIC_COLOR_MODE;
	cmd.byNumOfTotalpackages = 1;
	cmd.byPackageIndex = 1;
	cmd.wDuration = NOT_USED;
	memcpy(cmd.LedZonesInfo.ColorZoneInfo, lpZoneInfo, sizeof(LedZoneColorInfo) * nArraySize);
	cmd.byBrightness = DEFAULT_BRIGHTNESS;
	cmd.byArraySizeUsed = nArraySize;

	return this->m_IoProtocol->Write((unsigned char *)&(cmd), sizeof(cmd), &dwBytesWrite);
}

BOOL CLightingEffectNuvoton::SetLedZoneColorInfo(LPLedZoneColorInfo lpZoneInfo, int  nArraySize, BYTE byBrightness)
{
	DWORD dwBytesWrite = 0;
	IoCommand cmd = { 0 };
	memset(&cmd, 0, sizeof(IoCommand));

	cmd.eType = CommandType::SET_ZONE_INFO;
	cmd.eMode = CommandMode::STATIC_COLOR_MODE;
	cmd.byNumOfTotalpackages = 1;
	cmd.byPackageIndex = 1;
	cmd.wDuration = NOT_USED;
	memcpy(cmd.LedZonesInfo.ColorZoneInfo, lpZoneInfo, sizeof(LedZoneColorInfo) * nArraySize);
	cmd.byBrightness = byBrightness;
	cmd.byArraySizeUsed = nArraySize;

	//PipeCommand pipeCmd;
	//pipeCmd.commandId = 0xA1;
	//memcpy(pipeCmd.data, &cmd, sizeof(IoCommand));

	//return this->m_IoProtocol->Write((unsigned char *)&(pipeCmd), sizeof(pipeCmd), &dwBytesWrite);
	return this->m_IoProtocol->Write((unsigned char *)&(cmd), sizeof(cmd), &dwBytesWrite);
}


BOOL CLightingEffectNuvoton::GetLedZoneColorInfo(LPLedZoneColorInfo lpZoneInfo, int nArraySize)
{
	DWORD result = 0, dwBytesRead = 0;
	IoCommand cmd;
	memset(&cmd, 0, sizeof(IoCommand));

	cmd.eType = CommandType::GET_ZONE_INFO;
	cmd.eMode = CommandMode::STATIC_COLOR_MODE;	
	cmd.byNumOfTotalpackages = 1;
	cmd.byPackageIndex = 1;
	cmd.wDuration = NOT_USED;

	cmd.byBrightness = 255;
	cmd.byArraySizeUsed = nArraySize;
	this->m_IoProtocol->Read((unsigned char *)&(cmd), sizeof(cmd), &dwBytesRead);
	
	if (GetLastError() != 0)
	{
		return FALSE;
	}

	memcpy(lpZoneInfo, cmd.LedZonesInfo.ColorZoneInfo,  sizeof(LPLedZoneColorInfo) * nArraySize);

	return TRUE;
}

BOOL CLightingEffectNuvoton::SetLightingOff()
{
	DWORD dwBytesWrite = 0;
	IoCommand cmd;
	memset(&cmd, 0, sizeof(IoCommand));
	
	cmd.eType = CommandType::SET_ZONE_INFO;
	cmd.eMode = CommandMode::LIGHTING_OFF_MODE;
	cmd.byNumOfTotalpackages = 1;
	cmd.byPackageIndex = 1;
	cmd.byArraySizeUsed = 0;

	return this->m_IoProtocol->Write((unsigned char *)&(cmd), sizeof(cmd), &dwBytesWrite);

	return 0;
}

BOOL CLightingEffectNuvoton::SetLedZoneMonitorInfo(LPLedZoneMonitorInfo lpZoneInfo, int nArraySize)
{
	DWORD dwBytesWrite = 0;
	IoCommand cmd = { 0 };
	memset(&cmd, 0, sizeof(IoCommand));

	cmd.eType = CommandType::SET_ZONE_INFO;
	cmd.eMode = CommandMode::SYSTEM_MONITOR_MODE;
	cmd.byNumOfTotalpackages = 1;
	cmd.byPackageIndex = 1;
	cmd.wDuration = NOT_USED;
	memcpy(cmd.LedZonesInfo.MonitorZoneInfo, lpZoneInfo, sizeof(LedZoneMonitorInfo) * nArraySize);
	cmd.byBrightness = DEFAULT_BRIGHTNESS;
	cmd.byArraySizeUsed = nArraySize;

	return this->m_IoProtocol->Write((unsigned char *)&(cmd), sizeof(cmd), &dwBytesWrite);
}

BOOL CLightingEffectNuvoton::SetLedZoneMonitorInfo(_In_ LPLedZoneMonitorInfo lpZoneInfo, _In_  int  nArraySize, BYTE byBrightness)
{
	DWORD dwBytesWrite = 0;
	IoCommand cmd = { 0 };
	memset(&cmd, 0, sizeof(IoCommand));

	cmd.eType = CommandType::SET_ZONE_INFO;
	cmd.eMode = CommandMode::SYSTEM_MONITOR_MODE;
	cmd.byNumOfTotalpackages = 1;
	cmd.byPackageIndex = 1;
	cmd.wDuration = NOT_USED;
	memcpy(cmd.LedZonesInfo.MonitorZoneInfo, lpZoneInfo, sizeof(LedZoneMonitorInfo) * nArraySize);
	cmd.byBrightness = byBrightness;
	cmd.byArraySizeUsed = nArraySize;

	//PipeCommand pipeCmd;
	//pipeCmd.commandId = 0xA1;
	//memcpy(pipeCmd.data, &cmd, sizeof(IoCommand));

	//return this->m_IoProtocol->Write((unsigned char *)&(pipeCmd), sizeof(pipeCmd), &dwBytesWrite);
	return this->m_IoProtocol->Write((unsigned char *)&(cmd), sizeof(cmd), &dwBytesWrite);
}

BOOL CLightingEffectNuvoton::GetLedZoneMonitorInfo(LPLedZoneMonitorInfo lpZoneInfo)
{
	DWORD dwBytesWrite = 0;
	IoCommand cmd = { 0 };
	memset(&cmd, 0, sizeof(IoCommand));

	cmd.eType = CommandType::GET_ZONE_INFO;
	cmd.eMode = CommandMode::SYSTEM_MONITOR_MODE;
	cmd.byNumOfTotalpackages = 1;
	cmd.byPackageIndex = 1;
	cmd.wDuration = NOT_USED;

	return this->m_IoProtocol->Write((unsigned char *)&(cmd), sizeof(cmd), &dwBytesWrite);
}
