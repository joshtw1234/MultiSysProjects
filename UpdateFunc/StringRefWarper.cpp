//#include <windows.h>

#include "StringRefWarper.h"


StringRefWarper::StringRefWarper()
{
}

StringRefWarper::~StringRefWarper()
{
	WindowsDeleteString(_hstring);
}


