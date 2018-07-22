#pragma once

#include "ICommandHandler.h"
#include <memory>

class ISimpleCommandFactory
{
public:
	virtual ~ISimpleCommandFactory() {}
	virtual std::unique_ptr<ICommandHandler> GetCommandHandler(int command) = 0;
};