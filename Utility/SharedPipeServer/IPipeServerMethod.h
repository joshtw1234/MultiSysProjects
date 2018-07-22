#pragma once
#include "PipeData.h"

class IPipeServerMethod
{
	public:
		virtual ~IPipeServerMethod() {};
		virtual bool SendData(void* pipeInst, LPPipeData data) = 0;
		virtual bool SendData(LPPipeData data) = 0;
};