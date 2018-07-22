#pragma once
using namespace std;

class SimpleCommandFactoryImp : public ISimpleCommandFactory
{
public:
	virtual ~SimpleCommandFactoryImp();
	virtual unique_ptr<ICommandHandler> GetCommandHandler(int command);
};