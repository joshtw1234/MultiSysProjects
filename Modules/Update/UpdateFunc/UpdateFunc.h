// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the UPDATEFUNC_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// UPDATEFUNC_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef UPDATEFUNC_EXPORTS
#define UPDATEFUNC_API __declspec(dllexport)
#else
#define UPDATEFUNC_API __declspec(dllimport)
#endif


extern "C"
{
	UPDATEFUNC_API void Initialize(void);
	UPDATEFUNC_API void Uninitialize(void);
	UPDATEFUNC_API bool IsSupport(void);
	UPDATEFUNC_API void OMENPwerChange(void);
	UPDATEFUNC_API void ForeGroundChange(void);
};
