// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the COREAUDIOAPISDLL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// COREAUDIOAPISDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef COREAUDIOAPISDLL_EXPORTS
#define COREAUDIOAPISDLL_API __declspec(dllexport)
#else
#define COREAUDIOAPISDLL_API __declspec(dllimport)
#endif

// This class is exported from the dll
class COREAUDIOAPISDLL_API CCoreAudioApisDll {
public:
	CCoreAudioApisDll(void);
	// TODO: add your methods here.
};

//Value not export, need study
extern COREAUDIOAPISDLL_API int nCoreAudioApisDll;
extern "C"
{
	COREAUDIOAPISDLL_API int fnCoreAudioApisDll(void);
}
