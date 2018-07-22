#pragma once
typedef unsigned char BYTE;
typedef unsigned short WORD;

#define MAX_ZONE_SIZE 10
#define HID_CMD_SIGNATURE   0x43444948
#define NOT_USED 0
#define DEFAULT_BRIGHTNESS 50
#define MAX_BRIGHTNESS 100
#define DEFAULT_DURATION 3

enum CommandType : BYTE {
	GET_ZONE_INFO = 0x11,
	SET_ZONE_INFO = 0x12
};

enum CommandMode : BYTE {
	STATIC_COLOR_MODE = 0x1,
	COLOR_SHOW_MODE = 0x2,
	SYSTEM_MONITOR_MODE = 0x3,
	AUDIO_MONITOR_MODE = 0x4,
	LIGHTING_OFF_MODE = 0x5
};
//////// Basic 
typedef struct {
	BYTE byRed;
	BYTE byGreen;
	BYTE byBlue;
}LedZoneColorInfo, *LPLedZoneColorInfo;
///////////////////////

// Monitor mode
typedef struct {
	BYTE byindexOfPreset;
	BYTE byPercentage;
	BYTE byMonitor;
} LedZoneMonitorGroup, *LPLedZoneMonitorGroup;

typedef struct {
	union {
		LedZoneMonitorGroup GroupZone;
		LedZoneColorInfo LogoZone;
	}u;

}LedZoneMonitorInfo, *LPLedZoneMonitorInfo;
//

// Audio mode
typedef struct {
	LedZoneColorInfo ColorZone;
	BYTE byPercentage;
}LedZoneAudioInfo, *LPLedZoneAudioInfo;
//
#pragma pack(push)
#pragma pack(1) 

typedef struct {
	BYTE  byLength;
	CommandType eType;
	CommandMode eMode;
	BYTE byNumOfTotalpackages;
	BYTE byPackageIndex;
	WORD wDuration;
	union {
		LedZoneColorInfo  ColorZoneInfo[MAX_ZONE_SIZE];
		LedZoneMonitorInfo MonitorZoneInfo[MAX_ZONE_SIZE];
		LedZoneAudioInfo AudioZoneInfo[MAX_ZONE_SIZE];
	}LedZonesInfo;
	BYTE byBrightness;
	BYTE byArraySizeUsed;
	WORD wSignature;
	WORD wChecksum;


} IoCommand, *LPIoCommand;

//typedef struct {
//	int commandId;
//	unsigned char data[256];
//}PipeCommand;

enum PipeReceivedErrorCode : BYTE {
	ERROR_CAN_NOT_OPEN_USB_DEVICE = 0
};

enum PipeReceivedDataType : BYTE {
	TYPE_ERROR = 0,
	TYPE_PERFORMANCE_DATA = 1,
};

typedef struct 
{
	public:
		PipeReceivedDataType DataType;
		BYTE Data[254];
} PipeReceiveData;

typedef struct 
{
	public:
		PipeReceivedErrorCode ErrorCode;
} PipeReceiveError;

typedef struct {
	BYTE byCpuUsage;
	BYTE byCpuTemperature;
	BYTE byGpuUsage;
	BYTE byGpuTemperature;
}PerformanceData, *LPPerformanceData;

#pragma pack(pop)
