#pragma once
#include "IPipeServerMethod.h"

class ICommandHandler
{
	public:
		virtual ~ICommandHandler() {}
		virtual void Process(IPipeServerMethod *pipeServerMethod, void* pipeInst, unsigned char* data, int size) = 0;
};