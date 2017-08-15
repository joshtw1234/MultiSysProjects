#pragma once
#include "ICommandHandler.h"
enum CommandID
{
	COMMAND_X = 0xB1,
	COMMAND_GetOMENUpdate = 0xB2,
	COMMAND_IsHPSARunning = 0xB3,
	COMMAND_GetNetWork = 0xB4,
};

class CommandHandlerIsOverClockAble : public ICommandHandler
{
public:
	virtual ~CommandHandlerIsOverClockAble();
	virtual void Process(IPipeServerMethod *pipeServerMethod, HANDLE pipeInst, unsigned char* data, int size);
};

class CommandHandlerGetkOMENUpdate : public ICommandHandler
{
public:
	virtual ~CommandHandlerGetkOMENUpdate();
	virtual void Process(IPipeServerMethod *pipeServerMethod, HANDLE pipeInst, unsigned char* data, int size);
};

class CommandHandlerInitHPSAThreadProc : public ICommandHandler
{
public:
	virtual ~CommandHandlerInitHPSAThreadProc();
	virtual void Process(IPipeServerMethod *pipeServerMethod, HANDLE pipeInst, unsigned char* data, int size);
};

class CommandHandlerIsHPSARunning : public ICommandHandler
{
public:
	virtual ~CommandHandlerIsHPSARunning();
	virtual void Process(IPipeServerMethod *pipeServerMethod, HANDLE pipeInst, unsigned char* data, int size);
};

class CommandHandlerGetNetWorkConnect : public ICommandHandler
{
public:
	virtual ~CommandHandlerGetNetWorkConnect();
	virtual void Process(IPipeServerMethod *pipeServerMethod, HANDLE pipeInst, unsigned char* data, int size);
};






