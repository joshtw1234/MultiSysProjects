// This is the main DLL file.

#include "stdafx.h"
#include "PipeClientBase.h"

void PipeClient::PipeClientBase::OnDoWork(System::Object ^sender, System::ComponentModel::DoWorkEventArgs ^e)
{	
	DWORD byteRead;

	while (!this->worker->CancellationPending)
	{
		if (this->protocol)
		{
			PipeData ^data = gcnew PipeData();
			data->Data = gcnew cli::array<BYTE>(this->bufferSize);
			if (this->protocol->Read(data))
			{
				this->worker->ReportProgress(0, (Object^) data);
			}
		}

		//Thread::Sleep(200);
	}
}


void PipeClient::PipeClientBase::OnProgressChanged(System::Object ^sender, System::ComponentModel::ProgressChangedEventArgs ^e)
{
	this->DataReceived(this, (PipeData ^)e->UserState);
}
