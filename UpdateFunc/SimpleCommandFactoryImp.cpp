#include <Windows.h>
#include "ISimpleCommandFactory.h"
#include "CommandHandler.h"
#include "SimpleCommandFactoryImp.h"

SimpleCommandFactoryImp::~SimpleCommandFactoryImp()
{
}

unique_ptr<ICommandHandler> SimpleCommandFactoryImp::GetCommandHandler(int command)
{
	switch (command)
	{
	case COMMAND_X:
	{
		return unique_ptr<ICommandHandler>(new CommandHandlerIsOverClockAble());
	}
	case COMMAND_GetOMENUpdate:
	{
		return unique_ptr<ICommandHandler>(new CommandHandlerGetkOMENUpdate());
	}
	case COMMAND_IsHPSARunning:
	{
		return unique_ptr<ICommandHandler>(new CommandHandlerIsHPSARunning());
	}

	default:
		return NULL;
	}
}
